using NUnit.Framework;
using FluentAssertions;
using NeedleInAHaystack;
using System.IO;
using System;

namespace NeedleInAHaystack.Tests;

public class ProgramTests
{

    [Test]
    [TestCase("empty.txt", 0)]
    [TestCase("one-match.json", 1)]
    [TestCase("two-matches.xml", 2)]
    public void Should_write_how_many_times_the_base_filename_occurs_in_the_file(
        string inputFile,
        int expectedMatches)
    {
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource(inputFile) }))
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

    string TestResource(string relativeResourcePath)
    {
        return $"resources/{relativeResourcePath}";
    }

    string SingleLine(string contents)
    {
        return contents + Environment.NewLine;
    }

    [Test]
    public void Can_handle_file_without_extension()
    {
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource("one-match") }))
                .Should().Be(SingleLine("found 1"));
    }

    [Test]
    public void Can_handle_relative_path_dots()
    {
        ConsoleOutputOf(() => Program.Main(new string[] { $"resources/../resources/one-match.json" }))
                .Should().Be(SingleLine("found 1"));
    }

    [Test]
    public void Should_give_error_when_file_does_not_exist()
    {
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource("non-existing-file.txt") }))
                .Should().Be(SingleLine($"Could not find file '{Path.GetFullPath(TestResource("non-existing-file.txt"))}'."));
    }

    [Test]
    public void Should_give_error_when_trying_to_read_directory()
    {
        ConsoleOutputOf(() => Program.Main(new string[] { "resources" }))
                .Should().Be(SingleLine($"Access to the path '{Path.GetFullPath("resources")}' is denied."));
    }

    [Test]
    public void Should_give_error_when_no_argument_is_passed_to_the_program()
    {
        ConsoleOutputOf(() => Program.Main(new string[] { }))
                .Should().Be(SingleLine("Expecting a single path to a file as input argument."));
    }
}