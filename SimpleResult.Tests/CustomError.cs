namespace SimpleResult.Tests;

public record CustomError(string Message) : IError;