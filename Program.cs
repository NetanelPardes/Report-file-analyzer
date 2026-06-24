using System;
using System.Globalization;
using System.IO;

namespace program;

enum ReportType{ Collect, Analyze, Recon, Intel }
enum ReportStatus{ Rejected, Approved, Pending }
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
        if(!int.TryParse(myLine[2],out priority) )
        {
            Console.WriteLine("reason:Invalid record");
            return false;
        }
        if(priority < 1 || priority > 5)
        {
            Console.WriteLine("reason: priority out of range");
            return false;
        }
        double score;
        if(!double.TryParse(myLine[3], out score) )
        {
            Console.WriteLine("reason:Invalid record");
            return false;
        }
        if (score < 0.0 || score > 100.0)
        {
            Console.WriteLine("reason: score out of range");
            return false;
        }
        ReportType repotype;
        if (!ReportType.TryParse(myLine[1],true, out repotype))
        {
            Console.WriteLine("reason: its Invalid type");
            return false;
        }
        ReportStatus repostatus;
        if (!ReportType.TryParse(myLine[4],true, out repostatus))
        {
            Console.WriteLine("reason: its Invalid status");
            return false;
        }
        Console.WriteLine("Valid record processed");
        return true;
        
    }
    static int ProcessReports(List<string> my_lines, string[] Units , string[] Types, int[] Prioritys, double[] Scores, string[] Statuss ,ref int NumberValidLines, ref int NumberInvalidLines)
    {
        foreach (string line in my_lines)
        {
            string[] my_line = SplitLine(line);
            if(CountLine(my_line) && Verification(my_line))
            {
                Units[NumberValidLines] = my_line[0];
                Types[NumberValidLines] = my_line[1];
                int.TryParse(my_line[2], out int pra);
                Prioritys[NumberValidLines] = pra;
                double.TryParse(my_line[3], out double sco);
                Scores[NumberValidLines] = sco;
                Statuss[NumberValidLines] = my_line[4];
                NumberValidLines++;
            }
            else
            {
                NumberInvalidLines++;
            }
            
        }
        return NumberValidLines;
    }
    static double CalculateAverage(double[] scores , int numberOfValid)
    {
        double sum = 0;
        for(int i = 0; i < numberOfValid; i++)
        {
            sum += scores[i];
        }
        return sum / numberOfValid;
    }
    static void Main()
    {
        string path = @"..\..\..\reports.txt";
        List<string> myRepors = LoadFile(path);
        int count_line = myRepors.Count;
        if(count_line > 0)
        {
            int NumberValidLines = 0;
            int NumberInvalidLines = 0;

            string[] Units = new string[100];
            string[] Types = new string[100];
            int[] Prioritys = new int[100];
            double[] Scores = new double[100];
            string[] Statuss = new string[100];

            NumberValidLines = ProcessReports(myRepors, Units, Types, Prioritys, Scores, Statuss, ref NumberValidLines, ref NumberInvalidLines);
            Console.WriteLine($"Stored {NumberValidLines} valid records for analysis");
            double avg = CalculateAverage(Scores, NumberValidLines);
            Console.WriteLine(avg);
            
        }
        


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
