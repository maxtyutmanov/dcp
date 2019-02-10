<Query Kind="Program" />

/*
Given the mapping a = 1, b = 2, ... z = 26, and an encoded message, count the number of ways it can be decoded.

For example, the message '111' would give 3, since it could be decoded as 'aaa', 'ka', and 'ak'.

You can assume that the messages are decodable. For example, '001' is not allowed.
*/

void Main()
{
	var msg = string.Join("", Enumerable.Repeat("23", 10));
	Solve(msg).Dump();
	// 24157817
}

long Solve(string encodedStr)
{
	var numBase = Convert.ToByte('1');
	var encodedBytes = encodedStr.Select(c => (byte)(Convert.ToByte(c) - numBase + 1)).ToArray();
	
	return CountIterative(encodedBytes);
}

long CountIterative(byte[] encoded)
{
	if (encoded.Length < 2)
		return 1;
	
	var prevCount = 1L;
	var curCount = 1L;
	
	for (int i = encoded.Length - 2; i >= 0; --i)
	{
		var num = (encoded[i] * 10 + encoded[i + 1]);
		long newCount;
		
		if (num <= 26)
		{
			newCount = prevCount + curCount;
		}
		else
		{
			newCount = curCount;
		}

		prevCount = curCount;
		curCount = newCount;
	}
	
	return curCount;
}