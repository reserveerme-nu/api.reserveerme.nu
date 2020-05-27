using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace DAL.MySQL.Database
{
    [ExcludeFromCodeCoverage]
    public class QueryResult
    {
        public List<KeyValuePair<string, object>> Properties { get; private set; } =
            new List<KeyValuePair<string, object>>();

        public object this[string property] {
            get { return Properties.Find(x => x.Key == property).Value; }
        }

        public QueryResult(MySqlDataReader DataReader)
        {
            for (int i = 0; i < DataReader.FieldCount; i++)
            {
                Properties.Add(DataReader.GetValue(i).GetType().ToString() != "System.DBNull"
                    ? new KeyValuePair<string, object>(DataReader.GetName(i), DataReader.GetValue(i))
                    : new KeyValuePair<string, object>(DataReader.GetName(i), ""));
            }
        }
    }
}