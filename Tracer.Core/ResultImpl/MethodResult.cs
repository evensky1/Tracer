namespace Tracer.Core;

public class MethodResult
{
    public string Name { get; }
    public string ClassName { get; }
    public string Time { get; }
    public List<MethodResult> InnerMethods { get; }

    public MethodResult() {}
    public MethodResult(string name, string className, string time, List<MethodResult> innerMethods)
    {
        Name = name;
        ClassName = className;
        Time = time;
        InnerMethods = innerMethods;
    }
}