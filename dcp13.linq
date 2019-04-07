<Query Kind="Program" />

/*

Given an integer k and a string s, find the length of the longest substring that contains at most k distinct characters.

For example, given s = "abcba" and k = 2, the longest substring with k distinct characters is "bcb".

*/

void Main()
{
	Solve("abcba", 2).Dump("3");
	Solve("abcba", 4).Dump("5");
	Solve("aaaaabbaaaa", 1).Dump("5");
	Solve("aaaabbbbbb", 2).Dump("10");
	Solve("c", 2).Dump("1");
	Solve("cccccc", 2).Dump("6");
	Solve("abcdeff", 2).Dump("3");
}

int Solve(string s, int k)
{
	var wind = new SubstringWindow(s, k);
	
	while (!wind.EndOfString)
	{
		while (!wind.TryExtendRight())
		{
			wind.TrimLeft();
		}
	}
	
	return wind.MaxLengthEverSeen;
}

class SubstringWindow
{
	private readonly Dictionary<char, int> _charCounts = new Dictionary<char, int>();
	private readonly string _entireString;
	private readonly int _maxDistinct;
	private int _left = 0;
	private int _right = 0;

	public int MaxLengthEverSeen { get; private set; }
	
	private int CurrentLength => _right - _left + 1;
	
	public SubstringWindow(string str, int maxDistinct)
	{
		_entireString = str;
		_maxDistinct = maxDistinct;
		MaxLengthEverSeen = CurrentLength;
		_charCounts.Add(str[_left], 1);
	}
	
	public bool EndOfString => _right == _entireString.Length - 1;
	
	public bool TryExtendRight()
	{
		var nextChar = _entireString[_right + 1];
		
		if (!_charCounts.ContainsKey(nextChar) && _charCounts.Count == _maxDistinct)
		{
			return false;
		}
		
		++_right;
		if (_charCounts.ContainsKey(nextChar))
		{
			_charCounts[nextChar]++;
		}
		else
		{
			_charCounts.Add(nextChar, 1);
		}
		
		if (CurrentLength > MaxLengthEverSeen)
			MaxLengthEverSeen = CurrentLength;
		
		return true;
	}
	
	public void TrimLeft()
	{
		var leftChar = _entireString[_left];
		
		var leftCharCount = _charCounts[leftChar];
		if (leftCharCount > 1)
		{
			_charCounts[leftChar]--;
		}
		else
		{
			_charCounts.Remove(leftChar);
		}
		
		++_left;
	}
}

// Define other methods and classes here