<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Numerics.dll</Reference>
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	var p = new Fraction(1, 3);
	var n = 7;
	
	var sum = new Fraction(0, 1);
	var dataPoints = new List<DataPoint>();
	for (var k = 3; k <= n; ++k)
	{
		var ak = AkProb(p, n, k);
		sum += ak;
		dataPoints.Add(new DataPoint()
		{
			K = k,
			Cnk = C(n, k),
			PAk = ak
		});
	}
	
	dataPoints.Dump(true);
	sum.Dump();
}

class DataPoint
{
	public int K;
	public Fraction PAk;
	public long Cnk;
}

Fraction AkProb(Fraction p, int n, int k)
{
	var c = C(n, k);
	var cFrac = new Fraction(c, 1);
	
	return cFrac * GetPqProduct(p, n, k);
}

Fraction GetPqProduct(Fraction p, int n, int k)
{
	var res = new Fraction(1, 1);
	var one = new Fraction(1, 1);
	
	for (int i = 0; i < k; i++)
	{
		res *= p;
	}
	
	for (int i = 0; i < n - k; i++)
	{
		res *= (one - p);
	}
	
	return res;
}

long C(int n, int k)
{
	long numerator = 1;
	
	for (int i = n; i > k; --i)
	{
		numerator *= i;
	}
	
	long denominator = Fact(n - k);
	
	return numerator / denominator;
}

long Fact(int n)
{
	long res = 1;
	
	for (int i = 1; i <= n; i++)
	{
		res *= i;
	}
	
	return res;
}

class Fraction
{
	public long Numerator, Denominator;

	// Initialize the fraction from a string A/B.
	public Fraction(string txt)
	{
		string[] pieces = txt.Split('/');
		Numerator = long.Parse(pieces[0]);
		Denominator = long.Parse(pieces[1]);
		Simplify();
	}

	// Initialize the fraction from numerator and denominator.
	public Fraction(long numer, long denom)
	{
		Numerator = numer;
		Denominator = denom;
		Simplify();
	}

	// Return a * b.
	public static Fraction operator *(Fraction a, Fraction b)
	{
		// Swap numerators and denominators to simplify.
		Fraction result1 = new Fraction(a.Numerator, b.Denominator);
		Fraction result2 = new Fraction(b.Numerator, a.Denominator);

		return new Fraction(
			result1.Numerator * result2.Numerator,
			result1.Denominator * result2.Denominator);
	}

	// Return -a.
	public static Fraction operator -(Fraction a)
	{
		return new Fraction(-a.Numerator, a.Denominator);
	}

	// Return a + b.
	public static Fraction operator +(Fraction a, Fraction b)
	{
		// Get the denominators' greatest common divisor.
		long gcd_ab = GCD(a.Denominator, b.Denominator);

		long numer =
			a.Numerator * (b.Denominator / gcd_ab) +
			b.Numerator * (a.Denominator / gcd_ab);
		long denom =
			a.Denominator * (b.Denominator / gcd_ab);
		return new Fraction(numer, denom);
	}

	// Return a - b.
	public static Fraction operator -(Fraction a, Fraction b)
	{
		return a + -b;
	}

	// Return a / b.
	public static Fraction operator /(Fraction a, Fraction b)
	{
		return a * new Fraction(b.Denominator, b.Numerator);
	}

	// Simplify the fraction.
	private void Simplify()
	{
		// Simplify the sign.
		if (Denominator < 0)
		{
			Numerator = -Numerator;
			Denominator = -Denominator;
		}

		// Factor out the greatest common divisor of the
		// numerator and the denominator.
		long gcd_ab = GCD(Numerator, Denominator);
		Numerator = Numerator / gcd_ab;
		Denominator = Denominator / gcd_ab;
	}

	// Use Euclid's algorithm to calculate the
	// greatest common divisor (GCD) of two numbers.
	private static long GCD(long a, long b)
	{
		a = Math.Abs(a);
		b = Math.Abs(b);

		// Pull out remainders.
		for (; ; )
		{
			long remainder = a % b;
			if (remainder == 0) return b;
			a = b;
			b = remainder;
		};
	}

	// Convert a to a double.
	public static implicit operator double(Fraction a)
	{
		return (double)a.Numerator / a.Denominator;
	}

	// Return the fraction's value as a string.
	public override string ToString()
	{
		return Numerator.ToString() + "/" + Denominator.ToString();
	}
}

// Define other methods and classes here
