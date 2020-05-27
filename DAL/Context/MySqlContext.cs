using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DAL.Interfaces;
using DAL.MySQL.Database;
using Models;
using Models.Utils;

namespace DAL.MySQL.Contexts
{
    [ExcludeFromCodeCoverage]
    public abstract class MySqlContext<T> : IContext<T> where T : DatabaseItem
    {
        public Database.Database Database { get; }

        public MySqlContext(string connectionString)
        {
            Database = new Database.Database(connectionString);
        }

        public T Add(T entity)
        {
            var properties = (from t in typeof(T).GetProperties()
                where t.GetCustomAttributes<PropertyAttribute>().Any()
                select t).ToList();

            var values = new Dictionary<string, object>();
            var placeholders = new List<string>();

            foreach (var property in properties)
            {
                if (property.Name == "Id") continue;
                values.Add(property.Name.ToLower(), property.GetValue(entity));
                placeholders.Add("?");
            }

            entity.Id = Database
                .NonQuery(
                    $"INSERT INTO {typeof(T).Name.ToLower()} ({string.Join(", ", values.Keys)}) VALUES({string.Join(", ", placeholders)})",
                    values.Values.ToArray());


            return entity;
        }

        public T Read(KeyValuePair<string, Object> condition)
        {
            var results = Database.Query($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {condition.Key.ToLower()} = ?",
                condition.Value).ToList();
            if (!results.Any())
            {
                return null;
            }

            var result = results.First();
            return ModelCreator<T>.CreateInstance(result);
        }
        
        public List<T> ReadAll(Dictionary<string, Object> conditions, string keyword = "OR")
        {
            string query = conditions.First().Key + " = ?";
            foreach (var condition in conditions)
            {
                if (condition.Key != conditions.First().Key)
                {
                    query = query + " " + keyword + " " + condition.Key + " = ?";
                }
            }
            var results = Database.Query($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {query};",
                conditions.Values.ToArray()).ToList();
            var models = new List<T>();
            if (!results.Any())
            {
                return models;
            }

            foreach (var model in results)
            {
                models.Add(ModelCreator<T>.CreateInstance(model));
            }
            return models;
        }

        public T Update(KeyValuePair<string, Object> condition, KeyValuePair<string, Object> value)
        {
            Database.Query(
                $"UPDATE {typeof(T).Name.ToLower()} SET {value.Key.ToLower()} = ? WHERE {condition.Key.ToLower()} = ?", value.Value, condition.Value);
            return null;
        }

        public bool Delete(KeyValuePair<string, Object> condition)
        {
            throw new System.NotImplementedException();
        }
        
        public bool Exists(KeyValuePair<string, Object> condition)
        {
            var results = Database.Query($"SELECT COUNT(*) FROM {typeof(T).Name.ToLower()} WHERE {condition.Key.ToLower()} = ?",
                condition.Value).ToList();
            return (int)(long)results.First().Properties.First().Value != 0;
        }
    }
}