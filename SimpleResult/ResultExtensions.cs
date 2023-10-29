namespace SimpleResult;
public static class ResultExtensions
{
    public static void ThrowOnFailure(this IResult result)
    {
        var exception = result.ExceptionOrNull();
        if (exception is not null) throw exception;
    }
    public static T GetOrThrow<T>(this IResult<T> result)
    {
        result.ThrowOnFailure();
        return result.GetOrDefault();
    }
    public static T GetOrDefault<T>(this IResult<T> result, T defaultValue) => result.IsFailure ? defaultValue : result.GetOrDefault();
    public static IResult OnSuccess(this IResult result, Action action)
    {
        if (result.IsSuccess) action();
        return result;
    }
    public static IResult<T> OnSuccess<T>(this IResult<T> result, Action<T> action)
    {
        if (result.IsSuccess) action(result.GetOrDefault());
        return result;
    }

    [Obsolete("OnFailure is deprecated due to improved error handling. Please use the new version with additional error parameter.")]
    public static IResult OnFailure(this IResult result, Action<Exception> action)
    {

        var exception = result.ExceptionOrNull();
        if (exception is not null) action(exception);
        return result;

    }
    public static IResult OnFailure(this IResult result, Action<Exception?, Errors> action)
    {

        if (result.IsFailure) action(result.ExceptionOrNull(), result.Errors);
        return result;

    }
    public static IResult OnFailure<TError>(this IResult result, Action<Exception?, IReadOnlyList<TError>> action) where TError : IError
    {
        if (!result.IsFailure) return result;
        if (result.HasError<TError>())
        {
            action(result.ExceptionOrNull(), result.Errors.OfType<TError>().ToList());
        }
        return result;

    }
    public static IResult OnErrors<TError>(this IResult result, Action<IReadOnlyList<TError>> action) where TError : IError
    {
        if (!result.IsFailure) return result;
        if (result.HasError<TError>())
        {
            action(result.Errors.OfType<TError>().ToList());
        }
        return result;

    }
    public static IResult OnError<TError>(this IResult result, Action<TError> action) where TError : IError
    {
        if (!result.IsFailure) return result;
        var error = result.Errors.OfType<TError>().FirstOrDefault();
        if (error !=null)
        {
            action(error);
        }
        return result;

    }
    public static IResult OnException(this IResult result, Action<Exception, Errors> action)
    {
        var exception = result.ExceptionOrNull();
        if (exception is not null) action(exception, result.Errors);
        return result;
    }
    public static IResult OnException<T>(this IResult result, Action<T, Errors> action) where T : Exception
    {
        var exception = result.ExceptionOrNull();
        if (exception is T ex) action(ex, result.Errors);
        return result;
    }
    public static IResult OnInnerException<T>(this IResult result, Action<T, Errors> action) where T : Exception
    {
        var exception = result.ExceptionOrNull();
        if (exception?.InnerException is T ex) action(ex, result.Errors);
        return result;
    }


    [Obsolete("Switch is deprecated due to improved error handling. Please use the version accepting an Errors parameter instead.")]
    public static void Switch<TResult>(this TResult result, Action onSuccess, Action<Exception> onFailure) where TResult : IResult
    {
        try
        {
            result.ThrowOnFailure();
            onSuccess();
        }
        catch (Exception e)
        {
            onFailure(e);
        }
    }
    public static void Switch<TResult>(this TResult result, Action onSuccess, Action<Exception?,Errors> onFailure) where TResult : IResult
    {
        if (result.IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFailure(result.ExceptionOrNull(),result.Errors);
        }
    }

    [Obsolete("Switch is deprecated due to improved error handling. Please use the version accepting an Errors parameter instead.")]
    public static void Switch<T>(this IResult<T> result, Action<T> onSuccess, Action<Exception> onFailure)
    {
        try
        {
            var resultValue = result.GetOrThrow();
            onSuccess(resultValue);
        }
        catch (Exception e)
        {
            onFailure(e);
        }
    }
    public static void Switch<T>(this IResult<T> result, Action<T> onSuccess, Action<Exception?,Errors> onFailure)
    {
        if (result.IsSuccess)
        {
            onSuccess(result.GetOrDefault());
        }
        else
        {
            onFailure(result.ExceptionOrNull(),result.Errors);
        }
    }

    [Obsolete("Switch is deprecated due to improved error handling. Please use the version accepting an Errors parameter instead.")]
    public static TReturn Fold<TReturn, TResult>(this TResult result, Func<TReturn> onSuccess, Func<Exception, TReturn> onFailure) where TResult : IResult
    {
        return result.ExceptionOrNull() switch
        {
            null => onSuccess(),
            var exception => onFailure(exception)

        };
    }
    public static TReturn Fold<TReturn, TResult>(this TResult result, Func<TReturn> onSuccess, Func<Exception?,Errors, TReturn> onFailure) where TResult : IResult
    {
        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.ExceptionOrNull(), result.Errors);
    }

    [Obsolete("Fold is deprecated due to improved error handling. Please use the version accepting an Errors parameter instead.")]
    public static TReturn Fold<TReturn, T>(this IResult<T> result, Func<T, TReturn> onSuccess, Func<Exception, TReturn> onFailure)
    {
        return result.ExceptionOrNull() switch
        {
            null => onSuccess(result.GetOrDefault()),
            var exception => onFailure(exception)

        };
    }
    public static TReturn Fold<TReturn, T>(this IResult<T> result, Func<T, TReturn> onSuccess, Func<Exception?,Errors, TReturn> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.GetOrDefault())
            : onFailure(result.ExceptionOrNull(), result.Errors);
    }
    public static Result<TR> Map<TL, TR>(this Result<TL> result, Func<TL, TR> transform)
    {
        return result.IsSuccess
            ? Result<TR>.Success(transform(result.GetOrDefault()))
            : Result<TR>.Fail(result.ExceptionOrNull(), result.Errors);
    }
}