using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Models.Utils;

namespace DAL.MySQL.Database
{
    [ExcludeFromCodeCoverage]
    public static class ModelCreator<T>
    {
        public static T CreateInstance(QueryResult queryResult)
        {
            var model = (T) Activator.CreateInstance(typeof(T));

            var properties = (from t in typeof(T).GetProperties()
                where t.GetCustomAttributes<PropertyAttribute>().Any()
                select t).ToList();

            foreach (var (key, value) in queryResult.Properties)
            {
                var found = properties.Find(prop => prop.Name.ToLower() == key);
                if (found != null)
                {
                    Console.WriteLine(found.PropertyType);
                    if (found.PropertyType == typeof(Guid))
                    { 
                        found.SetValue(model, Guid.Parse(value.ToString()));
                    }
                    else if(found.PropertyType == typeof(int) && value != "")
                    {
                        found.SetValue(model, value);
                    }
                    else if (found.PropertyType != typeof(int?))
                    {
                        found.SetValue(model, value);
                    }
                    else if (found.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        if(found.PropertyType.GenericTypeArguments[0] == typeof(int) && value != "")
                        {
                            found.SetValue(model, (int?)value);
                        }
                    }
                }
            }

            return model;
        }
    }

}