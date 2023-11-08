using Project_Hardy.Classes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;

namespace Project_Hardy.Models
{
    public class Project
    {
        public int id { get; set; }

        public string description { get; set; }

        public DateTime starttime { get; set; }

        public DateTime endtime { get; set; }

        public Employee project_manager { get; set; }

        public List<ProjectStep> steps = new List<ProjectStep>();

        public int stepCount { get { return this.steps.Count; } }

        public Project()
        {
            endtime = DateTime.Now;
            starttime = DateTime.Now;

            steps = new List<ProjectStep>();

            project_manager = new Employee();
        }

        public void persist()
        {
            if (id == 0 || id == null)
            {
                var max_id = DBWorker.query(SQLBuilder.select("MAX(id) AS max_id", "project"));
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

            var res = DBWorker.query(SQLBuilder.select("*", "project", "id = " + id));

            if (!res.HasRows)
            {
                insert();
            }

            while (res.Read())
            {
                updateProjectManager();
                if (Convert.ToString(res["description"]) != this.description)
                {
                    this.update("description", this.description);
                }
                if (Convert.ToString(res["starttime"]) != "" + TimestampUtility.toTimestamp(this.starttime))
                {
                    this.update("starttime", TimestampUtility.toTimestamp(this.starttime));
                }
                if (Convert.ToString(res["endtime"]) != "" + TimestampUtility.toTimestamp(this.endtime))
                {
                    this.update("endtime", TimestampUtility.toTimestamp(this.endtime));
                }

                string sql = Project.stepsQuery(this.id);
                var db_steps = DBWorker.query(sql);

                if (db_steps.HasRows)
                {
                    while (db_steps.Read())
                    {
                        bool deleted = true;
                        foreach (var step in steps)
                        {
                            if (Convert.ToInt32(db_steps["id"]) == step.id)
                            {
                                deleted = false;
                            }
                        }
                        if (deleted)
                        {
                            ProjectStep s = new ProjectStep();
                            s.id = Convert.ToInt32(db_steps["id"]);
                            s.delete();
                            s = null;
                        }
                    }
                }

                foreach (var step in steps)
                {
                    step.persist(this.id);
                }

            }
        }

        private void updateProjectManager()
        {
            int project_manager_id = 0;
            if (this.project_manager != null)
            {
                if (string.IsNullOrEmpty(this.project_manager.fullname))
                    return;

                foreach (var employee in Employee.findAll())
                {
                    if (employee.firstname == project_manager.firstname && employee.surname == project_manager.surname)
                    {
                        project_manager_id = employee.id;
                        break;
                    }
                }

                if (project_manager_id == 0)
                {
                    string fullname = this.project_manager.fullname;
                    this.project_manager = new Employee();
                    this.project_manager.fullname = fullname;

                    this.project_manager.persist();
                    project_manager_id = this.project_manager.id;
                }

                this.update("project_manager_id", project_manager_id);

            }
        }

        private void insert()
        {
            int project_manager_id = 0;

            string sql = SQLBuilder.insert("project", "description, starttime, endtime, project_manager_id",
                "'" + description + "', " + "" + TimestampUtility.toTimestamp(this.starttime) + ", " + TimestampUtility.toTimestamp(this.endtime) + ", " + project_manager_id + ""
            );
            DBWorker.query(sql);

            string sql_check = SQLBuilder.select("id", "project",
                "description = '" + description + "' AND " +
                "starttime = " + TimestampUtility.toTimestamp(this.starttime) + " AND " +
                "endtime = " + TimestampUtility.toTimestamp(this.endtime) + " AND " +
                "project_manager_id = " + project_manager_id + " "
            );

            var res_check = DBWorker.query(sql_check);
            if (res_check.HasRows)
            {
                while (res_check.Read())
                {
                    id = Convert.ToInt32(res_check["id"]);
                }
            }

            updateProjectManager();

            foreach (var step in steps)
            {
                step.persist(this.id);
            }
        }
        private void update(string attr, string val)
        {
            string sql = SQLBuilder.update("project", attr + " = '" + val + "'", "id = " + this.id);
            DBWorker.query(sql);
        }

        private void update(string attr, int val)
        {
            string sql = SQLBuilder.update("project", attr + " = " + val + "", "id = " + this.id);
            DBWorker.query(sql);
        }

        public void delete()
        {
            string sql = SQLBuilder.deleteById("project", this.id);
            DBWorker.query(sql);

            foreach (var step in steps)
            {
                step.delete();
            }
        }

        public void debugOutput()
        {
            Console.WriteLine(
                "Description: " + description + Environment.NewLine +
                "Starttime: " + this.starttime.ToString(new CultureInfo("de-DE")) + Environment.NewLine +
                "StarttimeTimeStamp: " + TimestampUtility.toTimestamp(this.starttime) + Environment.NewLine +
                "Endtime: " + this.endtime.ToString(new CultureInfo("de-DE")) + Environment.NewLine +
                "EndtimeTimeStamp: " + TimestampUtility.toTimestamp(this.endtime) + Environment.NewLine +
                "ProjectManager: " + this.project_manager.firstname + " " + this.project_manager.surname + Environment.NewLine +
                "Steps:" + Environment.NewLine
            );

            foreach (var step in steps)
            {
                step.debugOutput();
            }
        }

        public static List<Project> findAll(string where = "", string orderby = "", string groupby = "")
        {
            var list = new List<Project>();
            var res = DBWorker.query(SQLBuilder.select("*", "project", where, orderby, groupby));

            if (!res.HasRows)
            {
                return list;
            }

            while (res.Read())
            {
                var tmp = new Project();
                mapAttributes(ref res, ref tmp);
                list.Add(tmp);
            }

            return list;
        }

        public static Project findById(int id)
        {
            var res = DBWorker.query(SQLBuilder.select("*", "project", "id = " + id));

            if (!res.HasRows)
            {
                return null;
            }

            while (res.Read())
            {
                var tmp = new Project();
                mapAttributes(ref res, ref tmp);
                return tmp;
            }

            return null;
        }

        private static void mapAttributes(ref SQLiteDataReader data, ref Project e)
        {
            e.id = Convert.ToInt32(data["id"]);
            e.description = Convert.ToString(data["description"]);
            e.starttime = TimestampUtility.toDateTime(Convert.ToInt32(data["starttime"]));
            e.endtime = TimestampUtility.toDateTime(Convert.ToInt32(data["endtime"]));

            int project_manager_id = Convert.ToInt32(data["project_manager_id"]);
            e.project_manager = Employee.findById(project_manager_id);

            if (e.project_manager == null)
                e.project_manager = new Employee();

            string sql = stepsQuery(e.id);
            var res = DBWorker.query(sql);

            if (res.HasRows)
            {
                while (res.Read())
                {
                    e.steps.Add(ProjectStep.findById(Convert.ToInt32(res["id"])));
                }
            }
        }

        public static string stepsQuery(int project_id)
        {
            return SQLBuilder.select("s.id",
                "Project_Step AS s JOIN Project_MM_Step as ss ON (ss.step_id = s.id) JOIN Project as p ON (p.id = ss.project_id)",
                "p.id = " + project_id,
                "s.identifier ASC"
            );
        }

        public static string stepsQuery(string where)
        {
            return SQLBuilder.select("s.id",
                "Project_Step AS s JOIN Project_MM_Step as ss ON (ss.step_id = s.id) JOIN Project as p ON (p.id = ss.project_id)",
                where,
                "s.identifier ASC"
            );
        }

        public int getTotalStepDuration()
        {
            int totalStepDuration = 0;
            foreach (var step in steps)
            {
                int endDuration = step.duration + backtrackPrevDurations(step.prev_identifier);
                if (endDuration > totalStepDuration)
                {
                    totalStepDuration = endDuration;
                }
            }
            return totalStepDuration;
        }

        public int backtrackPrevDurations(string identifier)
        {
            if (identifier == "" || identifier == null)
            {
                return 0;
            }

            int total = 0;
            for (int i = 0; i < stepCount; i++)
            {
                if (identifier == steps[i].identifier)
                {
                    total += steps[i].duration;
                    identifier = steps[i].prev_identifier;
                    i = -1;
                }
            }
            return total;
        }

        public int maxTraceDuration = 0;
        public void properlyOrderSteps()
        {
            List<string> potentialStartingIdentifiers = new List<string>();

            foreach (var step in steps)
            {
                if (step.prev_identifier == null || step.prev_identifier == "")
                {
                    potentialStartingIdentifiers.Add(step.identifier);
                }
            }

            List<string> trace = new List<string>();
            int lastLength = 0;

            foreach (var step in potentialStartingIdentifiers)
            {
                stepTrace = new List<string>();
                stepTrace.Add(step);
                emergencyBreaker = 100;
                stepTree(step);

                if (stepTrace.Count > lastLength)
                {
                    trace = stepTrace.ToList();
                    lastLength = stepTrace.Count;
                }
            }

            List<ProjectStep> newSteps = new List<ProjectStep>();

            foreach (var step in trace)
            {
                for (int i = 0; i < steps.Count; i++)
                {
                    if (steps[i].identifier == step)
                    {
                        newSteps.Add(steps[i]);
                        steps.RemoveAt(i);
                    }
                }
            }

            for (int i = 0; i < steps.Count; i++)
            {
                newSteps.Add(steps[i]);
            }

            steps = newSteps;
        }

        private List<string> stepTrace = new List<string>();
        private int emergencyBreaker = 100;
        private void stepTree(string identifier)
        {
            if (emergencyBreaker <= 0)
                return;
            emergencyBreaker--;

            foreach (var step in steps)
            {
                if (step.prev_identifier == identifier)
                {
                    stepTrace.Add(step.identifier);
                    stepTree(step.identifier);
                }
            }
        }
    }
}
