<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

/*
This problem was asked by Dropbox.

Conway's Game of Life takes place on an infinite two-dimensional board of square cells. Each cell is either dead or alive, and at each tick, the following rules apply:

Any live cell with less than two live neighbours dies.
Any live cell with two or three live neighbours remains living.
Any live cell with more than three live neighbours dies.
Any dead cell with exactly three live neighbours becomes a live cell.
A cell neighbours another cell if it is horizontally, vertically, or diagonally adjacent.

Implement Conway's Game of Life. It should be able to be initialized with a starting list of live cell coordinates and the number of steps it should run for. 
Once initialized, it should print out the board state at each step. Since it's an infinite board, print out only the relevant coordinates, i.e. from the top-leftmost live cell to bottom-rightmost live cell.
*/

void Main()
{
	var startCells = new List<Point>();
	
	for (int x = -1; x <= 1; x++)
		for (int y = -1; y <= 1; y++)
			startCells.Add(new Point(x, y));
	
	startCells.Add(new Point(2, 0));
	
	var renderer = new PrettyRenderer();
	var b = new Board(startCells);
	
	while (true)
	{
		b.Render(renderer);
		Console.ReadLine();
		b.Tick();
	}
}

class PrettyRenderer : IBoardRenderer
{
	private readonly int _scaleFactor;
	private Bitmap _currentBmp;
	private Graphics _currentGraphics;
	private int _minX;
	private int _maxX;
	private int _minY;
	private int _maxY;
	
	public PrettyRenderer(int scaleFactor = 10)
	{
		_scaleFactor = scaleFactor;
	}
	
	public void RenderLiveCell(Point p)
	{
		var tX = (p.X - _minX) * _scaleFactor;
		var tY = (_maxY - p.Y) * _scaleFactor;
		
		_currentGraphics.FillRectangle(Brushes.Black, tX, tY, _scaleFactor, _scaleFactor);
	}

	public void Restart(int minX, int maxX, int minY, int maxY)
	{
		_minX = minX;
		_maxX = maxX;
		_minY = minY;
		_maxY = maxY;

		var width = maxX - minX + 1;
		var height = maxY - minY + 1;

		_currentBmp = new Bitmap(width * _scaleFactor, height * _scaleFactor);
		_currentGraphics = Graphics.FromImage(_currentBmp);
		_currentGraphics.FillRectangle(Brushes.White, 0, 0, width * _scaleFactor, height * _scaleFactor);
	}
	
	public void Flush()
	{
		_currentGraphics.Flush();
		
		using (var ms = new MemoryStream())
		{
			_currentBmp.Save(ms, ImageFormat.Png);
			ms.Position = 0;
			var buffer = new byte[ms.Length];
			ms.Read(buffer, 0, (int)ms.Length);
			
			Util.Image(buffer).Dump();
		}

		_currentGraphics.Dispose();
		_currentGraphics = null;
		_currentBmp.Dispose();
		_currentBmp = null;
	}
}

struct Point
{
	public int X { get; }
	public int Y { get; }
	
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
	
	public Point Add(Point p)
	{
		return new Point(X + p.X, Y + p.Y);
	}
}

interface IBoardRenderer
{
	void Restart(int minX, int maxX, int minY, int maxY);
	void RenderLiveCell(Point p);
	void Flush();
}

class Board
{
	private static readonly Point[] _neighbourVectors = new UserQuery.Point[]
	{
		new Point(1, 1), new Point(0, 1), new Point(-1, 1), 
		new Point(1, 0), new Point(-1, 0),
		new Point(1, -1), new Point(0, -1), new Point(-1, -1)
	};
	private readonly HashSet<Point> _aliveCells = new HashSet<Point>();
	
	public Board(IEnumerable<Point> aliveCells)
	{
		_aliveCells = new HashSet<UserQuery.Point>(aliveCells);
	}

	public void Render(IBoardRenderer renderer)
	{
		if (_aliveCells.Count == 0)
		{
			renderer.Restart(0, 0, 0, 0);
			return;
		}

		var frame = new { MinX = 0, MaxX = 0, MinY = 0, MaxY = 0 };
		frame = _aliveCells.Aggregate(frame, (acc, p) => 
		{
			return new 
			{
				MinX = Math.Min(p.X, acc.MinX),
				MaxX = Math.Max(p.X, acc.MaxX),
				MinY = Math.Min(p.Y, acc.MinY),
				MaxY = Math.Max(p.Y, acc.MaxY)
			};
		});
		
		renderer.Restart(frame.MinX, frame.MaxX, frame.MinY, frame.MaxY);
		
		for (int x = frame.MinX; x <= frame.MaxX; x++)
		{
			for (int y = frame.MaxY; y >= frame.MinY; y--)
			{
				var p = new Point(x, y);
				if (IsCellAlive(p))
				{
					renderer.RenderLiveCell(p);
				}
			}
		}
		
		renderer.Flush();
	}
	
	public void Tick()
	{
		var totalNeighboursCount = 8;
		
		var willDie = new List<Point>();
		var candidateCells = new List<Point>(); // may become alive

		// Any live cell with less than two live neighbours dies.
		// Any live cell with two or three live neighbours remains living.
		// Any live cell with more than three live neighbours dies.
		foreach (var liveCell in _aliveCells)
		{
			var deadNeighbours = GetNeighbours(liveCell, IsCellDead);
			candidateCells.AddRange(deadNeighbours);
			var liveNeighboursCount = totalNeighboursCount - deadNeighbours.Count;
			if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
			{
				willDie.Add(liveCell);
			}
		}
		
		var willRevive = new List<Point>();
		// Any dead cell with exactly three live neighbours becomes a live cell.
		foreach (var deadCell in candidateCells)
		{
			var deadNeighbours = GetNeighbours(deadCell, IsCellDead);
			var liveNeighboursCount = totalNeighboursCount - deadNeighbours.Count;
			
			if (liveNeighboursCount == 3)
				willRevive.Add(deadCell);
		}
		
		foreach (var cell in willDie)
		{
			_aliveCells.Remove(cell);
		}
		
		foreach (var cell in willRevive)
		{
			_aliveCells.Add(cell);
		}
	}
	
	private List<UserQuery.Point> GetNeighbours(Point p, Predicate<Point> predicate)
	{
		var output = new List<UserQuery.Point>();		
		
		foreach (var v in _neighbourVectors)
		{
			var neighbourPoint = p.Add(v);
			
			if (predicate(neighbourPoint))
				output.Add(neighbourPoint);
		}
		
		return output;
	}
	
	private bool IsCellAlive(Point p) => _aliveCells.Contains(p);
	
	private bool IsCellDead(Point p) => !IsCellAlive(p);
}