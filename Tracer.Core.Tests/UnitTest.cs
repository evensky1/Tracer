namespace Tracer.Core.Tests;

public class Tests
{
    private ITracer _tracer;
    private const int WAIT_CONST = 100;

    private void FirstMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(WAIT_CONST);
        _tracer.StopTrace();
    }

    private void SecondMethod()
    {
        _tracer.StartTrace();
        FirstMethod();
        ThirdMethod();
        _tracer.StopTrace();
    }

    private void ThirdMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(WAIT_CONST);
        _tracer.StopTrace();
    }

    private long ParseMyTimeToLong(string time)
    {
        return long.Parse(time.Substring(0, time.Length - 2));
    }

    [SetUp]
    public void Setup()
    {
        _tracer = new Tracer();
    }

    [Test]
    public void Time_Format_Test()
    {
        FirstMethod();
        var time = _tracer.GetTraceResult().Threads[0].methods[0].Time;
        Assert.That("ms", Is.EqualTo(time.Substring(time.Length - 2)));
    }

    [Test]
    public void Method_Time_Count_Default_Scenario()
    {
        FirstMethod();
        var time = _tracer.GetTraceResult().Threads[0].methods[0].Time;
        Assert.True(ParseMyTimeToLong(time) >= WAIT_CONST);
    }

    [Test]
    public void Method_Time_Count_With_Child_Methods()
    {
        SecondMethod();
        var secondMethod = _tracer.GetTraceResult().Threads[0].methods[0];
        long summaryTime = 0;
        secondMethod.InnerMethods.ForEach(m => summaryTime += ParseMyTimeToLong(m.Time));
        Assert.True(ParseMyTimeToLong(secondMethod.Time) >= summaryTime);
    }

    [Test]
    public void Method_Name_Default_Scenario()
    {
        FirstMethod();
        SecondMethod();
        ThirdMethod();
        var rootMethods = _tracer.GetTraceResult().Threads[0].methods;
        Assert.That("FirstMethod", Is.EqualTo(rootMethods[0].Name));
        Assert.That("SecondMethod", Is.EqualTo(rootMethods[1].Name));
        Assert.That("ThirdMethod", Is.EqualTo(rootMethods[2].Name));
    }

    [Test]
    public void Method_Name_Child_Methods()
    {
        SecondMethod();
        var childMethods = _tracer.GetTraceResult().Threads[0].methods[0].InnerMethods;
        Assert.That("FirstMethod", Is.EqualTo(childMethods[0].Name));
        Assert.That("ThirdMethod", Is.EqualTo(childMethods[1].Name));
    }

    [Test]
    public void Thread_Id_Default_Scenario()
    {
        FirstMethod();
        var currentThread = _tracer.GetTraceResult().Threads[0];
        Assert.That(currentThread.Id, Is.EqualTo(Thread.CurrentThread.ManagedThreadId));
    }

    [Test]
    public void Several_Threads_Count()
    {
        _tracer.StartTrace();

        List<Thread> threads = new();
        for (int i = 0; i < 4; ++i)
        {
            var thread = new Thread(FirstMethod);
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads) thread.Join();
        _tracer.StopTrace();

        int actualCountOfThreads = _tracer.GetTraceResult().Threads.Count;
        Assert.That(actualCountOfThreads, Is.EqualTo(5));
    }

    [Test]
    public void Thread_Time_Count()
    {
        FirstMethod();
        SecondMethod();
        ThirdMethod();
        var currentThread = _tracer.GetTraceResult().Threads[0];
        long rootMethodsTime = 0;
        foreach (var methodResult in currentThread.methods)
        {
            rootMethodsTime += ParseMyTimeToLong(methodResult.Time);
        }

        Assert.That(ParseMyTimeToLong(currentThread.Time), Is.EqualTo(rootMethodsTime));
    }
}