namespace SimpleResult;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    IReadOnlyList<IError> Errors { get; }
    Exception? ExceptionOrNull();
    bool HasException<T>() where T:Exception;
    bool HasInnerException<T>() where T:Exception;
    bool HasError<T>() where T:IError;
}

public interface IResult<out T> : IResult
{
    T GetOrDefault();
}