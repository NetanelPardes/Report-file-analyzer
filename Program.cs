using System;
using System.IO;

namespace program;

class program
{
    static List<string> LoadFile(string myPath)
    {
        if (!File.Exists(myPath))
        {
            Console.WriteLine("Error: File 'reports.txt' not found");
            return [];
        }
        string[] myFileString = File.ReadAllLines(myPath);
        List<string> myResult = new List<string>();
        foreach(string report in myFileString)
        {
            myResult.Add(report);
        }
        if (myFileString.Length == 0)
        {
            Console.WriteLine("Error: File is empty");
        }
        return myResult;
    }
    static void Main()
    {
        string path = @"..\..\..\reports.txt";
        List<string> myRepors = LoadFile(path);
        //foreach(string t in myRepors)
        //{
        //    Console.WriteLine(t);
        //}
        

    }
}
