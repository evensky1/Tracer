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
        ThreadTimer = new Stopwatch();
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
        MethodTracer current = _processingMethods.Pop();
        // while (!_processingMethods.TryPop(out current)) { } //here lies eternal cycle when StopTrace works on MyMethod
        current.stopTrace();

        MethodTracer parent;
        if (_processingMethods.TryPeek(out parent)) 
        {
            parent.addChild(current);
        }

        if (_processingMethods.Count == 0)
        {
            ProcessedMethods.Add(current.getTraceResult());
        }

        Console.WriteLine($"Ms: {current.Timer.Elapsed.Milliseconds:0000} \n" +
                          $"Name: {current.Name} \n" +
                          $"Class name: {current.ClassName} \n" +
                          $"Childs: {current.getChildrens()}");
    }

    public void stopTimer()
    {
        ThreadTimer.Stop();
    }

    public ThreadResult GetTraceResult()
    {
        return new ThreadResult(Id, ThreadTimer.Elapsed, ProcessedMethods);
    }
}