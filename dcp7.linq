<Query Kind="Program" />

/*
Given the mapping a = 1, b = 2, ... z = 26, and an encoded message, count the number of ways it can be decoded.

For example, the message '111' would give 3, since it could be decoded as 'aaa', 'ka', and 'ak'.

You can assume that the messages are decodable. For example, '001' is not allowed.
*/

void Main()
{
	Solve("333").Dump();
}

int Solve(string encoded)
{
	var numBase = Encoding.ASCII.GetBytes(new[] { '1' })[0];
	var encodedBytes = Encoding.ASCII.GetBytes(encoded).Select(b => (byte)(b + 1 - numBase)).ToArray();
	encodedBytes.Dump();
	
	return Count(new ArraySegment<byte>(encodedBytes));
}

int Count(ArraySegment<byte> encoded)
{
	if (encoded.Count == 0 || encoded.Count == 1)
		return 1;
		
	var totalCount = Count(new ArraySegment<byte>(encoded.Array, encoded.Offset + 1, encoded.Count - 1));
		
	var twoDigitNum = (encoded.First() * 10) + encoded.Skip(1).First();
	if (twoDigitNum <= 26)
	{
		totalCount += Count(new ArraySegment<byte>(encoded.Array, encoded.Offset + 2, encoded.Count - 2));
	}
	
	return totalCount;
}