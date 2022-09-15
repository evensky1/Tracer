using System.Reflection;
using System.Reflection.Emit;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;

public class Serializator
{
    private const string XML_PATH =
        "C:\\Users\\fromt\\RiderProjects\\Tracer.Serialization\\Tracer.Serialization.Xml\\bin\\Debug\\net6.0\\Tracer.Serialization.Xml.dll";

    private const string JSON_PATH =
        "C:\\Users\\fromt\\RiderProjects\\Tracer.Serialization\\Tracer.Serialization.Json\\bin\\Debug\\net6.0\\Tracer.Serialization.Json.dll";

    private const string YAML_PATH =
        "C:\\Users\\fromt\\RiderProjects\\Tracer.Serialization\\Tracer.Serialization.Yaml\\bin\\Debug\\net6.0\\Tracer.Serialization.Yaml.dll";

    private const string XML_TYPE = "Tracer.Serialization.Xml.XmlTraceResultSerializer";
    private const string JSON_TYPE = "Tracer.Serialization.Json.JsonTraceResultSerializer";
    private const string YAML_TYPE = "Tracer.Serialization.Yaml.YamlTraceResultSerializer";

    public Serializator()
    {
    }

    public void SerializeXml(TraceResult traceResult)
    {
        var serializator = MyLoader(XML_PATH, XML_TYPE);
        MySerializer(traceResult, serializator);
    }

    public void SerializeJson(TraceResult traceResult)
    {
        var serializator = MyLoader(JSON_PATH, JSON_TYPE);
        MySerializer(traceResult, serializator);
    }

    public void SerializeYaml(TraceResult traceResult)
    {
        var serializator = MyLoader(YAML_PATH, YAML_TYPE);
        MySerializer(traceResult, serializator);
    }

    private ITraceResultSerializer MyLoader(string path, string typeName)
    {
        var assembly = Assembly.LoadFrom(path);
        var type = assembly.GetType(typeName);
        return (ITraceResultSerializer)Activator.CreateInstance(type);
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