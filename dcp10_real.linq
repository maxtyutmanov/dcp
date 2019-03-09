<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.RegularExpressions.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Design.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.ApplicationServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.Protocols.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Utilities.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Caching.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Tasks.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web.Util</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

/*
Implement a job scheduler which takes in a function f and an integer n, and calls f after n milliseconds.
*/

void Main()
{
	var s = new TaskScheduler();
	
	var sw = Stopwatch.StartNew();
	s.FireAndForget(() => Console.WriteLine("Hi1.5"), 3500);
	s.FireAndForget(() => Console.WriteLine("Hi1"), 3498);
	s.FireAndForget(() => Console.WriteLine("Hi2"), 3502);
	s.FireAndForget(() => Console.WriteLine("HiLate"), 5000);
	s.FireAndForget(() => Console.WriteLine("HiFast"), 1);
	Console.WriteLine("Finished queueing tasks in {0}", sw.Elapsed);
	
	Console.ReadLine();
	s.Dispose();
}

class TaskScheduler : IDisposable
{
	private readonly BlockingCollection<WorkItem> _workQueue = new BlockingCollection<UserQuery.WorkItem>(1024);
	private readonly CancellationTokenSource _cts = new CancellationTokenSource();
	private readonly Task _mainProcTask;
	
	public TaskScheduler()
	{
		_mainProcTask = Task.Run(() => MainProc(_cts.Token));
	}

	public void Dispose()
	{
		while (_workQueue.TryTake(out var _));
		_cts.Cancel();
		_mainProcTask.Wait();
	}

	public void FireAndForget(Action a, int millisecondsDelay)
	{
		_workQueue.Add(new WorkItem() { Action = a, RunAtUtc = DateTime.UtcNow.AddMilliseconds(millisecondsDelay) });
	}
	
	private void MainProc(CancellationToken ct)
	{
		try
		{	        
			MainProcInternal(ct);
		}
		catch (OperationCanceledException)
		{
		}
	}
	
	private void MainProcInternal(CancellationToken ct)
	{
		var items = new List<WorkItem>();

		while (!_workQueue.IsCompleted)
		{
			ct.ThrowIfCancellationRequested();

			var closestItem = items.FirstOrDefault();
			// -1 passed to TryTake will make it wait indefinitely
			var delay = -1;

			if (closestItem != null)
			{
				delay = (int)closestItem.RunAtUtc.Subtract(DateTime.UtcNow).TotalMilliseconds;
				if (delay < 0)
				{
					// we're somehow late, run the closest task NOW
					RunClosest(items);
					continue;
				}
			}

			if (_workQueue.TryTake(out var newItem, delay, ct))
			{
				// restructure the items collection
				var i = 0;
				while (i < items.Count && newItem.RunAtUtc > items[i].RunAtUtc)
				{
					++i;
				}
				items.Insert(i, newItem);
			}
			else
			{
				// timeout (delay) expired before the new items were added to the work queue, 
				// it means it's time to run the first task in the items collection
				RunClosest(items);
			}
		}
	}

	private void RunClosest(List<WorkItem> items)
	{
		var closest = items[0];
		
		Task.Run(() =>
		{
			closest.Action();
			var expected = closest.RunAtUtc;
			var actual = DateTime.UtcNow;
			var expectedStr = expected.ToString("yyyy-MM-dd HH:mm:ss.fff");
			var actualStr = actual.ToString("yyyy-MM-dd HH:mm:ss.fff");
			Console.WriteLine("Item was scheduled to run at {0} and was actually run at {1}. Diff in MS: {2}", expectedStr, actualStr, actual.Subtract(expected).TotalMilliseconds);
		});
		items.RemoveAt(0);
	}
}

class WorkItem
{
	public DateTime RunAtUtc { get; set; }
	public Action Action { get; set; }
}