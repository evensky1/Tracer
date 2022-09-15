using System.Diagnostics;
using System.Text;

namespace Tracer.Core;

public class MethodTracer
{
    public string Name { get; }
    public string ClassName { get; }
    public Stopwatch Timer { get; }

    public List<MethodTracer> InnerMethods { get; }
    public MethodTracer(string name, string className, Stopwatch timer)
    {
        Name = name;
        ClassName = className;
        Timer = timer;
        InnerMethods = new ();
        Timer.Start();
    }

    public MethodTracer StopTrace() 
    {
        Timer.Stop();
        return this;
    }

    public void AddChild(MethodTracer methodTracer)
    {
        InnerMethods.Add(methodTracer);
        InnerMethods.Append(methodTracer);
    }

    public string GetChildrens()
    {
        var result = new StringBuilder("\n");
        InnerMethods.ForEach(m => result.Append($"{m.Name}\n"));
        return result.ToString();
    }

    public MethodResult GetTraceResult()
    {
        var mappedMethods = new List<MethodResult>();
        InnerMethods.ForEach(m => mappedMethods.Add(m.GetTraceResult()));
        return new MethodResult(Name, ClassName, $"{Timer.Elapsed.Milliseconds}ms", mappedMethods);
    }
}