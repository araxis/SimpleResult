



## Simple Result
[![Arax.simpleResult - NuGet](https://img.shields.io/badge/nuget-Arax.SimpleResult-blue)](https://www.nuget.org/packages/Arax.SimpleResult)
[![NuGet](https://img.shields.io/nuget/dt/Arax.SimpleResult.svg)](https://www.nuget.org/packages/Arax.SimpleResult) 
[![.NET](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/araxis/SimpleResult/actions/workflows/dotnet.yml)

`SimpleResult` is a monad for modelling success (Ok) or failure (Err) operations.

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
## OR : more simle
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
       Result<int> foldResult = result.Fold(onSuccess: person => person.Age,onFailure:exception => exception.GetHashCode());
    }
```    
