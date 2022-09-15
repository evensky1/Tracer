using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class ThreadTracer
{
    private readonly Stack<MethodTracer> _processingMethods;
    public List<MethodResult> ProcessedMethods { get; }
    public int Id { get; }
    public Stopwatch ThreadTimer { get; }
    public ThreadTracer(int id)
    {
        _processingMethods = new();
        ProcessedMethods = new();
        ThreadTimer = new ();
        ThreadTimer.Start();
        Id = id;
    }

    public void StartTrace()
    {
        var stackTrace = new StackTrace(true);
        var method = stackTrace.GetFrame(2)?.GetMethod();
        var temp = new Stopwatch();
        _processingMethods.Push(new MethodTracer(method.Name, method.ReflectedType.Name, temp));
    }

    public void StopTrace()
    {
        var current = _processingMethods.Pop();
        current.StopTrace();

        MethodTracer parent;
        if (_processingMethods.TryPeek(out parent)) 
        {
            parent.AddChild(current);
        }

        if (_processingMethods.Count == 0)
        {
            ProcessedMethods.Add(current.GetTraceResult());
        }
        
    }

    public void StopTimer()
    {
        ThreadTimer.Stop();
    }

    public ThreadResult GetTraceResult()
    {
        return new ThreadResult(Id, $"{ThreadTimer.ElapsedMilliseconds}ms", ProcessedMethods);
    }
}