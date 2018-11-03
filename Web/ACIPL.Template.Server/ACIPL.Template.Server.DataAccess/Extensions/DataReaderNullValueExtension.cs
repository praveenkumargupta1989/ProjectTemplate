using System;
using System.Data;

namespace ACIPL.Template.Server.DataAccess.Extensions
{
    public static class DataReaderNullValueExtension
    {
        public static TResult Get<TResult>(this IDataReader reader, string name)
        {
            return reader.Get<TResult>(reader.GetOrdinal(name));
        }

        public static TResult Get<TResult>(this IDataReader reader, int c)
        {
            return ConvertTo<TResult>.From(reader[c]);
        }


        public static TResult Get<TResult>(this IDataRecord record, string name)
        {
            return record.Get<TResult>(record.GetOrdinal(name));
        }

        public static TResult Get<TResult>(this IDataRecord record, int c)
        {
            return ConvertTo<TResult>.From(record[c]);
        }

        public static bool IsNullableType(this Type type)
        {
            return
                (type.IsGenericType && !type.IsGenericTypeDefinition) &&
                (typeof(Nullable<>) == type.GetGenericTypeDefinition());
        }

    }
}