namespace SimpleResult
{

    public readonly struct Result<T>
    {
        private readonly T _value;
        private readonly Failure? _failure;

        private Result(T value, Failure? failure)
        {
            _value = value;
            this._failure = failure;
        }
        
        public bool IsSuccess => _failure==null;
        
        public bool IsFailure => _failure!=null;

        public IEnumerable<ErrorInfo> Errors => _failure?.ErrorInfos ?? new List<ErrorInfo>();

        public T GetOrDefault() => IsFailure ? default : _value;

        public Exception? ExceptionOrNull() => _failure?.Exception;


        public static Result<T> Success(T value) => new(value,null);

        public static Result<T> Fail(Exception exception) => new(default,new Failure(exception));

        public static Result<T> Error(IEnumerable<ErrorInfo> resultErrors) => new(default, new Failure(resultErrors));
        public static Result<T> Error(ErrorInfo errorInfo) => new(default, new Failure(new List<ErrorInfo>{errorInfo}));

        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(Exception exception) => Fail(exception);
        public static implicit operator Result<T>(ErrorInfo errorInfo) => Error(errorInfo);
        public static implicit operator Result<T>(List<ErrorInfo> errorInfos) => Error(errorInfos);

    
    }
}