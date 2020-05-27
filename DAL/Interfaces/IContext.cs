using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IContext<T> where T : class
    {
        T Add(T entity);

        T Read(KeyValuePair<string, Object> condition);

        List<T> ReadAll(Dictionary<string, Object> conditions, string keyword = "OR");

        T Update(KeyValuePair<string, Object> condition, KeyValuePair<string, Object> value);

        bool Delete(KeyValuePair<string, Object> condition);

        bool Exists(KeyValuePair<string, Object> condition);
    }
}