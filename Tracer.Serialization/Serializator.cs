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

    public Serializator()
    {
    }

    public void SerializeXml(TraceResult traceResult)
    {
        var assembly = Assembly.LoadFrom(XML_PATH);

        Type xmlType = assembly.GetType("Tracer.Serialization.Xml.XmlTraceResultSerializer");

        var xmlSerializer = (ITraceResultSerializer)Activator.CreateInstance(xmlType);

        using var fs = new FileStream($"result.{xmlSerializer.Format}", FileMode.OpenOrCreate);

        xmlSerializer.Serialize(traceResult, fs);

        Console.WriteLine($"Successfully serialized into {fs.Name}");
    }

    public void SerializeJson(TraceResult traceResult)
    {
        var assembly = Assembly.LoadFrom(JSON_PATH);

        Type jsonType = assembly.GetType("Tracer.Serialization.Json.JsonTraceResultSerializer");

        var jsonSerializer = (ITraceResultSerializer)Activator.CreateInstance(jsonType);

        using var fs = new FileStream($"result.{jsonSerializer.Format}", FileMode.OpenOrCreate);

        jsonSerializer.Serialize(traceResult, fs);

        Console.WriteLine($"Successfully serialized into {fs.Name}");
    }

    public void SerializeYaml(TraceResult traceResult)
    {
        var assembly = Assembly.LoadFrom(YAML_PATH);

        Type yamlType = assembly.GetType("Tracer.Serialization.Yaml.YamlTraceResultSerializer");

        var yamlSerializer = (ITraceResultSerializer)Activator.CreateInstance(yamlType);

        using var fs = new FileStream($"result.{yamlSerializer.Format}", FileMode.OpenOrCreate);

        yamlSerializer.Serialize(traceResult, fs);

        Console.WriteLine($"Successfully serialized into {fs.Name}");
    }
}