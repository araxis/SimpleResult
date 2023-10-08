using System;

namespace SimpleResult;

public static class ResultAsyncExtensions
{
    public static async Task<IResult> OnSuccess(this IResult result, Func<Task> action)
    {
        if (result.IsSuccess) await action();
        return result;
    }
    public static async Task<IResult<T>> OnSuccess<T>(this IResult<T> result, Func<T, Task> action)
    {
        if (result.IsSuccess) await action(result.GetOrDefault());
        return result;
    }
    public static async Task<IResult> OnFailure(this IResult result, Func<Exception?, Errors, Task> action)
    {

        if (result.IsFailure) await action(result.ExceptionOrNull(), result.Errors);
        return result;
    }
    public static async Task<IResult> OnFailure<TError>(this IResult result, Func<Exception?, IReadOnlyCollection<TError>, Task> action) where TError : IError
    {
        if (!result.IsFailure) return result;
        if (result.Errors.OfType<TError>().Any())
        {
            await action(result.ExceptionOrNull(), result.Errors.OfType<TError>().ToList());
        }
        return result;
    }
    public static async Task<IResult> OnErrors<TError>(this IResult result, Func<IReadOnlyList<TError>, Task> action) where TError : IError
    {
        if (result.Errors.OfType<TError>().Any())
        {
            await action(result.Errors.OfType<TError>().ToList());
        }
        return result;

    }
    public static async Task<IResult> OnError<TError>(this IResult result, Func<TError, Task> action) where TError : IError
    {
        var error = result.Errors.OfType<TError>().FirstOrDefault();
        if (error != null)
        {
           await action(error);
        }
        return result;

    }
    public static async Task<IResult> OnException(this IResult result, Func<Exception, Errors, Task> action)
    {
        var exception = result.ExceptionOrNull();
        if (exception is not null) await action(exception, result.Errors);
        return result;
    }
    public static async Task<IResult> OnException<T>(this IResult result, Func<T, Errors, Task> action) where T : Exception
    {
        var exception = result.ExceptionOrNull();
        if (exception is T ex) await action(ex, result.Errors);
        return result;
    }
    public static async Task<IResult> OnInnerException<T>(this IResult result, Func<T, Errors, Task> action) where T : Exception
    {
        var exception = result.ExceptionOrNull();
        if (exception?.InnerException is T ex) await action(ex, result.Errors);
        return result;
    }
    public static async Task Switch<TResult>(this TResult result, Func<Task> onSuccess, Func<Exception?, Errors, Task> onFailure) where TResult : IResult
    {
        if (result.IsSuccess)
        {
            await onSuccess();
        }
        else
        {
            await onFailure(result.ExceptionOrNull(), result.Errors);
        }
    }
    public static async Task Switch<T>(this IResult<T> result, Func<T, Task> onSuccess, Func<Exception?, Errors, Task> onFailure)
    {
        if (result.IsSuccess)
        {
            await onSuccess(result.GetOrDefault());
        }
        else
        {
            await onFailure(result.ExceptionOrNull(), result.Errors);
        }
    }
    public static async Task<TReturn> Fold<TReturn, TResult>(this TResult result, Func<Task<TReturn>> onSuccess, Func<Exception?, Errors, Task<TReturn>> onFailure) where TResult : IResult
    {
        return result.IsSuccess
            ? await onSuccess()
            : await onFailure(result.ExceptionOrNull(), result.Errors);
    }
    public static async Task<TReturn> Fold<TReturn, T>(this IResult<T> result, Func<T, Task<TReturn>> onSuccess, Func<Exception?, Errors, Task<TReturn>> onFailure)
    {
        return result.IsSuccess
            ? await onSuccess(result.GetOrDefault())
            : await onFailure(result.ExceptionOrNull(), result.Errors);
    }
    public static async Task<Result<TR>> Map<TL, TR>(this Result<TL> result, Func<TL, Task<TR>> transform)
    {
        return result.IsSuccess
            ? Result<TR>.Success(await transform(result.GetOrDefault()))
            : Result<TR>.Fail(result.ExceptionOrNull(), result.Errors);
    }
}