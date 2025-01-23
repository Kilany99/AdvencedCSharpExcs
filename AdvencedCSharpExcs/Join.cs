using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvencedCSharpExcs
{
//    Requirements:
//- Create related data sets: Departments and Employees
//- Implement:
//  * Inner Join
//  * Group Join
//  * Left Outer Join
//  * Cross Join
//- Handle multiple join conditions
//- Create a hierarchical data structure from flat data
    internal class Join
    {
        private readonly List<Department> _departments;
        private readonly List<Employee> _employees;
        public Join()
        {
            _departments = InitializeDept();
            _employees = InitializeEmp();
        }

        public void DemonstrateJoins()
        {
            var innerJoinResult =
                from emp in _employees
                join dept in _departments
                on emp.DepartmentId equals dept.Id
                select new
                {
                    EmployeeName = emp.Name,
                    DepartmentName = dept.Name,
                    Location = dept.Location
                };

            // Left Outer Join
            var leftJoinResult =
                from emp in _employees
                join dept in _departments
                on emp.DepartmentId equals dept.Id into deptGroup
                from dept in deptGroup.DefaultIfEmpty()
                select new
                {
                    EmployeeName = emp.Name,
                    DepartmentName = dept?.Name ?? "No Department",
                    Location = dept?.Location ?? "Unknown"

                };
            // Group Join
            var groupJoinResult =
                from dept in _departments
                join emp in _employees
                on dept.Id equals emp.DepartmentId into empGroup
                select new
                {
                    Department = dept.Name,
                    EmployeeCount = empGroup.Count(),
                    TotalSalary = empGroup.Sum(e => e.Salary),
                    Employees = empGroup.Select(e => e.Name)
                };

            // Cross Join
            var crossJoinResult =
                from dept in _departments
                from emp in _employees
                select new
                {
                    Department = dept.Name,
                    Employee = emp.Name
                };
            PrintResults(innerJoinResult, leftJoinResult, groupJoinResult, crossJoinResult);

        }

        private void PrintResults(dynamic innerJoin, dynamic leftJoin,
        dynamic groupJoin, dynamic crossJoin)
        {
            if (innerJoin != null)
            {
                foreach (var res in innerJoin)
                {
                    Console.WriteLine($"Result from inner join {res}");
                    Console.WriteLine();
                }
            }
            if (leftJoin != null)
            {
                foreach (var res in leftJoin)
                {
                    Console.WriteLine($"Result from left join {res}");
                    Console.WriteLine();
                }
            }

            if (groupJoin != null)
            {
                foreach (var res in groupJoin)
                {
                    Console.WriteLine($"Result from group join {res}");
                    Console.WriteLine();
                }
            }
            if (crossJoin != null)
            {
                foreach (var res in crossJoin)
                {
                    Console.WriteLine($"Result from cross join {res}");
                    Console.WriteLine();
                }
            }



        }

        private List<Department> InitializeDept()
        {
            return new List<Department>
            {
                new() { Id = 1, Name = "IT", Location = "New York" },
                new() { Id = 2, Name = "HR", Location = "London" },
                new() { Id = 3, Name = "Finance", Location = "Tokyo" }
            };
        }

        private List<Employee> InitializeEmp()
        {
            return new List<Employee>
        {
            new() { Id = 1, Name = "John", DepartmentId = 1, Salary = 50000 },
            new() { Id = 2, Name = "Jane", DepartmentId = 1, Salary = 60000 },
            new() { Id = 3, Name = "Bob", DepartmentId = 2, Salary = 45000 },
            new() { Id = 4, Name = "Alice", DepartmentId = 4, Salary = 70000 }
        };

        }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }

}
