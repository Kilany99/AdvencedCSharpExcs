using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    internal class LambdaExpressions
    {
    }

    //    Requirements:
    //- Create a StudentManager class that manages a list of students
    //- Implement custom extension methods using lambda expressions for:
    //  * Finding students by grade range
    //  * Grouping students by subject performance
    //  * Calculating weighted averages
    //- Use both expression and statement lambdas

    public static class StudentExtensions
    {

        public static IEnumerable<Student> WithinGradeRange(this IEnumerable<Student> students,
            string subject,double lowerRange, double upperRange) 
        {
            return students.Where(s =>
            s.SubjectGrades.ContainsKey(subject) &&
            s.SubjectGrades[subject] >= lowerRange &&
            s.SubjectGrades[subject] <= upperRange);

        }

        public static IEnumerable<IGrouping<string,Student>> GroupByPerformance(
            this IEnumerable<Student> students,string subject)
        {
            return students.Where(s =>
            s.SubjectGrades.ContainsKey(subject))
                .GroupBy(s =>
                {
                var grade = s.SubjectGrades[subject];
                    return grade switch
                    {
                        >= 90 => "Excellent",
                        >= 80 => "VeryGood",
                        >= 70 => "Good",
                        >= 60 => "Pass",
                        _ => "Not Pass"
                    };
                });
        }

        public static double CalculateWeightedAverage(this Student student,Dictionary<string,double> weights)
        {
            return student.SubjectGrades.
            Where(grads => weights.ContainsKey(grads.Key))
            .Select(grads => grads.Value * weights[grads.Key])
            .Sum();
        }

    }

    public class Student
    {
        public string Name { get; private set; }
        public Dictionary<string,double> SubjectGrades { get; set; }
        public int Year { get; private set; }

        public Student(string name, int year)
        {
            Name = name;
            Year = year;
            SubjectGrades = [];
        }
    }


    public class MainClass1
    {
        public void main()
        {

            var students = new List<Student>
        {
            new Student("John", 1)
            {
                SubjectGrades = new Dictionary<string, double>
                {
                    { "Math", 95 },
                    { "Physics", 88 },
                    { "Chemistry", 75 }
                }
            },
            // Add more students...
        };

            // Find students with math grades between 80 and 100
            var highMathScorers = students.WithinGradeRange("Math", 80, 100);
            // Group students by physics performance
            var physicsGroups = students.GroupByPerformance("Physics");

            // Calculate weighted average for a student
            var weights = new Dictionary<string, double>
        {
            { "Math", 0.4 },
            { "Physics", 0.4 },
            { "Chemistry", 0.2 }
        };

            foreach (var student in students)
            {
                var weightedAvg = student.CalculateWeightedAverage(weights);
                Console.WriteLine($"{student.Name}'s weighted average: {weightedAvg:F2}");
            }
        }
    }

}
