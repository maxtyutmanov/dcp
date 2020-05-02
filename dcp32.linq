<Query Kind="Program" />

/*
This problem was asked by Jane Street.

Suppose you are given a table of currency exchange rates, represented as a 2D array. Determine whether there is a possible arbitrage: 
that is, whether there is some sequence of trades you can make, starting with some amount A of any currency, so that you can end up with some amount greater than A of that currency.

There are no transaction costs and you can trade fractional quantities.
*/

void Main()
{
	var er1 = new double[4, 4]
	{
		{ 1, 1.0/60, 1.0/75, 1.0/40 },
		{ 60, 1, 2, 2 },
		{ 75, 1.0/2, 1, 2 },
		{ 40, 1.0/2, 1.0/2, 1 }
	};
	
	Solve(er1).Dump("Should be possible for ix 0");

	var er2 = new double[4, 4]
	{
		{ 1, 1.0/60, 1.0/75, 1.0/40 },
		{ 60, 1, 0.75, 2 },
		{ 75, 4.0/3, 1, 2 },
		{ 40, 1.0/2, 1.0/2, 1 }
	};

	Solve(er2).Dump("Should not be possible");

	var er3 = new double[4, 4]
	{
		{ 1, 1.0/60, 1.0/75, 1.0/40 },
		{ 60, 1, 1, 2 },
		{ 75, 1, 1, 2 },
		{ 40, 1.0/2, 1.0/2, 1 }
	};
	
	Solve(er3).Dump("Should be possible");

	var er4 = new double[2, 2]
	{
		{ 1, 1.0/60 },
		{ 61, 1 }
	};

	Solve(er4).Dump("Should be possible");
}

bool Solve(double[,] er)
{
	var n = er.GetLength(0);
	var chain = Enumerable.Range(0, n).Select(i => TryCurrency(er, i)).FirstOrDefault();
	if (chain != null)
	{
		Console.WriteLine("Arbitrage is possible, trade chain is {0}; final amount is {1}", string.Join(", ", chain.GetListOfCurrencyIds()), chain.Amount);
		return true;
	}
	return false;
}

TradeChain TryCurrency(double[,] er, int targetCurIx)
{
	// slightly modified version of Dijkstra algorithm
	
	var n = er.GetLength(0);
	var chains = new TradeChain[n];
	for (int i = 0; i < n; i++)
	{
		chains[i] = new TradeChain(i);
	}
	
	chains[targetCurIx].Amount = 1;
	
	while (true)
	{
		var notProcessed = chains.Where(l => !l.IsProcessed);
		if (!notProcessed.Any())
			return null; // every trade chain is processed
			
		// visit the trade chain with the most promising exchange rate
		var curChain = MaxBy(notProcessed, l => l.Amount);
		curChain.MarkAsProcessed();
		
		for (int i = 0; i < n; i++)
		{
			if (chains[i].IsProcessed) continue;

			// try to include this currency to our chain of trades
			var newAmount = curChain.Amount * er[curChain.CurrencyIx, i];
			if (newAmount > chains[i].Amount)
			{
				chains[i].Amount = newAmount;
				chains[i].Prev = curChain;
			}
				
			if (newAmount * er[i, targetCurIx] > 1)	// arbitrage is possible
			{
				chains[i].Amount = newAmount * er[i, targetCurIx];
				return chains[i];
			}
				
		}
	}
}

class TradeChain
{
	public int CurrencyIx { get; }
	public TradeChain Prev { get; set; }
	public double Amount { get; set; }
	public bool IsProcessed { get; private set; }
	
	public TradeChain(int ix)
	{
		CurrencyIx = ix;
	}
	
	public void MarkAsProcessed()
	{
		IsProcessed = true;
	}
	
	public List<int> GetListOfCurrencyIds()
	{
		var l = new List<int>();
		var cur = this;
		
		while (cur != null)
		{
			l.Add(cur.CurrencyIx);
			cur = cur.Prev;
		}
		
		l.Reverse();
		return l;
	}
}

T MaxBy<T,V>(IEnumerable<T> source, Func<T, V> sel) where V: IComparable<V>
{
	var en = source.GetEnumerator();
	if (en.MoveNext())
	{
		T curMaxI = en.Current;
		V curMaxV = sel(curMaxI);
		
		while (en.MoveNext())
		{
			var curV = sel(en.Current);
			if (curV.CompareTo(curMaxV) > 0)
			{
				curMaxV = curV;
				curMaxI = en.Current;
			}
		}
		
		return curMaxI;
	}
	else throw new InvalidOperationException("source is empty");
}