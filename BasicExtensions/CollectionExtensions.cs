using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BasicExtensions
{
    public static class CollectionExtensions
    {
        public static List<T> ToList<T>(this DataSet ds) => ds.Tables[typeof(T).Name].ToList<T>();
        public static List<T> ToList<T>(this DataTable dt)
        {
            var result = new List<T>();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                var val = Activator.CreateInstance<T>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!dt.Columns.Contains(prop.Name))
                        continue;
                    if (row[prop.Name] == DBNull.Value)
                        continue;
                    prop.SetValue(val, row[prop.Name], null);
                }
                result.Add(val);
            }
            return result;
        }
        public static List<T> CopyList<T>(this List<T> entities)
        {
            try
            {
                var result = new T[entities.Count];
                entities.CopyTo(result);
                return result.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static IEnumerable<IEnumerable<T>> Paging<T>(this List<T> src, int pageSize = 100)
        {
            System.Diagnostics.Contracts.Contract.Requires(src != null);
            System.Diagnostics.Contracts.Contract.Requires(pageSize > 0);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<List<List<T>>>() != null);
            using (var enumerator = src.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var currentPage = new List<T>(pageSize)
                    {
                        enumerator.Current
                    };
                    while (currentPage.Count < pageSize && enumerator.MoveNext())
                    {
                        currentPage.Add(enumerator.Current);
                    }
                    yield return currentPage;
                }
            }
        }
        public static bool HasItems<T>(this IEnumerable<T> src) => src != null && src.Any();
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
    }
}
