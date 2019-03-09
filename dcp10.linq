<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/*
Implement a job scheduler which takes in a function f and an integer n, and calls f after n milliseconds.
*/

void Main()
{
	var s = new TaskScheduler();
	
	s.Schedule(() => Console.WriteLine("Hi"), 3000);
	s.Schedule(() => Console.WriteLine("Hi2"), 4000);
	s.Schedule(() => Console.WriteLine("HiFirst"), 1000);
	
	Console.ReadLine();
}

class TaskScheduler
{
	public void Schedule(Action a, int millisecondsDelay)
	{
		Task.Delay(millisecondsDelay).ContinueWith(_ => a());
	}
}