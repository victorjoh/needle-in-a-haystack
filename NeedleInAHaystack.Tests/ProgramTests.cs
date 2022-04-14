using NUnit.Framework;
using FluentAssertions;
using NeedleInAHaystack;
using System.IO;
using System;

namespace NeedleInAHaystack.Tests;

public class ProgramTests
{

    [Test]
    [TestCase("resources/empty.txt", 0)]
    [TestCase("resources/one-match.json", 1)]
    [TestCase("resources/two-matches.xml", 2)]
    public void Should_write_how_many_times_the_base_filename_occurs_in_the_file(
        string inputFile,
        int expectedMatches)
    {
        ConsoleOutputOf(() => Program.Main(new string[] { inputFile }))
                .Should().Be(SingleLine($"found {expectedMatches}"));
    }

    string ConsoleOutputOf(Action action)
    {
        using (StringWriter output = new StringWriter())
        {
            Console.SetOut(output);
            action();
            return output.ToString();
        }
    }

    string SingleLine(string contents) {
        return contents + Environment.NewLine;
    }
}