using System.Collections.Concurrent;

namespace Tracer.Core;

public class Tracer : ITracer
{
    private readonly ConcurrentDictionary<int, ThreadTracer> _tracers;
    
    public Tracer()
    {
        _tracers = new ();
    }

    public void StartTrace()
    {
        var temp = new ThreadTracer(Thread.CurrentThread.ManagedThreadId);
        _tracers.GetOrAdd(Thread.CurrentThread.ManagedThreadId, temp).StartTrace();
    }

    public void StopTrace()
    {
        var temp = new ThreadTracer(Thread.CurrentThread.ManagedThreadId);
        _tracers.GetOrAdd(Thread.CurrentThread.ManagedThreadId, temp).StopTrace();
    }

    public TraceResult GetTraceResult()
    {
        var _completeTracers = new List<ThreadResult>();
        
        foreach (var tv in _tracers.Values)
        {
            _completeTracers.Add(new ThreadResult(tv.Id, $"{tv.ThreadTimer.ElapsedMilliseconds}ms", tv.ProcessedMethods));
        }
        
        return new TraceResult(_completeTracers);
    }
}