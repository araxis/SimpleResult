namespace SimpleResult;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    IReadOnlyCollection<IError> Errors { get; }
    Exception? ExceptionOrNull();

}

public interface IResult<out T> : IResult
{
    T GetOrDefault();
}