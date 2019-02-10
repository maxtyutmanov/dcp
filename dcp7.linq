<Query Kind="Program" />

/*
Given the mapping a = 1, b = 2, ... z = 26, and an encoded message, count the number of ways it can be decoded.

For example, the message '111' would give 3, since it could be decoded as 'aaa', 'ka', and 'ak'.

You can assume that the messages are decodable. For example, '001' is not allowed.
*/

void Main()
{
	var msg = string.Join("", Enumerable.Repeat("12", 50));
	Solve(msg).Dump();
	// 24157817
}

long Solve(string encodedStr)
{
	var numBase = Convert.ToByte('1');
	var encodedBytes = encodedStr.Select(c => (byte)(Convert.ToByte(c) - numBase + 1)).ToArray();
	var cache = new long[encodedBytes.Length];
	
	return Count(new ArraySegment<byte>(encodedBytes), cache);
}

long Count(ArraySegment<byte> encoded, long[] cache)
{
	if (encoded.Count == 0 || encoded.Count == 1)
		return 1;

	if (cache[encoded.Offset] != 0)
		return cache[encoded.Offset];

	long oneDigitPrefixCount = 0;
	long twoDigitPrefixCount = 0;
	
	oneDigitPrefixCount = Count(new ArraySegment<byte>(encoded.Array, encoded.Offset + 1, encoded.Count - 1), cache);
		
	var twoDigitPrefix = (encoded.First() * 10) + encoded.Skip(1).First();
	if (twoDigitPrefix <= 26)
	{
		twoDigitPrefixCount = Count(new ArraySegment<byte>(encoded.Array, encoded.Offset + 2, encoded.Count - 2), cache);
	}
	
	var totalCount = oneDigitPrefixCount + twoDigitPrefixCount;
	cache[encoded.Offset] = totalCount;
	return totalCount;
}