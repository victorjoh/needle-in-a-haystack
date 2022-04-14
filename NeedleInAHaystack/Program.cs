using System;
using System.IO;
using System.Text;

namespace NeedleInAHaystack;

public class Program
{

    public static void Main(string[] args)
    {
        string baseFilename = Path.GetFileNameWithoutExtension(args[0]);
        int nbrOfMatches = 0;
        try {
            foreach (string line in File.ReadLines(args[0]))
                if (line.Contains(baseFilename))
                    nbrOfMatches++;
            Console.WriteLine("found " + nbrOfMatches);
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
}