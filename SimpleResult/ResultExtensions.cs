namespace SimpleResult;

public static class ResultExtensions
{

    public static Result<T> RunCatching<T>(Func<T> block)
    {
        try
        {
            return Result<T>.Success(block());
        }
        catch (Exception? ex)
        {
            return Result<T>.Fail(ex);
        }
    }

    public static void ThrowOnFailure<T>(this Result<T> result)
    {
        var exception = result.ExceptionOrNull();
        if (exception is not null) throw exception;
    }

    public static T? GetOrThrow<T>(this Result<T?> result)
    {
        result.ThrowOnFailure();
       return result.GetOrDefault();
    }
    
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {

        if (result.IsSuccess) action(result.GetOrDefault());
        return result;
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<Exception> action)
    {

        var exception = result.ExceptionOrNull();
        if (exception is not null)  action(exception);
        return result;

    }

    public static TR Fold<TR, T>(this Result<T> result, Func<T, TR> onSuccess, Func<Exception, TR> onFailure)
    {
        return result.ExceptionOrNull() switch
        {
            null => onSuccess(result.GetOrDefault()),
            var exception => onFailure(exception)
            
        };
    }

    public static T GetOrDefault<T>(this Result<T> result,T defaultValue) => result.IsFailure ? defaultValue : result.GetOrDefault();

    public static Result<R> Map<T,R>(this Result<T> result,Func<T,R> transform)
    {

        return result.ExceptionOrNull() switch
        {
            null => Result<R>.Success(transform(result.GetOrDefault())),
            var exception => Result<R>.Fail(result.ExceptionOrNull()!)
            
        };

      
    }
}