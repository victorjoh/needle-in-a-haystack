using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NeedleInAHaystack;

public class Program
{

    public static void Main(string[] args)
    {
        string baseFilename = Path.GetFileNameWithoutExtension(args[0]);
        string fileContents;
        try {
            fileContents = File.ReadAllText(args[0]);
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return;
        }

        Console.WriteLine($"found {FindOccurences.of(baseFilename).within(fileContents)}");
    }
}

public class FindOccurences
{
    string needle;

    private FindOccurences(String needle) {
        this.needle = needle;
    }

    public static FindOccurences of(string needle) {
        return new FindOccurences(needle);
    }

    public int within(string haystack) {
        int nbrOfOccurences = 0;
        foreach (string line in Regex.Split(haystack, "\r\n|\r|\n"))
            if (line.Contains(needle))
                nbrOfOccurences++;
        return nbrOfOccurences;
    }
}