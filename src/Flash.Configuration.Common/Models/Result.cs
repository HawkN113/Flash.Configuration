namespace Flash.Configuration.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, string.Empty);

    public static Result Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("Error message cannot be empty.");

        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(T value) : base(true, string.Empty)
    {
        Value = value;
    }

    private Result(string error) : base(false, error)
    {
        Value = default!;
    }

    public static Result<T> Success(T value) => new Result<T>(value);

    public new static Result<T> Failure(string error) => new Result<T>(error);
}