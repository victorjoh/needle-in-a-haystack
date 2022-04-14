using NUnit.Framework;
using FluentAssertions;
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
        string fullPath = Path.GetFullPath(TestResource("non-existing-file.txt"));
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource("non-existing-file.txt") }))
                .Should().Be(SingleLine($"Could not find file '{fullPath}'."));
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

    [Test]
    [TestCase("utf8")]
    [TestCase("utf16le")]
    [TestCase("utf16be")]
    public void Can_handle_character_encoding(string encoding)
    {
        // The test resource contains one match in the specified encoding.
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource($"{encoding}-世界您好.txt") }))
                .Should().Be(SingleLine("found 1"));
    }

    [Test]
    public void Can_not_handle_iso88591_character_encoding()
    {
        // The test resource contains one match in iso88591 encoding.
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource("iso88591-åäö.txt") }))
                .Should().Be(SingleLine("found 0"));
    }

    [Test]
    public void Should_give_error_when_there_is_no_base_filename()
    {
        string fullPath = Path.GetFullPath(TestResource(".txt"));
        ConsoleOutputOf(() => Program.Main(new string[] { TestResource(".txt") }))
                .Should().Be(SingleLine($"The file '{fullPath}' does not have a base filename. Aborting."));
    }

    [Test]
    public void Should_count_multiple_occurences_on_same_line()
    {
        "123abc456abc".CountOccurencesOf("abc")
                .Should().Be(2);
    }

    [Test]
    public void Should_not_count_overlapping_matches()
    {
        "aaaaaaaaa".CountOccurencesOf("aaa")
                .Should().Be(3);
    }
}