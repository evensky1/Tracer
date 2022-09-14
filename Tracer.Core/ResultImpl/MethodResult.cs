namespace Tracer.Core;

public class MethodResult
{
    public string Name { get; }
    public string ClassName { get; }
    public TimeSpan Time { get; }
    public List<MethodTracer> InnerMethods { get; }

    public MethodResult(string name, string className, TimeSpan time, List<MethodTracer> innerMethods)
    {
        Name = name;
        ClassName = className;
        Time = time;
        InnerMethods = innerMethods;
    }
}