<Query Kind="Program" />

/*
This problem was asked by Microsoft.

Compute the running median of a sequence of numbers. That is, given a stream of numbers, print out the median of the list so far on each new element.

Recall that the median of an even-numbered list is the average of the two middle numbers.
*/

void Main()
{
	var source = new[] { 2, 1, 5, 7, 2, 0, 5 };
	Solve(source).ToList().Dump();
}

IEnumerable<double> Solve(IEnumerable<int> stream)
{
	var en = stream.GetEnumerator();
	if (!en.MoveNext())
	{
		yield break;
	}

	var buffer = new LinkedList<int>();
	var median = buffer.AddFirst(en.Current);
	
	yield return median.Value;

	while (en.MoveNext())
	{
		var val = en.Current;
		
		var insLeft = (val < median.Value || (val == median.Value && buffer.Count % 2 == 0));
		if (insLeft)
		{
			SortedInsert(buffer, null, val);
			if (buffer.Count % 2 == 0)
			{
				median = median.Previous;
			}
		}
		else
		{
			SortedInsert(buffer, median, val);
			if (buffer.Count % 2 != 0)
			{
				median = median.Next;
			}
		}

		if (buffer.Count % 2 == 0)
		{
			// the median of an even-numbered list is the average of the two middle numbers
			yield return (median.Value + median.Next.Value) / 2.0;
		}
		else
		{
			yield return median.Value;
		}
	}
}

void SortedInsert(LinkedList<int> buffer, LinkedListNode<int> after, int newValue)
{
	var cur = after?.Next ?? buffer.First;
	while (cur != null)
	{
		if (newValue <= cur.Value)
		{
			buffer.AddBefore(cur, newValue);
			return;
		}
		cur = cur.Next;
	}
	buffer.AddLast(newValue);
}