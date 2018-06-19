This library aims to bring a lot of abstractions present in the functional 
world so they play nice in C#

I have avoided the more standard naming to fit the language, this means map
and bind are Select and SelectMany. Using this style naming means you have 
LINQ query syntax at your disposal for things like monads. So your code can 
have the feel of f# computational blocks if that works for you :)

Here is adding two Maybe wrapped values, the query syntax maps to Select
and SelectMany to provide the composition in the same way as the IEnumerable
monad. LINQ is a monad engine

var result =
	from a in 5.ToSome()
    from b in 3.ToSome()
    select a + b;

