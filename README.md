



## Simple Result
[![.NET](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/vpre/Arax.SimpleResult.svg)](https://www.nuget.org/packages/Arax.SimpleResult)
[![NuGet](https://img.shields.io/nuget/dt/Arax.SimpleResult.svg)](https://www.nuget.org/packages/Arax.SimpleResult) 

`SimpleResult`  is a lightweight library that provides a convenient way to handle the results of operations in your .NET applications. The library offers a straightforward approach to managing success and failure scenarios, making your code more readable, maintainable, and less prone to errors.

### Installing SimpleResult

You should install [SimpleResult with NuGet](https://www.nuget.org/packages/Arax.SimpleResult):

    Install-Package Arax.SimpleResult
    
Or via the .NET Core command line interface:

    dotnet add package Arax.SimpleResult

## Usage

The following sections demonstrate various ways to use the `SimpleResult` library for handling the results of operations.

##  Make Result 
```csharp
    var successResult = Result.Success();
    var intResult = Result<int>.Success(12);
    Result<int> intResult = 12;
    var failureResult = Result.Fail("An error occurred");
    var failureResultWithValue = Result<int>.Fail("An error occurred");
    var failureResultErors = Result.Fail("An error occurred","Another error occurred");
    var failureResultException = Result.Fail(new InvalidOperationException()); 
    var failureResultException = Result.Fail(new InvalidOperationException(),"An error occurred"); 
    Result failure = new InvalidOperationException(); 
```
    
## Basic Usage 
```csharp
public void UseResult()
{
    var result = GetPerson(1);
    var isSuccess = result.IsSuccess;
    var isFailure = result.IsFailure;    
    
    //default value is based on result type
    var person = result.GetOrDefault();
    //or can passed as a parameter
    var personWithCustomDefault = result.GetOrDefault(new Person("custom"));
    
    //if IsFailure == false => exception is null and Errors is empty
    var exception = result.ExceptionOrNull();
    var errors = result.Errors;
}
``` 
## Performing an action on success 
Imagine you have a method that returns a result,you can use the OnSuccess method to execute an action only when the result is successful.
```csharp
    public void UseResult()
    {
       var result = GetPerson(1);
       result.OnSuccess(person  => { 
         // Perform code
       });
    }
```  
## Handling failures and exceptions
If you want to handle failures, you can use the OnFailure or OnException method to perform an action when the result is unsuccessful:
```csharp

    var result = DangerousOperation();

    // Here exception is nullable
    result.OnFailure((exception, errs) => Console.WriteLine($"Exception: {exception?.Message ?? "None"}, errors: {string.Join(", ", errs)}"));

    // if result failed because of any type exception
    result.OnException((ex, errs) =>
    {
        Console.WriteLine($"Exception: {ex.Message}, errors: {string.Join(", ", errs)}"));
    }

    // if result failed because of a DivideByZeroException
    result.OnException<DivideByZeroException>((ex, errors) =>
    {
        Console.WriteLine($"Caught DivideByZeroException: {ex.Message}");
    });
      // if result failed because of a exception with InvalidOperationException inner exception
    result.OnInnerException<InvalidOperationException>((ex, errors) =>
    {
        Console.WriteLine($"Caught InvalidOperationException as InnerException: {ex.Message}");
    });

```

## Switching Between Success and Failure Actions
Use the Switch method to execute different actions based on the success or failure of the result:

```csharp
    var result = DoSomething();
    result.Switch(
        onSuccess: () => Console.WriteLine("Operation succeeded"),
        onFailure: (exception, errors) => Console.WriteLine($"Operation failed with errors: {string.Join(", ", errors)}")
    );

```
For results with values, you can access the value within the Switch method:
```csharp
    var result = DoSomethingWithValue();
    result.Switch(
        onSuccess: value => Console.WriteLine($"Operation succeeded with value: {value}"),
        onFailure: (exception, errors) => Console.WriteLine($"Operation failed with errors: {string.Join(", ", errors)}")
    );
```
## Transform the Result Based on Success or Failure 
The Fold method allows you to transform the result into another type, depending on whether the result is successful or not:
```csharp
    var result = DoSomething();
    string message = result.Fold(
        onSuccess: () => "Success",
        onFailure: (exception, errors) => $"Failure: {string.Join(", ", errors)}"
    );

    Console.WriteLine(message);
```    
For results with values, you can access the value within the Fold method:
```csharp
    var result = DoSomethingWithValue();
    string message = result.Fold(
        onSuccess: value => $"Success: {value}",
        onFailure: (exception, errors) => $"Failure: {string.Join(", ", errors)}"
    );

    Console.WriteLine(message);
```
## Mapping a Result to Another Type
The Map method allows you to transform the value of a Result<T> object into another type, while preserving the success or failure state of the original result. The method accepts a function that takes the current value of the result and returns a new value of a different type.
```csharp
    var result = Result<int>.Success(10);

    // Mapping to another type
    var newResult = result.Map(x => x.ToString());

    // Output
    // newResult.IsSuccess == true
    // newResult.GetOrDefault() == "10"

    var result = Result<int>.Fail("Something went wrong");
    var newResult = result.Map(x => x.ToString());

    // Output
    // newResult.IsFailure == true
    // newResult.Errors == { "Something went wrong" }


```

## Asynchronous Extensions
The SimpleResult library also provides asynchronous extensions, allowing you to handle success and failure scenarios in async methods. Here's a quick overview of the available async extension methods:

- OnSuccess: Perform an async action when the result is successful.
- OnFailure: Perform an async action when the result is unsuccessful.
- OnFailure<TError>: Perform an async action when the result is unsuccessful and contains a specific error type.
- OnException: Perform an async action when the result contains an exception.
- OnException<T>: Perform an async action when the result contains a specific exception type.
- OnInnerException<T>: Perform an async action when the result contains an inner exception of a specific type.
- Switch: Execute different async actions based on the success or failure of the result.
- Fold: Transform the result into another type using async actions based on the success or failure of the result.
- Map: Map a result to another type using an async transformation function.

Here's an example of using the OnSuccess async extension method:
```csharp
    public async Task UseResultAsync()
    {
        var result = await FetchPersonAsync(1);
        await result.OnSuccess(async person =>
        {
            await DoSomethingAsync(person);
        });
    }
```
And here's an example of using the OnFailure async extension method with a specific error type:
```csharp
    public async Task UseResultAsync()
    {
        var result = await DangerousOperationAsync();
        await result.OnFailure<CustomError>(async (exception, errors) =>
        {
            Console.WriteLine($"Caught CustomError: {string.Join(", ", errors)}");
            await HandleCustomErrorAsync(errors);
        });
}
```
For more detailed examples and usage information related to asynchronous operations, please refer to the sections on [Performing Actions on Success](#performing-an-action-on-success), [Handling Failures and Exceptions](#handling-failures-and-exceptions), [Switching Between Success and Failure Actions](#switching-between-success-and-failure-actions), [Transform the Result Based on Success or Failure](#transform-the-result-based-on-success-or-failure), and [Mapping a Result to Another Type](#mapping-a-result-to-another-type) in this README. The async extension methods provided by the `ResultAsyncExtensions` class work similarly to their synchronous counterparts but support async actions and transformations for seamless integration with asynchronous operations in your code.

## Try-Catch 
```csharp
    
    public void UseResult()
    {
       var result = GetPerson(1);
       try
       {
          var person = result.GetOrThrow();
          //use person
        }
        catch (Exception e)
        {
         //catch exception
        }
    }
``` 
