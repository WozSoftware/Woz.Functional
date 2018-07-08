# Woz.Functional

This library aims to bring a lot of abstractions present in the functional 
world so they play nice in C# in a handy .NET Core 2.0 nuget package.

I have avoided the more standard naming to fit the C# language, this means 
map and bind are Select and SelectMany. Using this style naming means you have 
LINQ query syntax at your disposal while using monads. So your code can have 
the feel of f# computational blocks if that works for you :)

# Monads

The current monads are:

- Maybe = Value or None
- Result = Value or Error
- Lazy = Lazy composition
- Task = async composition
- State = Thread state

Here is an example of adding two Maybe wrapped values, the query syntax maps 
to Select and SelectMany to provide the composition in the same way as the
IEnumerable monad. LINQ is a monad engine

```
var result =
    from a in 5.ToSome()
    from b in 3.ToSome()
    select a + b;
```
Computation will stop if a or b are None

# Composition

- Function composition
- Argument reversal
- Function and Action curry
- Function and Action de-curry

