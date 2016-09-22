using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Helpers
{
    public static class DapperConfig
    {
        public static void ConfigureColumnNameMapping<T>()
        {
            var typeMap = new CustomPropertyTypeMap(typeof(T), (type, columnName) =>
                type.GetProperties().FirstOrDefault(prop =>
                    prop.GetCustomAttributes(inherit: false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName)));
            Dapper.SqlMapper.SetTypeMap(typeof(T), typeMap);
        }
    }
}
