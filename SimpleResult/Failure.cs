namespace SimpleResult;

internal class Failure
{
    public Exception? Exception { get; }
    public IReadOnlyCollection<IError> ErrorInfos { get; } 
    public Failure(params IError[] errorInfos)
    {
        ErrorInfos =  new List<IError>(errorInfos);
        Exception = null;
    }
    public Failure(Exception exception,params IError[] errors)
    {
        Exception = exception;
        ErrorInfos = errors.ToList();
    }
  
 
}