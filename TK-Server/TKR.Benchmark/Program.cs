using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<Benchmark>();

Console.WriteLine(DistTo(72, 58, 74, 27) + " < " + 15);

var d = SqDistTo(72, 58, 74, 27);
Console.WriteLine(d + " < "+ 15 * 15);

double DistTo(double x, double y, double xx, double yy)
{
    var dx = x - xx;
    var dy = y - yy;
    return Math.Sqrt(dx * dx + dy * dy);
}

double SqDistTo(double x, double y, double xx, double yy)
{
    var dx = x - xx;
    var dy = y - yy;
    return dx * dx + dy * dy;
}

[MemoryDiagnoser(true)]
public class Benchmark
{
    public List<Entity> Entities100 = new List<Entity>();
    public List<Entity> Entities10000 = new List<Entity>();

    public Benchmark()
    {
        Entities100 = new List<Entity>(100);
        for (var i = 0; i < 100; i++)
        {
            Entities10000.Add(new Entity()
            {
                X = Random.Shared.Next(0, 256),
                Y = Random.Shared.Next(0, 256)
            });
        }
        Entities10000 = new List<Entity>(10000);
        for (var i = 0; i < 10000; i++)
        {
            Entities10000.Add(new Entity()
            {
                X = Random.Shared.Next(0, 256),
                Y = Random.Shared.Next(0, 256)
            });
        }
    }

    [Benchmark]
    public Entity DistTo100()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities100)
        {
            var dist = entity.DistTo(32, 32);
            if (dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }

    [Benchmark]
    public Entity SqDistToA100()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities100)
        {
            var dist = entity.SqDistTo(32, 32);
            if (dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }

    [Benchmark]
    public Entity SqDistTo100()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities100)
        {
            var dist = entity.SqDistTo(32, 32);
            if (dist * dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }

    [Benchmark]
    public Entity DistTo100000()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities10000)
        {
            var dist = entity.DistTo(32, 32);
            if (dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }

    [Benchmark]
    public Entity SqDistToA100000()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities10000)
        {
            var dist = entity.SqDistTo(32, 32);
            if (dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }

    [Benchmark]
    public Entity SqDistToB100000()
    {
        Entity closestEntity = null;
        var closestDist = double.MinValue;
        foreach (var entity in Entities10000)
        {
            var dist = entity.SqDistTo(32, 32);
            if (dist * dist < closestDist)
            {
                closestEntity = entity;
                closestDist = dist;
            }
        }
        return closestEntity;
    }
}


public struct Position
{
    public double X;
    public double Y;
}

public class Entity
{
    public double X;
    public double Y;

    public double DistTo(ref Position pos)
    {
        var dx = pos.X - X;
        var dy = pos.Y - Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public double SqDistTo(ref Position pos)
    {
        var dx = pos.X - X;
        var dy = pos.Y - Y;
        return dx * dx + dy * dy;
    }

    public double DistTo(Entity entity)
    {
        var dx = entity.X - X;
        var dy = entity.Y - Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public double SqDistTo(Entity entity)
    {
        var dx = entity.X - X;
        var dy = entity.Y - Y;
        return dx * dx + dy * dy;
    }

    public double DistTo(double x, double y)
    {
        var dx = x - X;
        var dy = y - Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public double SqDistTo(double x, double y)
    {
        var dx = x - X;
        var dy = y - Y;
        return dx * dx + dy * dy;
    }
}