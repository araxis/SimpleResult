



## Simple Result
[![.NET](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/vpre/Arax.SimpleResult.svg)](https://www.nuget.org/packages/Arax.SimpleResult)
[![NuGet](https://img.shields.io/nuget/dt/Arax.SimpleResult.svg)](https://www.nuget.org/packages/Arax.SimpleResult) 

`SimpleResult` is a monad for modeling success (Success) or failure (Exception).

### Installing SimpleResult

You should install [SimpleResult with NuGet](https://www.nuget.org/packages/Arax.SimpleResult):

    Install-Package Arax.SimpleResult
    
Or via the .NET Core command line interface:

    dotnet add package Arax.SimpleResult

##  Make Result<T> and Return 
```csharp
    public Result<Person> GetPerson(long id)
    {
        try
        {
            
           // var person = do logic;
            return Result<Person>.Success(person);
          
        }
        catch (Exception e)
        {
            return Result<Person>.Fail(e);
           
        }
    }
```
## OR : More Simple
```csharp
public Result<Person> GetPerson(long id)
    {
        try
        {
           
           // var person = do logic;
            return person;
          
        }
        catch (Exception e)
        {
            return e;
        }
    }
```
    
## Error result without Exceptions (maybe change in future!!!)
    
in this case
* result.IsFailure is true and
* result.ExceptionOrNull() always is null.
* result.Errors return list of Errorinfo
    
```csharp
 public record Param(int Prop);
    
 public Result<bool> Method(Param param)
 {
   if (param.Prop <= 15){
                                //or list<ErrorInfo>
      return Result<bool>.Error(new ErrorInfo("error type", "identifier", "error message"));
   }
   return Result<bool>.Success(true);
 }
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
    
    //if IsFailure == false => exception is null
    var exception = result.ExceptionOrNull();
    //
    var errors = result.Errors;
}
```
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
## Callback 
```csharp
    
    public void UseResult()
    {
       var result = GetPerson(1);
       result.OnSuccess(person  => {  });
       result.OnFailure(exception  => { });
       result.Switch(onSuccess:person  => {  },onFailure:exception  => { });
    }
```  
## Map 
```csharp
    
    public void UseResult()
    {
       var result = GetPerson(1);
    
       //mapping will execute just on success path
       Result<int> mapResult = result.Map(person => person.Age);
        
       //each path has map function
       Result<int> foldResult = result.Fold(onSuccess: person => person.Age,
                                            onFailure:exception => exception.GetHashCode());
    }
```    
