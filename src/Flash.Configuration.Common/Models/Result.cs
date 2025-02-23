namespace Flash.Configuration.Common.Models;

public class Result
{
    public bool IsSuccessful => Errors.Count == 0;
    public bool IsFailed => !IsSuccessful;

    public string ErrorMessages => Errors.Select(e => e.Message).Aggregate((a, b) => $"{a}, {b}");

    public List<Error> Errors { get; protected set; }

    protected Result(List<Error> errors)
    {
        Errors = [];
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result([]);
    }

    public static Result Fail(Error error)
    {
        var validErrors = new List<Error> { error };
        return new Result(validErrors);
    }
}