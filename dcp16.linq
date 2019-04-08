<Query Kind="Program" />

/*

You run an e-commerce website and want to record the last N order ids in a log. Implement a data structure to accomplish this, with the following API:

record(order_id): adds the order_id to the log
get_last(i): gets the ith last element from the log. i is guaranteed to be smaller than or equal to N.
You should be as efficient with time and space as possible.

*/

void Main()
{
	var bufSize = 100;
	
	var buf = new CyclicBuffer<int>(bufSize);
	
	// putting items from 1 to 150
	Enumerable.Range(1, 150).ToList().ForEach(i => buf.Record(i));
	
	// getting last 100 items
	for (int i = 1; i <= 100; i++)
	{
		buf.GetLast(i).Dump();
	}
}

class CyclicBuffer<T>
{
	private readonly int _capacity;
	private int _head;
	private T[] _buffer;
	
	public CyclicBuffer(int capacity)
	{
		_capacity = capacity;
		_head = 0;
		_buffer = new T[capacity];
	}
	
	public void Record(T item)
	{
		_head++;
		if (_head == _capacity)
			_head = 0;
		
		_buffer[_head] = item;
	}
	
	public T GetLast(int i)
	{
		// because our index is zero-based, but i is 1-based
		i = i - 1;
		
		var position = _head - i;
		
		if (position < 0)
			position += _capacity;
			
		return _buffer[position];
	}
}