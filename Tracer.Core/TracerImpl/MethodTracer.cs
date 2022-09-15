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

    public MethodTracer stopTrace() 
    {
        Timer.Stop();
        return this;
    }

    public void addChild(MethodTracer methodTracer)
    {
        InnerMethods.Add(methodTracer);
        InnerMethods.Append(methodTracer);
    }

    public string getChildrens()
    {
        var result = new StringBuilder("\n");
        InnerMethods.ForEach(m => result.Append($"{m.Name}\n"));
        return result.ToString();
    }

    public MethodResult getTraceResult()
    {
        var mappedMethods = new List<MethodResult>();
        InnerMethods.ForEach(m => mappedMethods.Add(m.getTraceResult()));
        return new MethodResult(Name, ClassName, Timer.Elapsed, mappedMethods);
    }
}