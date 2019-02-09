<Query Kind="Program" />

void Main(string[] args)
{
	var input = new[] { 5, 8, 5, 3, 10 };
	var output = Solve(input);

	Console.WriteLine(input.Aggregate(1, (prod, x) => prod * x));
	Console.WriteLine(string.Join(", ", output));
}

int[] Solve(int[] input)
{
	var n = input.Length;
	var output = new int[n];

	// precalculate right products
	output[n - 1] = input[n - 1];
	for (var i = n - 2; i >= 1; i--)
	{
		output[i] = output[i + 1] * input[i];
	}

	// go from left to right calculating left product and multiplying it by rgit staight product
	var leftProduct = 1;
	for (int i = 0; i < n - 1; i++)
	{
		var rightProduct = output[i + 1];
		// (to left of i) * (to right of i) = everything except i-th item
		output[i] = leftProduct * rightProduct;
		leftProduct *= input[i];
	}

	output[n - 1] = leftProduct;
	return output;
}