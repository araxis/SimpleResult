namespace SimpleResult;

internal class Failure
{
    public Exception Exception { get; }
    public Failure(Exception exception)
    {
        Exception = exception;

    }

    public override bool Equals(object? other)
    {
        if (other is null) return false;
        return other is Failure otherFailure && Exception == otherFailure.Exception;
    }



    public override int GetHashCode()
    {
        return Exception.GetHashCode();
    }

    public override string ToString() => $"Failure({Exception})";
}