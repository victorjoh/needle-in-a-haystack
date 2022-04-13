using NUnit.Framework;
using FluentAssertions;
using ConsoleApp1;
using System.IO;
using System;

namespace NeedleInAHaystack.Tests;

public class Tests
{

    [Test]
    public void Test1()
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            Program.Main(new string[] { "resources/empty.txt" });
            sw.ToString().Should().Be("found 0\n");
        }

    }
}