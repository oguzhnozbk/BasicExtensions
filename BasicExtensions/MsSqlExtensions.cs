using BasicExtensions.Attribute;
using System;
using System.Linq;

namespace BasicExtensions
{
    public static class MsSqlExtensions
    {
        public static string CreateSqlTableControlScript<T>() where T : class, new()
        {
            string sql = "IF(EXISTS\n"
                   + "(\n"
                   + "    SELECT 1\n"
                   + "    FROM INFORMATION_SCHEMA.TABLES\n"
                   + "    WHERE TABLE_SCHEMA = 'dbo'\n"
                   + $"          AND TABLE_NAME = '{typeof(T).Name}'\n"
                   + "))\n"
                   + "    BEGIN\n"
                   + $"        {GetTableAlter<T>()}"
                   + "    END;\n"
                   + "    ELSE\n"
                   + "    BEGIN\n"
                   + $"       {GetTableCreate<T>()}\n"
                   + "    END;";

            return sql;
        }
        private static string GetTableCreate<T>() where T : class, new()
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(s => {
                var attribute = s.GetCustomAttributes(typeof(IgnoreColumnAttribute), false);
                if (attribute != null && attribute.Length > 0)
                    return false;
                return true;
            }).ToArray();
            var sql = $"CREATE TABLE [{type.Name}] ( \n";
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var script = GetColumnScript(property);
                if (!string.IsNullOrWhiteSpace(script))
                {
                    sql += script;
                    if (properties.Length - 1 != i)
                        sql += " , \n";
                }
            }
            sql += " );";
            return sql;
        }
        private static string GetTableAlter<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(s => {
                var attribute = s.GetCustomAttributes(typeof(IgnoreColumnAttribute), false);
                if (attribute != null && attribute.Length > 0)
                    return false;
                return true;
            }).ToArray();
            var sql = "";
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var script = GetAlterColumn(type.Name, property);
                if (!string.IsNullOrWhiteSpace(script))
                    sql += $"{script} \n";
            }
            return sql;
        }
        private static bool IsNullable(System.Reflection.PropertyInfo property) => property.GetCustomAttributes(typeof(RequiredColumnAttribute), false) != null && property.GetCustomAttributes(typeof(RequiredColumnAttribute), false).Length > 0 ? false : property.PropertyType.IsGenericType && property.PropertyType.Name.ToLower().Contains("nullable") ? true : property.PropertyType.Name == typeof(string).Name ? true : false;
        private static string GetPropertyName(System.Reflection.PropertyInfo property) => property.PropertyType.IsGenericType ? property.PropertyType.Name.ToLower().Contains("nullable") ? property.PropertyType.GenericTypeArguments[0].Name : "" : property.PropertyType.Name;
        private static bool IsEnumType(System.Reflection.PropertyInfo property) => property.PropertyType.IsGenericType ? property.PropertyType.Name.ToLower().Contains("nullable") ? property.PropertyType.GenericTypeArguments[0].IsEnum : false : property.PropertyType.IsEnum;
        private static bool IsValueType(System.Reflection.PropertyInfo property) => property.PropertyType.IsGenericType ? property.PropertyType.Name.ToLower().Contains("nullable") ? property.PropertyType.GenericTypeArguments[0].IsValueType : false : property.PropertyType.IsValueType;
        private static string GetColumnType(System.Reflection.PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                if (IsEnumType(property))
                    return "INT";
                else if (IsValueType(property))
                {
                    var propertyName = GetPropertyName(property);
                    if (propertyName == typeof(DateTime).Name)
                        return "DATETIME";
                    else if (propertyName == typeof(int).Name || propertyName == typeof(short).Name)
                        return "INT";
                    else if (propertyName == typeof(long).Name)
                        return "BIGINT";
                    else if (propertyName == typeof(double).Name || propertyName == typeof(float).Name)
                        return "FLOAT";
                    else if (propertyName == typeof(decimal).Name)
                        return "DECIMAL";
                    else if (propertyName == typeof(bool).Name)
                        return "BIT";
                }
                else if (property.PropertyType.Name == typeof(string).Name)
                    return "NVARCHAR";
            }
            return string.Empty;
        }
        private static string GetColumnScript(System.Reflection.PropertyInfo property) => string.IsNullOrWhiteSpace(GetColumnType(property)) ? "" : $" [{property.Name}] [{GetColumnType(property)}] {GetColumnTypeLength(property)} {(IsNullable(property) ? "" : "NOT")} NULL ";
        private static string GetColumnTypeLength(System.Reflection.PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                if (property.PropertyType.Name == typeof(string).Name)
                {
                    var attribute = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                    var length = 64;
                    if (attribute != null && attribute.Length > 0)
                        length = (attribute[0] as ColumnLengthAttribute).Length;
                    return $" ({length}) ";
                }
                else if (GetPropertyName(property) == typeof(decimal).Name)
                {
                    var attribute = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                    var precision = 15;
                    var scale = 3;
                    if (attribute != null && attribute.Length > 0)
                    {
                        precision = (attribute[0] as ColumnLengthAttribute).Precision;
                        scale = (attribute[0] as ColumnLengthAttribute).Scale;
                    }
                    return $" ({precision},{scale}) ";
                }
            }
            return "";
        }
        private static string GetAlterColumn(string tableName, System.Reflection.PropertyInfo property)
        {
            var columnType = GetColumnType(property);
            if (string.IsNullOrWhiteSpace(columnType))
                return "";
            var columnName = property.Name;

            var columnLengthScript = string.Empty;
            var columnTypeScript = columnType;
            var isNullable = IsNullable(property);
            if (columnType == "NVARCHAR")
            {
                var attribute = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                var length = 64;
                if (attribute != null && attribute.Length > 0)
                    length = (attribute[0] as ColumnLengthAttribute).Length;
                columnLengthScript = $"AND CHARACTER_MAXIMUM_LENGTH = {length}";
                columnTypeScript = $"NVARCHAR ({length})";
            }
            else if (columnType == "DECIMAL")
            {
                var attribute = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                var precision = 15;
                var scale = 3;
                if (attribute != null && attribute.Length > 0)
                {
                    precision = (attribute[0] as ColumnLengthAttribute).Precision;
                    scale = (attribute[0] as ColumnLengthAttribute).Scale;
                }
                columnLengthScript = $"AND NUMERIC_PRECISION = {precision} AND NUMERIC_SCALE = {scale}";
                columnTypeScript = $"DECIMAL ({precision},{scale})";
            }
            string sql = $"IF(EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'dbo' AND table_name = '{tableName}' AND COLUMN_NAME = '{columnName}'))\n"
                + "BEGIN\n"
                + $"    IF(NOT EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'dbo' AND table_name = '{tableName}' AND COLUMN_NAME = '{columnName}' AND DATA_TYPE = '{GetColumnType(property)}' AND IS_NULLABLE='{(isNullable ? "YES" : "NO")}' {columnLengthScript} )) \n"
                + "    BEGIN\n"
                + $"        ALTER TABLE {tableName} ALTER COLUMN {columnName} {columnTypeScript} {(isNullable ? "" : "NOT")} NULL;\n"
                + "    END;\n"
                + "END;\n"
                + "ELSE\n"
                + "BEGIN\n"
                + $"    ALTER TABLE {tableName} ADD {columnName} {columnTypeScript} {(isNullable ? "" : "NOT")} NULL;\n"
                + "END;";
            return sql;
        }
    }
}
