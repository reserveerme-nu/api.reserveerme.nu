﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace DAL.MySQL.Database
{
    [ExcludeFromCodeCoverage]
    public sealed class Database
    {
        private MySqlConnection databaseConnection;
        
        public Database(string connectionString)
        {
            databaseConnection = new MySqlConnection(connectionString);
        }

        public void SetConnection(string host, string username, string password, string database)
        {
            databaseConnection = new MySqlConnection("server=" + host + ";user id=" + username + ";password=" + password + ";database=" + database);
        }

        private void OpenConnection()
        {
            try
            {
                databaseConnection.Open();                
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception("Could not connect to database");
            }
        }

        private void CloseConnection()
        {
            try
            {
                databaseConnection.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception("Could not disconnect from database");
            }
        }

        public IEnumerable<QueryResult> Query(string query, params object[] parameters)
        {
            var results = new List<QueryResult>();
            var databaseQuery = new MySqlCommand(query, databaseConnection);

            if (parameters != null)
            {
                for (var i = 1; i < parameters.Length + 1; i++)
                {
                    databaseQuery.Parameters.AddWithValue("param" + i, parameters[i - 1]);
                }
            }

            OpenConnection();
            try
            {
                var dataReader = databaseQuery.ExecuteReader();

                while (dataReader.Read())
                {
                    var result = new QueryResult(dataReader);
                    results.Add(result);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                CloseConnection();
            }

            return results;
        }
        
        public int NonQuery(string query, params object[] parameters)
        {
            var command = new MySqlCommand(query, databaseConnection);
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                command.Parameters.AddWithValue($"param{i}", parameter);
            }

            command.CommandText += ";SELECT LAST_INSERT_ID();";
            OpenConnection();
            var result = command.ExecuteScalar();        
            var id = Convert.ToInt32(result);
            CloseConnection();

            return id;
        }
        
        public long Scalar(string query, List<string> parameters)
        {
            var databaseQuery = new MySqlCommand(query, databaseConnection);

            if (parameters != null)
            {
                var param = 0;
                foreach (var parameter in parameters)
                {
                    databaseQuery.Parameters.AddWithValue($"param{param}", parameter);
                    param++;
                }
            }

            OpenConnection();
            try
            {
                databaseQuery.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                CloseConnection();
            }

            return databaseQuery.LastInsertedId;
        }

        public List<QueryResult> Procedure(string procedure, params object[] parameters)
        {
            var results = new List<QueryResult>();
            var databaseQuery = new MySqlCommand(procedure, databaseConnection);
            databaseQuery.CommandType = CommandType.StoredProcedure;

            foreach (var parameter in parameters)
            {
                databaseQuery.Parameters.AddWithValue($"param{(Array.IndexOf(parameters, parameter) + 1).ToString()}", parameter);
            }
            
            
            OpenConnection();

            try
            {
                var dataReader = databaseQuery.ExecuteReader();
                while (dataReader.Read())
                {
                    var result = new QueryResult(dataReader);
                    results.Add(result);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                CloseConnection();
            }
            
            return results;
        }
    }
}
