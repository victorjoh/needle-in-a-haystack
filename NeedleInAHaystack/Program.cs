using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace NeedleInAHaystack;

public static class Program
{

    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Expecting a single path to a file as input argument.");
            return;
        }

        string fileContents;
        try
        {
            fileContents = File.ReadAllText(args[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        string baseFilename = Path.GetFileNameWithoutExtension(args[0]);

        Console.WriteLine($"found {fileContents.CountOccurencesOf(baseFilename)}");
    }

    public static int CountOccurencesOf(this string haystack, string needle)
    {
        int nbrOfOccurences = 0;
        int nextOccurenceStart = haystack.IndexOf(needle, 0);
        while (nextOccurenceStart != -1)
        {
            nextOccurenceStart = haystack.IndexOf(needle, nextOccurenceStart + needle.Length);
            nbrOfOccurences++;
        }
        return nbrOfOccurences;
    }
}
