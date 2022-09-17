using System.Reflection;
using System.Reflection.Emit;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;

public class Serializator
{
    private const string PATH =
        "C:\\Users\\fromt\\RiderProjects\\Tracer.Serialization\\plugins";
    
    public Serializator()
    {
    }

    private ITraceResultSerializer MyLoader(string path)
    {
        var assembly = Assembly.LoadFrom(path);
        var type = assembly.GetExportedTypes()[0]; //should find ITraceResultSerializer realization
        return (ITraceResultSerializer)Activator.CreateInstance(type);
    }

    public void SerializeAll(TraceResult traceResult)
    {
        var files = Directory.GetFiles(PATH);

        foreach (var serializer in files)
        {
            var serializator = MyLoader(serializer);
            MySerializer(traceResult, serializator);
        }
    }

    private void MySerializer(TraceResult traceResult, ITraceResultSerializer serializer)
    {
        try
        {
            using var fs = new FileStream($"result.{serializer.Format}", FileMode.Create);
            serializer.Serialize(traceResult, fs);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }
}