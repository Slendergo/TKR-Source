namespace wServer
{
    public enum EffectType
    {
        Potion = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        AreaBlast = 5,      //radius=pos1.x
        Dead = 6,
        Trail = 7,
        Diffuse = 8,        //radius=dist(pos1,pos2)
        Flow = 9,
        Trap = 10,          //radius=pos1.x
        Lightning = 11,     //particleSize=pos2.x
        Concentrate = 12,   //radius=dist(pos1,pos2)
        BlastWave = 13,     //origin=pos1, radius = pos2.x
        Earthquake = 14,
        Flashing = 15,      //period=pos1.x, numCycles=pos1.y
        BeachBall = 16
    }
}
