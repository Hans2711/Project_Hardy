using System;
using System.Data.SQLite;

namespace Project_Hardy.Classes
{
    internal class DBWorker
    {
        public static SQLiteConnection conn;

        public static bool install(string filepath)
        {
            conn = new SQLiteConnection("Data Source=" + filepath);

            try
            {
                conn.Open();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }

        public static SQLiteDataReader query(string sql)
        {
            Console.WriteLine(sql);
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            return cmd.ExecuteReader();
        }
    }
}
