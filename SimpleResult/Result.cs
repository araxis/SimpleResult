namespace SimpleResult;

public readonly record struct Result : IResult
{
    private readonly Failure? _failure;

    // Use expression-bodied properties for IsSuccess and IsFailure
    public bool IsSuccess => _failure == null;
    public bool IsFailure => _failure != null;
    
    private Result(Failure? failure)
    {
        _failure = failure;
    }

    public Exception? ExceptionOrNull() => _failure?.Exception;
 
    public IReadOnlyCollection<IError> Errors => _failure?.ErrorInfos ?? new List<IError>();

    public static IResult Fail(Exception exception) => new Result(new Failure(exception));
    public static Result Success() => new(null);
    public static Result Fail(IEnumerable<IError> resultErrors) => new(new Failure(resultErrors.ToArray()));
    public static Result Fail(params string[] message)
    {
        var errors = message.Select(m =>(IError) new Error(m)).ToArray();
        return Fail(errors);
    }
    public static Result Fail(Exception exception,params string[] message)
    {
        var errors = message.Select(m =>(IError) new Error(m)).ToArray();
        return Fail(exception,errors);
    }

    public static Result Fail(Exception exception,params IError[] errors) => new( new Failure(exception,errors));
    public static Result Fail(Exception exception,IEnumerable<IError> errors) => Fail(exception,errors.ToArray());
    public static Result Fail(params IError[] resultErrors) => new(new Failure(resultErrors));

    public static implicit operator Result(Exception exception) => (Result)Fail(exception);
    public static implicit operator Result(IError[] errorInfo) => Fail(errorInfo);
}

public readonly struct Result<T> : IResult<T>
{
    private readonly T _value;
    private readonly Failure? _failure;

    // Use expression-bodied properties for IsSuccess and IsFailure
    public bool IsSuccess => _failure == null;
    public bool IsFailure => _failure != null;

    private Result(T value, Failure? failure)
    {
        _value = value;
        _failure = failure;
    }

    public IReadOnlyCollection<IError> Errors => _failure?.ErrorInfos ?? new List<IError>();
    public T GetOrDefault() => IsFailure ? default : _value;
    public Exception? ExceptionOrNull() => _failure?.Exception;

    public static Result<T> Success(T value) => new(value, null);
    public static Result<T> Fail(Exception exception) => new(default, new Failure(exception));
    public static Result<T> Fail(Exception? exception,params IError[] errors) => new(default, new Failure(exception,errors));
    public static Result<T> Fail(Exception? exception,IEnumerable<IError> errors) =>Fail(exception,errors.ToArray());
    public static Result<T> Fail(params string[] message)
    {
        var errors = message.Select(m =>(IError) new Error(m)).ToArray();
        return Fail(errors);
    }
    public static Result<T> Fail(Exception? exception,params string[] message)
    {
        var errors = message.Select(m =>(IError) new Error(m)).ToArray();
        return Fail(exception,errors);
    }

    public static Result<T> Fail(IEnumerable<IError> errors) => new(default, new Failure(errors.ToArray()));
    public static Result<T> Fail(params IError[] errors) => new(default, new Failure(errors));

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Exception exception) => Fail(exception);
    public static implicit operator Result<T>(IError[] errorInfos) => Fail(errorInfos);
}