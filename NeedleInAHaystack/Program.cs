using System;
using System.IO;
using System.Text;

namespace NeedleInAHaystack;

public class Program
{

    public static void Main(string[] args)
    {
        int pos = args[0].IndexOf('.');
        string name = args[0].Substring(0, pos);
        int counter = 0;
        foreach (string line in System.IO.File.ReadLines(args[0]))
            if (line.Contains(name))
                counter++;
        Console.WriteLine("found " + counter);
    }
}