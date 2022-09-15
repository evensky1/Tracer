namespace Tracer.Core;

public class TraceResult
{
    public IReadOnlyList<ThreadResult> Threads { get; }

    public TraceResult() {}
    
    public TraceResult(IReadOnlyList<ThreadResult> threads)
    {
        Threads = threads;
    }
}