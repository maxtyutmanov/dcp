<Query Kind="Program" />

void Main()
{
	var totalCnt = 100_000_000;
	var stats = new int[8];
	
	for (var i = 0; i < totalCnt; ++i)
	{
		stats[rand7()]++;
	}

	stats.Select((num, ix) => new { ix, num }).Where(x => x.ix != 0).Chart(x => x.ix, x => x.num).Dump();
	
	(_internalLoopCnt / (double)totalCnt).Dump("Average internal loop execs");
}

static Random _gen = new Random();
static long _internalLoopCnt = 0;

int rand5() => _gen.Next(1, 6); 

int rand7()
{
	int r1;
	int r2;
	
	do
	{
		r1 = rand5();
		r2 = rand5();
		++_internalLoopCnt;
	} while (r1 == 5 && r2 > 1);
	
	var a = (r1 - 1) * 5 + r2;
	var r7 = (a + 2) / 3;
	
	return r7;
}

// Define other methods and classes here
