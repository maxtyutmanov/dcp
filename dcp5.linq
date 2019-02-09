<Query Kind="Program" />

/*

cons(a, b) constructs a pair, and car(pair) and cdr(pair) returns the first and last element of that pair. For example, car(cons(3, 4)) returns 3, and cdr(cons(3, 4)) returns 4.

Given this implementation of cons:

def cons(a, b):
    def pair(f):
        return f(a, b)
    return pair
Implement car and cdr.

*/

void Main()
{
	Car(Cons(3, 4)).Dump();
	Cdr(Cons(3, 4)).Dump();
}

delegate T Pair<T>(Func<T, T, T> f);

Pair<T> Cons<T>(T a, T b)
{
	return (Func<T, T, T> f) => f(a, b);
}

T Car<T>(Pair<T> pair)
{
	Func<T, T, T> extractor = (T a, T b) => a;
	return pair(extractor);
}

T Cdr<T>(Pair<T> pair)
{
	Func<T, T, T> extractor = (T a, T b) => b;
	return pair(extractor);
}

// Define other methods and classes here
