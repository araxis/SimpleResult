namespace SimpleResult;

public static class ResultExtensions
{


 
    public static Result<T> RunCatching<T>(this Func<T> block)
    {
        try
        {
            return Result<T>.Success(block());
        }
        catch (Exception ex)
        {
            return Result<T>.Fail(ex);
        }
    }

    public static async Task<Result<T>> RunCatching<T>(this Func<Task<T>> block)
    {
        try
        {
            var result = await block();
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<T>.Fail(ex);
        }
    }

    private static void ThrowOnFailure<T>(this Result<T> result)
    {
        var exception = result.ExceptionOrNull();
        if (exception is not null) throw exception;
    }

    public static T GetOrThrow<T>(this Result<T> result)
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

    public static void Switch<T>(this Result<T> result, Action<T> onSuccess, Action<Exception> onFailure)
    {
        try
        {
            var resultValue= result.GetOrThrow();
            onSuccess(resultValue);
        }
        catch (Exception e)
        {
            onFailure(e);
        }
    }

    public static R Fold<R, T>(this Result<T> result, Func<T, R> onSuccess, Func<Exception, R> onFailure)
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
            var exception => Result<R>.Fail(exception)
            
        };

      
    }
    
}