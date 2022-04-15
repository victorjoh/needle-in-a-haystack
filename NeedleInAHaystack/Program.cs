using System.Diagnostics.Contracts;

namespace NeedleInAHaystack;

public static class Program
{

    public static void Main(string[] args)
    {
        string output = GetSinglePath(args)
                .FlatMap(ReadFile)
                .FlatMap(GetNonEmptyBaseFilename)
                .Map(textFile => textFile.Contents.CountOccurencesOf(textFile.BaseFilename))
                .Map(nbrOfOccurences => $"found {nbrOfOccurences}")
                .OrElse(error => error);
        Console.WriteLine(output);
    }

    private static Result<string> GetSinglePath(string[] args)
    {
        return args.Length == 1 ?
                Result<string>.Ok(args[0]) :
                Result<string>.Fail("Expecting a single path to a file as input argument.");
    }

    private static Result<TextFile> ReadFile(string path)
    {
        try
        {
            return Result<TextFile>.Ok(new TextFile(path, File.ReadAllText(path)));
        }
        catch (Exception e)
        {
            return Result<TextFile>.Fail(e.Message);
        }
    }

    private static Result<BaseTextFile> GetNonEmptyBaseFilename(TextFile textFile)
    {
        string baseFilename = Path.GetFileNameWithoutExtension(textFile.Path);
        return baseFilename != "" ?
                Result<BaseTextFile>.Ok(new BaseTextFile(baseFilename, textFile.Contents)) :
                Result<BaseTextFile>.Fail(
                        $"The file '{Path.GetFullPath(textFile.Path)}' does not have a base filename. Aborting.");
    }

    public static int CountOccurencesOf(this string haystack, string needle)
    {
        Contract.Requires(needle != "");
        int nbrOfOccurences = 0;
        int nextOccurenceStart = haystack.IndexOf(needle, 0);
        while (nextOccurenceStart != -1)
        {
            nbrOfOccurences++;
            nextOccurenceStart = haystack.IndexOf(needle, nextOccurenceStart + needle.Length);
        }
        return nbrOfOccurences;
    }
}

readonly record struct TextFile(string Path, string Contents);
readonly record struct BaseTextFile(string BaseFilename, string Contents);

sealed class Result<T>
{
    private T value;
    private bool success;
    private string error;

    private Result(T value, bool success, string error)
    {
        Contract.Requires(value != null || !success);
        this.value = value;
        this.success = success;
        this.error = error;
    }

    public static Result<T> Fail(string error)
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
                mapper.Invoke(value) :
                new Result<U>(default(U)!, false, error);
    }

    public Result<U> Map<U>(Func<T, U> mapper)
    {
        return success ?
                new Result<U>(mapper.Invoke(value), true, "") :
                new Result<U>(default(U)!, false, error);
    }

    public T OrElse(Func<string, T> other)
    {
        return success ? value : other.Invoke(error);
    }
}
