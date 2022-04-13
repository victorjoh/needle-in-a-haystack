using NUnit.Framework;
using FluentAssertions;
using ConsoleApp1;
using System.IO;
using System;

namespace NeedleInAHaystack.Tests;

public class ProgramTests
{

    [Test]
    [TestCase("empty.txt", 0)]
    [TestCase("one-match.json", 1)]
    [TestCase("two-matches.xml", 2)]
    public void Should_write_how_many_times_the_file_root_name_occurs_in_the_file(
        string inputFile,
        int matches)
    {
        using (StringWriter output = new StringWriter())
        {
            Console.SetOut(output);
            Program.Main(new string[] { inputFile });
            output.ToString().Should().Be("found " + matches + Environment.NewLine);
        }
    }
}