using Project_Hardy.Classes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Project_Hardy.Models
{
    public class Employee
    {
        public int id { get; set; }

        public string firstname { get; set; }

        public string surname { get; set; }

        public string department { get; set; }

        public string phone { get; set; }

        public string fullname
        {
            get
            {
                if (firstname != null && firstname != "" && surname != null && surname != "")
                {
                    return firstname + " " + surname;
                }
                if (firstname != null && firstname != "")
                {
                    return firstname;
                }
                if (surname != null && surname != "")
                {
                    return surname;
                }

                return "";
            }
            set
            {
                string[] parts = value.Split(' ');
                if (parts.Length == 2)
                {
                    firstname = parts[0];
                    surname = parts[1];
                }
                if (parts.Length == 1)
                {
                    firstname = parts[0];
                    surname = string.Empty;
                }
                if (parts.Length >= 3)
                {
                    firstname = parts[0];
                    parts = parts.Skip(1).ToArray();
                    surname = string.Join(" ", parts);
                }
            }
        }

        public void persist()
        {
            if (id == 0 || id == null)
            {
                var max_id = DBWorker.query(SQLBuilder.select("MAX(id) AS max_id", "employee"));
                if (max_id.HasRows)
                {
                    while (max_id.Read())
                    {
                        if (max_id["max_id"].GetType() != typeof(DBNull))
                        {
                            this.id = Convert.ToInt32(max_id["max_id"]) + 1;
                        }
                    }
                }
                else
                {
                    id = 1;
                }

                insert();
                return;
            }

            var res = DBWorker.query(SQLBuilder.select("*", "employee", "id = " + id));

            if (!res.HasRows)
            {
                insert();
            }

            while (res.Read())
            {
                if (Convert.ToString(res["firstname"]) != this.firstname)
                {
                    this.update("firstname", this.firstname);
                }
                if (Convert.ToString(res["surname"]) != this.surname)
                {
                    this.update("surname", this.surname);
                }
                if (Convert.ToString(res["department"]) != this.department)
                {
                    this.update("department", this.department);
                }
                if (Convert.ToString(res["phone"]) != this.phone)
                {
                    this.update("phone", this.phone);
                }
            }
        }

        private void insert()
        {
            string sql = SQLBuilder.insert("employee", "firstname, surname, department, phone",
                "'" + firstname + "', " + "'" + surname + "', '" + department + "', '" + phone + "'"
            );
            var res = DBWorker.query(sql);

            string sql_check = SQLBuilder.select("id", "employee",
                "firstname = '" + firstname + "' AND " +
                "surname = '" + surname + "' AND " +
                "department = '" + department + "' AND " +
                "phone = '" + phone + "' "
                );

            var res_check = DBWorker.query(sql_check);
            if (res_check.HasRows)
            {
                while (res_check.Read())
                {
                    id = Convert.ToInt32(res_check["id"]);
                }
            }
        }

        private void update(string attr, string val)
        {
            string sql = SQLBuilder.update("employee", attr + " = '" + val + "'", "id = " + this.id);
            DBWorker.query(sql);
        }

        public void delete()
        {
            string sql = SQLBuilder.deleteById("employee", this.id);
            DBWorker.query(sql);

            sql = SQLBuilder.update("Project", "project_manager_id = null", "project_manager_id = " + this.id);
            DBWorker.query(sql);
        }

        public void debugOutput()
        {
            Console.WriteLine(
                "Firstname: " + firstname + Environment.NewLine +
                "Surname: " + surname + Environment.NewLine +
                "Department: " + department + Environment.NewLine +
                "Phone: " + phone + Environment.NewLine
                );
        }

        public static List<Employee> findAll(string where = "", string orderby = "", string groupby = "")
        {
            var list = new List<Employee>();
            var res = DBWorker.query(SQLBuilder.select("*", "employee", where, orderby, groupby));

            if (!res.HasRows)
            {
                return list;
            }

            while (res.Read())
            {
                var tmp = new Employee();
                mapAttributes(ref res, ref tmp);
                list.Add(tmp);
            }

            return list;
        }

        public static Employee findById(int id)
        {
            var res = DBWorker.query(SQLBuilder.select("*", "employee", "id = " + id));

            if (!res.HasRows)
            {
                return null;
            }

            while (res.Read())
            {
                var tmp = new Employee();
                mapAttributes(ref res, ref tmp);
                return tmp;
            }

            return null;
        }

        private static void mapAttributes(ref SQLiteDataReader data, ref Employee e)
        {
            e.id = Convert.ToInt32(data["id"]);
            e.firstname = Convert.ToString(data["firstname"]);
            e.surname = Convert.ToString(data["surname"]);
            e.department = Convert.ToString(data["department"]);
            e.phone = Convert.ToString(data["phone"]);
        }
    }
}
