using System;
using System.Globalization;
using System.IO;

namespace program;

class program
{
    static List<string> LoadFile(string myPath)
    {
        if (!File.Exists(myPath))
        {
            string[] my_file = myPath.Split('\\');
            Console.WriteLine($"Error: File {my_file[my_file.Length-1]} not found");
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
    static string[] SplitLine(string line)
    {
        string[] myLine = line.Split(',', StringSplitOptions.TrimEntries);

        return myLine;
    }
    static bool CountLine(String[] myLine)
    {
        if(myLine.Length == 5)
        {
            return true;
        }
        return false;
    }
    static bool Verification(String[] myLine)
    {
        int priority;
        if(!int.TryParse(myLine[2],out priority) || (priority < 1 && priority > 5))
        {
            //need to fix
            Console.WriteLine("reason:Invalid record");
            return false;
        }
        double score;
        if(!double.TryParse(myLine[3], out score) || (score < 0.0 && score > 100.0))
        {
            //need to fix
            Console.WriteLine("reason:Invalid record");
            return false;
        }
        Console.WriteLine("Valid record processed");
        return true;
    }
    //static int ProcessReports(List<string> myRepo)
    //{

    //}
    static void Main()
    {
        string path = @"..\..\..\reports.txt";
        List<string> myRepors = LoadFile(path);
        //foreach(string t in myRepors)
        //{
        //    Console.WriteLine(t);
        //}
        if (myRepors.Count > 0)
        {
            Console.WriteLine($"File loaded: {myRepors.Count} lines found");
        }
        //ProcessLine("Alpha, Collect,3.87,5,Approved");
    }
}
