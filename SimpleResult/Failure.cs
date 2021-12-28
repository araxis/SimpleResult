namespace SimpleResult;

internal class Failure
{

    public Exception? Exception { get; }
    public IEnumerable<ErrorInfo> ErrorInfos { get; } 
    public Failure(IEnumerable<ErrorInfo> errorInfos)
    {
        ErrorInfos = errorInfos ?? new List<ErrorInfo>();
        Exception = null;
    }
    public Failure(Exception exception)
    {
        Exception = exception;
        ErrorInfos = new List<ErrorInfo>();
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

   // public override string ToString() => $"Failure({Exception})";
}