﻿using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class ThreadTracer
{
    private readonly Stack<MethodTracer> _processingMethods;
    public List<MethodResult> ProcessedMethods { get; }
    public int Id { get; }

    private long _time;
    
    public ThreadTracer(int id)
    {
        _processingMethods = new();
        ProcessedMethods = new();
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
        try
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
                _time += current.Timer.ElapsedMilliseconds;
                ProcessedMethods.Add(current.GetTraceResult());
            }
        }
        catch (InvalidOperationException e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }
    public ThreadResult GetTraceResult()
    {
        return new ThreadResult(Id, $"{_time}ms", ProcessedMethods);
    }
}