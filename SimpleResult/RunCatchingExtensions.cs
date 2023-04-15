namespace SimpleResult;
public static class RunCatchingExtensions
{
    public static IResult RunCatching(this Action block)
    {
        try
        {
            block();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    public static IResult<T> RunCatching<T>(this Func<T> block)
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

    public static async Task<IResult<T>> RunCatching<T>(this Func<Task<T>> block)
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
}