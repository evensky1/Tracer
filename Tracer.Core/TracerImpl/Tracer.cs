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
        var threadId = Thread.CurrentThread.ManagedThreadId;
        _tracers.GetOrAdd(threadId, _ =>  new ThreadTracer(threadId)).StartTrace();
    }

    public void StopTrace()
    {
        var temp = new ThreadTracer(Thread.CurrentThread.ManagedThreadId);
        _tracers.GetOrAdd(Thread.CurrentThread.ManagedThreadId, temp).StopTrace();
    }

    public TraceResult GetTraceResult()
    {
        var completeTracers = new List<ThreadResult>();
        
        foreach (var tv in _tracers.Values)
        {
            completeTracers.Add(tv.GetTraceResult());
        }

        return new TraceResult(completeTracers);
    }
}