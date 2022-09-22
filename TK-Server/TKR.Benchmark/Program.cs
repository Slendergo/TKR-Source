using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using System;

BenchmarkRunner.Run<Benchmark>();

[MemoryDiagnoser(true)]
public class Benchmark
{
    public Dictionary<int, string> Clients = new Dictionary<int, string>();

    public Benchmark()
    {
        for(var i =0; i < 300; i++)
            Clients.Add(i, RandomString(10));

        string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }
    }

    [Benchmark]
    public void A()
    {
        FindClient("Test");
    }

    [Benchmark]
    public void B()
    {
        FindClient("Test");
    }

    public string FindClient(string name) => Clients.Values.SingleOrDefault(_ => string.Equals(_, name, StringComparison.InvariantCultureIgnoreCase));

    public string FindClient2(string name)
    {
        foreach (var cname in Clients.Values)
            if (string.Equals(cname, name, StringComparison.InvariantCultureIgnoreCase))
                return cname;
        return null;
    }
}
