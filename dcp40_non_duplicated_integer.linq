<Query Kind="Program" />

/*
This problem was asked by Google.

Given an array of integers where every integer occurs three times except for one integer, which only occurs once, find and return the non-duplicated integer.

For example, given [6, 1, 3, 3, 3, 6, 6], return 1. Given [13, 19, 13, 13], return 19.

Do this in O(N) time and O(1) space.
*/

void Main()
{
	var n = 10000;
	
	for (int i = 0; i < 100; i++)
	{
		var (testA, expected) = GenerateTestCase(n);
		var actual = Solve(testA);
		
		if (actual == expected)
		{
			Console.WriteLine("OK!");
		}
		else
		{
			throw new Exception("WRONG");
		}
	}
}

(int[] a, int result) GenerateTestCase(int n)
{
	var a = new int[n * 3 + 1];
	var rand = new Random();
	
	for (int i = 0; i < n; i++)
	{
		var r = i;
		if (rand.Next(2) == 0)
		{
			// occasionally flip the sign
			r = -i;
		}
		
		a[i] = r;
		a[i + n] = r;
		a[i + n * 2] = r;
	}
	
	var result = rand.Next(-100000, 100000);
	a[n * 3] = result;
	
	// shuffle
	
	for (int i = 0; i < a.Length; i++)
	{
		var j = rand.Next(a.Length);
		var tmp = a[i];
		a[i] = a[j];
		a[j] = tmp;
	}
	
	return (a, result);
}

int Solve(int[] a)
{
	var bm = new BitwiseModulo3(a[0]);
	for (int i = 1; i < a.Length; i++)
	{
		bm = bm.Add(a[i]);
	}
	return bm.ToInt();
}

struct BitwiseModulo3
{
	private byte[] _tBits;
	
	public BitwiseModulo3(int number)
	{
		_tBits = new byte[32];
		
		for (int i = 0; i < 32; i++)
		{
			if ((number & (1 << i)) != 0)
			{
				_tBits[i] = 1;
			}
		}
	}
	
	public int ToInt()
	{
		var res = 0;
		for (int i = 0; i < 32; i++)
		{
			if (_tBits[i] == 1)
			{
				res |= (1 << i);
			}
		}
		return res;
	}
	
	private BitwiseModulo3(byte[] bits)
	{
		_tBits = bits;
	}
	
	public BitwiseModulo3 Add(int x)
	{
		return Add(new BitwiseModulo3(x));
	}
	
	public BitwiseModulo3 Add(BitwiseModulo3 w2)
	{
		var w1 = this;
		var resBits = new byte[32];
		for (int i = 0; i < 32; i++)
		{
			resBits[i] = (byte)((w1._tBits[i] + w2._tBits[i]) % 3);
		}
		return new BitwiseModulo3(resBits);
	}
}