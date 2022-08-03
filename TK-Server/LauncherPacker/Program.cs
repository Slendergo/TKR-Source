using System.Security.Cryptography;

var map = new JsonMap();
map.Version = "1.0";

Console.WriteLine("Generating Checksums");

var client = Directory.GetFiles("Deployment/Client", "*", SearchOption.AllDirectories);
foreach(var path in client)
{
    var data = File.ReadAllBytes(path);

    using var md5 = MD5.Create();
    map.Records.Add(new Record()
    {
        Path = path,
        Checksum = Convert.ToHexString(md5.ComputeHash(data))
    });
    Console.WriteLine($"Generated Checksum: {path} -> {map.Records.Last().Checksum}");
}

var json = System.Text.Json.JsonSerializer.Serialize<JsonMap>(map, new System.Text.Json.JsonSerializerOptions() { WriteIndented = false });
using var file = File.CreateText("Deployment/CheckSum.json");
file.WriteLine(json);

Console.ReadKey();

public class JsonMap
{
    public string Version { get; set; }
    public List<Record> Records { get; set; } = new List<Record>();
}

public class Record
{
    public string Path { get; set; }
    public string Checksum { get; set; }
}
