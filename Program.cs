using System;
using System.Globalization;
using System.IO;
using static System.Formats.Asn1.AsnWriter;
namespace program;
enum ReportType{ Collect, Analyze, Recon, Intel }
enum ReportStatus{ Rejected, Approved, Pending }
class MyProgram
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
        if (myLine[0] == "")
        {
            Console.WriteLine("reason:The unit must not be empty.");
            return false;
        }
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
    static int ProcessReports(List<string> my_lines, string[] Units , ReportType[] Types, int[] Prioritys, double[] Scores, ReportStatus[] Statuss ,ref int NumberValidLines, ref int NumberInvalidLines)
    {
        foreach (string line in my_lines)
        {
            string[] my_line = SplitLine(line);
            if(CountLine(my_line) && Verification(my_line))
            {
                Units[NumberValidLines] = my_line[0];
                ReportType.TryParse(my_line[1], true, out ReportType repotype);
                Types[NumberValidLines] = repotype;
                int.TryParse(my_line[2], out int pra);
                Prioritys[NumberValidLines] = pra;
                double.TryParse(my_line[3], out double sco);
                Scores[NumberValidLines] = sco;
                ReportType.TryParse(my_line[4], true, out ReportStatus repostatus);
                Statuss[NumberValidLines] = repostatus;
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
        return (sum / numberOfValid);
    }
    static double FindMaxScore(double[] scores, int numberOfValid)
    {
        double myMax = scores[0];
        for(int i = 0; i < numberOfValid;i++)
        {
            if (scores[i] > myMax)
            {
                myMax = scores[i];
            }
        }
        return myMax;
    }
    static double FindMinScore(double[] scores, int numberOfValid)
    {
        double myMin = scores[0];
        for (int i = 0; i < numberOfValid; i++)
        {
            if (scores[i] < myMin)
            {
                myMin = scores[i];
            }
        }
        return myMin;
    }
    static int CountByStatus(ReportStatus[] my_status, string status, int Valid)
    {
        int statusCount = 0;
        for (int i = 0; i < Valid; i++)
        {
            if (my_status[i].ToString().ToLower() == status.ToLower())
            {
                statusCount++;
            }
        }
        return statusCount;
    }
    static int CountByType(ReportType[] my_type, string type, int Valid)
    {
        int typeCount = 0;
        for (int i = 0; i < Valid; i++)
        {
            if (my_type[i].ToString().ToLower() == type.ToLower())
            {
                typeCount++;
            }
        }
        return typeCount;
    }
    static void DisplayBasicStatistics(double[] scores , int Valid)
    {
        double avg = CalculateAverage(scores, Valid);
        double my_max = FindMaxScore(scores, Valid);
        double my_min = FindMinScore(scores, Valid);
        Console.WriteLine("===report satistic==");
        Console.WriteLine($"the count of reports is {Valid}");
        Console.WriteLine($"the score average is {avg.ToString("F2")}");
        Console.WriteLine($"the max score is {my_max}");
        Console.WriteLine($"the min score is {my_min}\n");
    }
    static void DisplayStatusCounts(ReportStatus[] status , int Valid)
    {
        Console.WriteLine("=== Reports by Status ===");
        int myRejected = CountByStatus(status, "Rejected", Valid);
        Console.WriteLine($"the count of status Rejected is {myRejected}");
        int myApproved = CountByStatus(status, "Approved", Valid);
        Console.WriteLine($"the count of status Approved is {myApproved}");
        int myPending = CountByStatus(status, "Pending", Valid);
        Console.WriteLine($"the count of status Pending is {myPending}\n");
    }
    static void DisplayTypeCounts(ReportType[] Types, int Valid)
    {
        Console.WriteLine("=== Reports by Type ===");
        int myCollect = CountByType(Types, "Collect", Valid);
        Console.WriteLine($"the count of type Collect is {myCollect}");
        int myAnalyze = CountByType(Types, "Analyze", Valid);
        Console.WriteLine($"the count of type Analyze is {myAnalyze}");
        int myRecon = CountByType(Types, "Recon", Valid);
        Console.WriteLine($"the count of type Recon is {myRecon}");
        int myIntel = CountByType(Types, "Intel", Valid);
        Console.WriteLine($"the count of type Intel is {myIntel}\n");
    }
    static void DisplayHighestPriorityApproved(string[] Units, ReportType[] Types, int[] Prioritys, double[] Scores, ReportStatus[] Statuss, int Valid)
    {
        int maxi = Prioritys[0];
        int index = -1;
        for (int i = 0; i < Valid; i++)
        {
            if (Statuss[i] == ReportStatus.Approved && Prioritys[i] > maxi)
            {
                maxi = Prioritys[i];
                index = i;
            }
        }
        if(index == -1)
        {
            Console.WriteLine("No approved reports found");
            return;
        }
        Console.WriteLine("=== Highest Priority Approved Report ===");
        Console.WriteLine($"Unit: {Units[index]}");
        Console.WriteLine($"Type: {Types[index]}");
        Console.WriteLine($"Priority: {Prioritys[index]}");
        Console.WriteLine($"Score: {Scores[index]}\n");
    }
    static void DisplayAverageByPriority(int[] Prioritys, double[] Scores, int Valid)
    {
        int[] countP = new int[6];
        double[] sumP = new double[6];
        for (int i = 0; i < Valid; i++)
        {
            int p = Prioritys[i];
            countP[p] += 1;
            sumP[p] += Scores[i];
        }
        Console.WriteLine("=== Average Score by Priority ===");
        for (int i = 1; i < 6; i++)
        {
            Console.WriteLine($"Priority {i} : {(countP[i] != 0 ? (sumP[i] / countP[i]).ToString("F2") : "No reports")}");
        }
        Console.WriteLine("\n");
    }
    static void Main()
    {
        string path = @"..\..\..\reports.txt";
        List<string> myRepors = LoadFile(path);
        int count_line = myRepors.Count;
        Console.WriteLine($"File loaded: {count_line} lines found\n");
        if(count_line > 0)
        {
            int NumberValidLines = 0;
            int NumberInvalidLines = 0;
            string[] Units = new string[100];
            ReportType[] Types = new ReportType[100];
            int[] Prioritys = new int[100];
            double[] Scores = new double[100];
            ReportStatus[] Statuss = new ReportStatus[100];
            NumberValidLines = ProcessReports(myRepors, Units, Types, Prioritys, Scores, Statuss, ref NumberValidLines, ref NumberInvalidLines);
            Console.WriteLine($"Stored {NumberValidLines} valid records for analysis\n");
            DisplayBasicStatistics(Scores, NumberValidLines);
            DisplayStatusCounts(Statuss, NumberValidLines);
            DisplayTypeCounts(Types, NumberValidLines);
            DisplayHighestPriorityApproved(Units, Types, Prioritys, Scores, Statuss, NumberValidLines);
            DisplayAverageByPriority(Prioritys, Scores, NumberValidLines);

            Console.WriteLine("Processing complete");
            Console.WriteLine($"Valid records: {NumberValidLines}");
            Console.WriteLine($"Invalid records: {NumberInvalidLines}");
        }
    }
}
