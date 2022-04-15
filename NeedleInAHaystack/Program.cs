namespace NeedleInAHaystack;

using Contents = String;
using FilePath = String;
using BaseFilename = String;
using Error = String;

public static class Program
{

    public static void Main(string[] args)
    {
        GetSinglePath(args)
                .FlatMap(ReadFile)
                .FlatMap(GetNonEmptyBaseFilename)
                .Map(CountBaseFilenameOccurencesInContents)
                .Map(nbrOfOccurences => $"found {nbrOfOccurences}")
                .OrElse(error => error)
                .Consume(Console.WriteLine);
    }

    private static Result<FilePath> GetSinglePath(string[] args)
    {
        return args.Length == 1 ?
                Result<FilePath>.Ok(args[0]) :
                Result<FilePath>.Fail("Expecting a single path to a file as input argument.");
    }

    private static Result<(FilePath, Contents)> ReadFile(FilePath path)
    {
        try
        {
            return Result<(FilePath, Contents)>.Ok((path, File.ReadAllText(path)));
        }
        catch (Exception e)
        {
            return Result<(FilePath, Contents)>.Fail(e.Message);
        }
    }

    private static Result<(BaseFilename, Contents)> GetNonEmptyBaseFilename((FilePath, Contents) textFile)
    {
        var (path, contents) = textFile;
        string baseFilename = Path.GetFileNameWithoutExtension(path);
        return baseFilename != "" ?
                Result<(BaseFilename, Contents)>.Ok((baseFilename, contents)) :
                Result<(BaseFilename, Contents)>.Fail(
                        $"The file '{Path.GetFullPath(path)}' does not have a base filename. Aborting.");
    }

    private static int CountBaseFilenameOccurencesInContents((BaseFilename, Contents) textFile)
    {
        var (baseFilename, contents) = textFile;
        return contents.CountOccurencesOf(baseFilename);
    }

    public static int CountOccurencesOf(this string haystack, string needle)
    {
        if (needle == "") throw new ArgumentException("Empty string is not allowed");
        int nbrOfOccurences = 0;
        int nextOccurenceStart = haystack.IndexOf(needle, 0);
        while (nextOccurenceStart != -1)
        {
            nbrOfOccurences++;
            nextOccurenceStart = haystack.IndexOf(needle, nextOccurenceStart + needle.Length);
        }
        return nbrOfOccurences;
    }

    private static void Consume<T>(this T value, Action<T> action) {
        action(value);
    }
}

sealed class Result<T>
{
    private T value;
    private bool success;
    private Error error;

    private Result(T value, bool success, Error error)
    {
        this.value = value;
        this.success = success;
        this.error = error;
    }

    public static Result<T> Fail(Error error)
    {
        return new Result<T>(default(T)!, false, error);
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(value, true, "");
    }

    public Result<U> FlatMap<U>(Func<T, Result<U>> mapper)
    {
        return success ?
                mapper(value) :
                Result<U>.Fail(error);
    }

    public Result<U> Map<U>(Func<T, U> mapper)
    {
        return success ?
                Result<U>.Ok(mapper(value)) :
                Result<U>.Fail(error);
    }

    public T OrElse(Func<Error, T> other)
    {
        return success ? value : other(error);
    }
}
