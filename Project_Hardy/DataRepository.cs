using Project_Hardy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Hardy
{
    internal class DataRepository
    {
        public List<Employee> employees;

        public List<Project> projects;

        public void fetchAll()
        {
            employees = Employee.findAll();
            projects = Project.findAll();
        }

        public void persistAll()
        {
            if (employees == null || projects == null)
            {
                fetchAll();
            }

            foreach (Employee employee in employees)
            {
                employee.persist();
            }

            foreach (Project project in projects)
            {
                project.persist();
            }
        }

        public void debutOutput()
        {
            if (employees == null || projects == null)
            {
                fetchAll();
            }

            Console.WriteLine("Employees: ");
            foreach (Employee employee in employees)
            {
                employee.debugOutput();
            }

            Console.WriteLine("=======================================================");

            Console.WriteLine("Projects: ");
            foreach (Project project in projects)
            {
                project.debugOutput();
            }
        }
    }
}
