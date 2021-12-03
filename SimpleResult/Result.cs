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

        public T GetOrDefault() => IsFailure ? default : _value;

        public Exception? ExceptionOrNull() => _failure?.Exception;


        public static Result<T> Success(T value) => new(value,null);

        public static Result<T> Fail(Exception? exception) => new(default,new Failure(exception));

        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(Exception? exception) => Fail(exception);

    
    }
}