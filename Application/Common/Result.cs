using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public class Result<T>
{
    public T? Value { get; init; }
    public string? Error { get; init; }

    public bool IsSuccess => Error is null;
    private Result(T? value, string? error)
    {
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new Result<T>(value, null);

    public static Result<T> Failure(string error)=> new Result<T>(default, error);
}
public class Result
{
    public string? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(string? error)
    {
        Error = error;
    }

    public static Result Success()=> new Result(null);

    public static Result Failure(string error) => new Result(error);
}