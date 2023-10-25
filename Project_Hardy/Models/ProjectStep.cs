using Project_Hardy.Classes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Project_Hardy.Models
{
    public class ProjectStep
    {
        public int id { get; set; }

        public string identifier { get; set; }

        public string description { get; set; }

        public int duration { get; set; }

        public string prev_identifier { get; set; }

        public bool deletionMark { get; set; }

        public bool is_empty
        {
            get
            {
                return (
                    identifier == null && description == null &&
                    (duration == null || duration <= 0) && prev_identifier == null
                );
            }
        }

        public void persist(int project_id)
        {
            if (is_empty) return;

            if (identifier != null)
            {
                identifier = identifier.ToUpper();
            }
            if (prev_identifier != null)
            {
                prev_identifier = prev_identifier.ToUpper();
            }

            if (id == 0 || id == null)
            {
                string duplicate_sql = Project.stepsQuery("p.id = " + project_id + " AND s.identifier = '" + identifier + "'");
                var duplicate_res = DBWorker.query(duplicate_sql);

                if (duplicate_res.HasRows)
                {
                    deletionMark = true;
                    return;
                }

                var max_id = DBWorker.query(SQLBuilder.select("MAX(id) AS max_id", "project_step"));
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

                insert(project_id);
                return;
            }

            var res = DBWorker.query(SQLBuilder.select("*", "project_step", "id = " + id));

            if (!res.HasRows)
            {
                insert(project_id);
            }

            while (res.Read())
            {
                if (Convert.ToString(res["identifier"]) != this.identifier)
                {
                    this.update("identifier", this.identifier);
                }
                if (Convert.ToString(res["description"]) != this.description)
                {
                    this.update("description", this.description);
                }
                if (Convert.ToInt32(res["duration"]) != this.duration)
                {
                    this.update("duration", this.duration);
                }
                if (Convert.ToString(res["prev_identifier"]) != this.prev_identifier)
                {
                    this.update("prev_identifier", this.prev_identifier);
                }
            }
        }

        private void insert(int project_id)
        {
            string sql = SQLBuilder.insert("project_step", "identifier, description, duration, prev_identifier",
                "'" + identifier + "', " + "'" + description + "', " + duration + ", '" + prev_identifier + "'"
            );
            DBWorker.query(sql);

            string sql_check = SQLBuilder.select("id", "project_step",
                "identifier = '" + identifier + "' AND " +
                "description = '" + description + "' AND " +
                "duration = " + duration + " AND " +
                "prev_identifier = '" + prev_identifier + "' "
                );

            var res_check = DBWorker.query(sql_check);
            if (res_check.HasRows)
            {
                while (res_check.Read())
                {
                    id = Convert.ToInt32(res_check["id"]);
                }
            }

            string insert_mm = SQLBuilder.insert("project_mm_step", "project_id, step_id", project_id + ", " + this.id);
            DBWorker.query(insert_mm);
        }

        private void update(string attr, string val)
        {
            string sql = SQLBuilder.update("project_step", attr + " = '" + val + "'", "id = " + this.id);
            DBWorker.query(sql);
        }

        private void update(string attr, int val)
        {
            string sql = SQLBuilder.update("project_step", attr + " = " + val + "", "id = " + this.id);
            DBWorker.query(sql);
        }

        public void delete()
        {
            string sql = SQLBuilder.deleteById("project_step", this.id);
            DBWorker.query(sql);

            sql = SQLBuilder.delete("project_mm_step", "step_id = " + this.id);
            DBWorker.query(sql);
        }

        public void debugOutput()
        {
            Console.WriteLine(
                "Identifier: " + identifier + Environment.NewLine +
                "Description: " + description + Environment.NewLine +
                "Duration: " + duration + Environment.NewLine +
                "Previous Identifer: " + prev_identifier + Environment.NewLine
                );
        }


        public static List<ProjectStep> findAll(string where = "", string orderby = "", string groupby = "")
        {
            var list = new List<ProjectStep>();
            var res = DBWorker.query(SQLBuilder.select("*", "project_step", where, orderby, groupby));

            if (!res.HasRows)
            {
                return list;
            }

            while (res.Read())
            {
                var tmp = new ProjectStep();
                mapAttributes(ref res, ref tmp);
                list.Add(tmp);
            }

            return list;
        }

        public static ProjectStep findById(int id)
        {
            var res = DBWorker.query(SQLBuilder.select("*", "project_step", "id = " + id));

            if (!res.HasRows)
            {
                return null;
            }

            while (res.Read())
            {
                var tmp = new ProjectStep();
                mapAttributes(ref res, ref tmp);
                return tmp;
            }

            return null;
        }

        private static void mapAttributes(ref SQLiteDataReader data, ref ProjectStep e)
        {
            e.id = Convert.ToInt32(data["id"]);
            e.identifier = Convert.ToString(data["identifier"]);
            e.description = Convert.ToString(data["description"]);
            e.duration = Convert.ToInt32(data["duration"]);
            e.prev_identifier = Convert.ToString(data["prev_identifier"]);
        }
    }
}
