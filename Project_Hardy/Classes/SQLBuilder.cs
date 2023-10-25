using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Hardy.Classes
{
    internal class SQLBuilder
    {
        public static string select(string selection, string table, string where = "", string orderby = "", string groupby = "")
        {
            string sql = "SELECT " + selection + " FROM " + table;

            if (where != "")
            {
                sql += " WHERE " + where;
            }

            if (orderby != "")
            {
                sql += " ORDER BY " + orderby;
            }

            if (groupby != "")
            {
                sql += " GROUP BY " + groupby;
            }

            return sql;
        }

        public static string insert(string table, string attribs, string values)
        {
            return "INSERT INTO " + table + " (" + attribs + ") VALUES (" + values + ")";
        }

        public static string update(string table, string setStatement, string where)
        {
            return "UPDATE " + table + " SET " + setStatement + " WHERE " + where;
        }

        public static string deleteById(string table, int id)
        {
            if (id == null || id == 0)
            {
                return "";
            }
            return "DELETE FROM " + table + " WHERE id = " + id;
        }

        public static string delete(string table, string where)
        {
            if (where == null || where == "")
            {
                return "";
            }
            return "DELETE FROM " + table + " WHERE " + where;
        }
    }
}
