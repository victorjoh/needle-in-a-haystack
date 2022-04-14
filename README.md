# needle-in-a-haystack
Finds all occurrences of the base name of a file in that file.

I did this on Fedora:
sudo dnf copr enable @dotnet-sig/dotnet
sudo dnf install dotnet

dotnet run


how to debug in vscode https://stackoverflow.com/a/70283792

dotnet run --project NeedleInAHaystack /home/victor/Code/needle-in-a-haystack/NeedleInAHaystack.Tests/empty.txt

things to test:
release file handle?
character encoding
dots in path
slashes in path
non existing file
...