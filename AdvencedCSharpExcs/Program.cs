// See https://aka.ms/new-console-template for more information


using AdvencedCSharpExcs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


//IEnumerable<string> filtered = names.Where(n => n.Contains("a"));
//IEnumerable<string> sorted = filtered.OrderBy(n => n.Length);
//IEnumerable<string> finalQuery = sorted.Select(n => n.ToUpper());
//foreach (string name in filtered)
//    Console.Write(name + "|"); // Harry|Mary|Jay|
//Console.WriteLine();
//foreach (string name in sorted)
//    Console.Write(name + "|"); // Jay|Mary|Harry|
//Console.WriteLine();
//foreach (string name in finalQuery)
//    Console.WriteLine(name + "|"); // JAY|MARY|HARRY|

//Func<string, bool> func = n => n.Contains("o");
//foreach(string s in values)
//    Console.WriteLine(func(s));  //false false false

//IEnumerable<string> outerQuery = names
//.Where(n => n.Length == names.OrderBy(n2 => n2.Length)  
//.Select(n2 => n2.Length).First());
//foreach (string s in outerQuery)
//    Console.WriteLine(s + "|");

//IEnumerable<string> outerQuery2 =
//from n in names
//where n.Length ==
//(from n2 in names orderby n2.Length select n2.Length).First()
//select n;



