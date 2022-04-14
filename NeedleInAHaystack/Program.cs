using System;
using System.IO;
using System.Text;

namespace NeedleInAHaystack;

public class Program
{

    public static void Main(string[] args)
    {
        string filename = Path.GetFileName(args[0]);
        string baseFilename = filename.Substring(0, filename.IndexOf('.'));
        int nbrOfMatches = 0;
        foreach (string line in System.IO.File.ReadLines(args[0]))
            if (line.Contains(baseFilename))
                nbrOfMatches++;
        Console.WriteLine("found " + nbrOfMatches);
    }
}