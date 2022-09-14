namespace Tracer.Core;

public class ThreadResult
{
    public int Id { get; }
    public TimeSpan Time { get; }
    public IReadOnlyList<MethodResult> methods { get; }

    public ThreadResult(int id, TimeSpan time, IReadOnlyList<MethodResult> methods)
    {
        Id = id;
        Time = time;
        this.methods = methods;
    }
}