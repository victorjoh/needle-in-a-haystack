using NUnit.Framework;
using FluentAssertions;

namespace NeedleInAHaystack.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        "dwad".Should().Be("dwadwa");
    }
}