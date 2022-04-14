# Needle in a haystack
Counts the number of occurrences of the base filename in the contents of a given
file. For example, given the file `abc.txt`:
```
Hello, abc!
Now I know my abc.
Good bye!
```
then the output will be `found 2`.

To run the program, first install dotnet. This is how I did it on Fedora:
```
sudo dnf copr enable @dotnet-sig/dotnet
sudo dnf install dotnet
```

then you can run the tests with:
```
dotnet test
```

You can run the program with your own input file like this for example:
```
dotnet run --project NeedleInAHaystack NeedleInAHaystack.Tests/resources/two-matches.xml
```

## Assumptions
* We only support the following character encodings: uft8, utf16be and uft16le.
* We have no performance constraints.