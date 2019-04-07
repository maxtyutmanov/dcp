<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/*
The area of a circle is defined as πr^2. Estimate π to 3 decimal places using a Monte Carlo method.

Hint: The basic equation of a circle is x2 + y2 = r2.
*/

void Main()
{
	var r = 0.5;
	var n = GetNumOfSamples(14);	// 14 bits ~ 5 decimal digits
	var sum = 0L;
	
	// running monte carlo tests in parallel, each thread will have its own random number generator
	Parallel.For<(long subtotal, Random rand)>(0, n, () => (0, new Random()), (i, loop, state) =>
	{
		var (x, y) = GenerateRandomPoint(state.rand);
		state.subtotal += PutRandomPointAndGetValue(x, y, r);
		return state;
	},
	state =>
	{
		Interlocked.Add(ref sum, state.subtotal);
	});
	
	var ksi = sum / (double)n;
	
	var pi = 4 * ksi;
	
	pi.Dump();
}

(double x, double y) GenerateRandomPoint(Random rand)
{
	return (rand.NextDouble() - 0.5, rand.NextDouble() - 0.5);
}

int PutRandomPointAndGetValue(double x, double y, double r)
{
	return IsInCircle(x, y, r) ? 1 : 0;
}

bool IsInCircle(double x, double y, double r)
{
	return x*x + y*y <= r*r;
}

long GetNumOfSamples(int precisionInBits)
{
	// multiplying num of samples by 4 should double the precision, i.e. give one more correct bit
	
	return (long)Math.Pow(4, (double)precisionInBits);
}

// Define other methods and classes here
