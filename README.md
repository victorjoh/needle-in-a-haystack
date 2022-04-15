# Needle in a haystack
This program counts occurrences of the base filename in the contents of a given
file. For example, given the file `abc.txt`:
```
Hello, abc!
Now I know my abc.
Good bye!
```
then the output will be `found 2`.

To run the program, first install `dotnet`. This is how I did it on Fedora:
```
sudo dnf copr enable @dotnet-sig/dotnet
sudo dnf install dotnet
```

On Windows, [download .NET SDK x64](https://dotnet.microsoft.com/en-us/download)
and install it.

Then you can run the tests with:
```
dotnet test
```

You can run the program with your own input file, like we do below with the
`two-matches.xml` test file:
```
dotnet run --project NeedleInAHaystack NeedleInAHaystack.Tests/resources/two-matches.xml
```

## Assumptions
* We don't count overlapping matches, for example a file `aaa.txt` with contents
  `aaaaaa` counts as 2 matches, not 4.
* We only support the following character encodings: uft8, utf16be and uft16le.
* We have no performance constraints.