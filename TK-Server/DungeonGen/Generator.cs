using dungeonGen.definitions;
using dungeonGen.templates;
using RotMG.Common;
using RotMG.Common.Rasterizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dungeonGen
{
    public class Generator
    {
        private readonly Random rand;
        private readonly DungeonTemplate template;

        private RoomCollision collision;
        private int maxDepth;
        private int maxRoomNum;
        private int minRoomNum;
        private List<Room> rooms;
        private Room rootRoom;

        public Generator(int seed, DungeonTemplate template)
        {
            rand = new Random(seed);
            this.template = template;
            Step = GenerationStep.Initialize;
        }

        public GenerationStep Step { get; set; }

        public DungeonGraph ExportGraph()
        {
            if (Step != GenerationStep.Finish)
                throw new InvalidOperationException();

            return new DungeonGraph(template, rooms.ToArray());
        }

        public void Generate(GenerationStep? targetStep = null)
        {
            while (Step != targetStep && Step != GenerationStep.Finish)
                RunStep();
        }

        public IEnumerable<Room> GetRooms() => rooms;

        private void GenerateBranches()
        {
            var numRooms = new Range(minRoomNum, maxRoomNum).Random(rand);

            List<Room> copy;

            while (rooms.Count < numRooms)
            {
                copy = new List<Room>(rooms);
                rand.Shuffle(copy);

                var worked = false;

                foreach (var room in copy)
                {
                    if (rooms.Count > numRooms)
                        break;

                    if (rand.Next() % 2 == 0)
                        continue;

                    worked |= GenerateBranchInternal(room, room.Depth + 1, template.MaxDepth, true);
                }

                if (!worked)
                    break;
            }
        }

        private bool GenerateBranchInternal(Room prev, int depth, int maxDepth, bool doBranch)
        {
            if (depth >= maxDepth)
                return false;

            var connPtNum = GetMaxConnectionPoints(prev);
            var seq = Enumerable.Range(0, connPtNum).ToList();

            rand.Shuffle(seq);

            if (doBranch)
            {
                var numBranch = prev.NumBranches.Random(rand);
                numBranch -= prev.Edges.Count;

                for (var i = 0; i < numBranch; i++)
                {
                    var rm = template.CreateNormal(depth, prev);

                    Link? link = null;

                    foreach (var connPt in seq)
                        if ((link = PlaceRoom(prev, rm, connPt)) != null)
                        {
                            seq.Remove(connPt);
                            break;
                        }

                    if (link == null)
                        return false;

                    Edge.Link(prev, rm, link.Value);

                    if (!GenerateBranchInternal(rm, depth + 1, maxDepth, false))
                    {
                        collision.Remove(rm);
                        Edge.UnLink(prev, rm);
                        return false;
                    }

                    rm.Depth = depth;
                    rooms.Add(rm);
                }
            }
            else
            {
                while (prev.Edges.Count < prev.NumBranches.Begin)
                {
                    var rm = template.CreateNormal(depth, prev);

                    Link? link = null;

                    foreach (var connPt in seq)
                        if ((link = PlaceRoom(prev, rm, connPt)) != null)
                        {
                            seq.Remove(connPt);
                            break;
                        }

                    if (link == null)
                        return false;

                    Edge.Link(prev, rm, link.Value);

                    if (!GenerateBranchInternal(rm, depth + 1, maxDepth, false))
                    {
                        collision.Remove(rm);
                        Edge.UnLink(prev, rm);
                        return false;
                    }

                    rm.Depth = depth;
                    rooms.Add(rm);
                }
            }

            return true;
        }

        private bool GenerateSpecialInternal(Room prev, int depth, int targetDepth)
        {
            var connPtNum = GetMaxConnectionPoints(prev);
            var seq = Enumerable.Range(0, connPtNum).ToList();

            rand.Shuffle(seq);

            bool specialPlaced;
            do
            {
                Room rm;

                if (targetDepth == depth)
                    rm = template.CreateSpecial(depth, prev);
                else
                    rm = template.CreateNormal(depth, prev);

                Link? link = null;

                foreach (var connPt in seq)
                    if ((link = PlaceRoom(prev, rm, connPt)) != null)
                    {
                        seq.Remove(connPt);
                        break;
                    }

                if (link == null)
                    return false;

                if (targetDepth == depth)
                    specialPlaced = true;
                else
                    specialPlaced = GenerateSpecialInternal(rm, depth + 1, targetDepth);

                if (specialPlaced)
                {
                    rm.Depth = depth;
                    Edge.Link(prev, rm, link.Value);
                    rooms.Add(rm);
                }
                else
                    collision.Remove(rm);
            } while (!specialPlaced);

            return true;
        }

        private void GenerateSpecials()
        {
            if (template.SpecialRmCount == null)
                return;

            var numRooms = (int)template.SpecialRmCount.NextValue();

            for (var i = 0; i < numRooms; i++)
            {
                int targetDepth;

                do targetDepth = (int)template.SpecialRmDepthDist.NextValue();
                while (targetDepth > maxDepth * 3 / 2);

                var generated = false;

                do
                {
                    var room = rooms[rand.Next(rooms.Count)];

                    if (room.Depth >= targetDepth)
                        continue;

                    generated = GenerateSpecialInternal(room, room.Depth + 1, targetDepth);
                } while (!generated);
            }
        }

        private bool GenerateTarget()
        {
            var targetDepth = (int)template.TargetDepth.NextValue();

            rootRoom = template.CreateStart(0);
            rootRoom.Pos = new Point(0, 0);
            collision.Add(rootRoom);
            rooms.Add(rootRoom);

            if (GenerateTargetInternal(rootRoom, 1, targetDepth))
            {
                minRoomNum = targetDepth * template.NumRoomRate.Begin;
                maxRoomNum = targetDepth * template.NumRoomRate.End;
                maxDepth = rooms.Count;
                return true;
            }

            return false;
        }

        private bool GenerateTargetInternal(Room prev, int depth, int targetDepth)
        {
            var connPtNum = GetMaxConnectionPoints(prev);
            var seq = Enumerable.Range(0, connPtNum).ToList();

            rand.Shuffle(seq);

            bool targetPlaced;

            do
            {
                Room rm;

                if (targetDepth == depth)
                    rm = template.CreateTarget(depth, prev);
                else
                    rm = template.CreateNormal(depth, prev);

                Link? link = null;

                foreach (var connPt in seq)
                    if ((link = PlaceRoom(prev, rm, connPt)) != null)
                    {
                        seq.Remove(connPt);
                        break;
                    }

                if (link == null)
                    return false;

                if (targetDepth == depth)
                    targetPlaced = true;
                else
                    targetPlaced = GenerateTargetInternal(rm, depth + 1, targetDepth);

                if (targetPlaced)
                {
                    rm.Depth = depth;
                    Edge.Link(prev, rm, link.Value);
                    rooms.Add(rm);
                }
                else
                    collision.Remove(rm);
            } while (!targetPlaced);
            return true;
        }

        private int GetMaxConnectionPoints(Room rm) => rm is FixedRoom ? ((FixedRoom)rm).ConnectionPoints.Length : 4;

        private Link? PlaceRoom(Room src, Room target, int connPt)
        {
            var sep = template.RoomSeparation.Random(rand);

            if (src is FixedRoom && target is FixedRoom)
                return PlaceRoomFixed((FixedRoom)src, (FixedRoom)target, connPt, sep);

            if (src is FixedRoom)
                return PlaceRoomSourceFixed((FixedRoom)src, target, connPt, sep);

            if (target is FixedRoom)
                return PlaceRoomTargetFixed(src, (FixedRoom)target, connPt, sep);

            return PlaceRoomFree(src, target, (Direction)connPt, sep);
        }

        private Link? PlaceRoomFixed(FixedRoom src, FixedRoom target, int connPt, int sep)
        {
            var conn = src.ConnectionPoints[connPt];
            var targetDirection = conn.Item1.Reverse();
            var targetConns = (Tuple<Direction, int>[])target.ConnectionPoints.Clone();

            rand.Shuffle(targetConns);

            Tuple<Direction, int> targetConnPt = null;

            foreach (var targetConn in targetConns)
                if (targetConn.Item1 == targetDirection)
                {
                    targetConnPt = targetConn;
                    break;
                }

            if (targetConnPt == null)
                return null;

            int x, y;
            Link? link = null;

            switch (conn.Item1)
            {
                case Direction.North:
                case Direction.South:
                    x = src.Pos.X + conn.Item2 - targetConnPt.Item2;

                    if (conn.Item1 == Direction.South)
                        y = src.Pos.Y + src.Height + sep;
                    else
                        y = src.Pos.Y - sep - target.Height;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    link = new Link(conn.Item1, src.Pos.X + conn.Item2);
                    break;

                case Direction.East:
                case Direction.West:
                    y = src.Pos.Y + conn.Item2 - targetConnPt.Item2;

                    if (conn.Item1 == Direction.East)
                        x = src.Pos.X + src.Width + sep;
                    else
                        x = src.Pos.X - sep - target.Width;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    link = new Link(conn.Item1, src.Pos.Y + conn.Item2);
                    break;
            }

            collision.Add(target);
            return link;
        }

        private Link? PlaceRoomFree(Room src, Room target, Direction connPt, int sep)
        {
            int x, y;
            Link? link = null;

            switch (connPt)
            {
                case Direction.North:
                case Direction.South:
                    var minX = src.Pos.X + template.CorridorWidth - target.Width;
                    var maxX = src.Pos.X + src.Width - template.CorridorWidth;

                    x = rand.Next(minX, maxX + 1);

                    if (connPt == Direction.South)
                        y = src.Pos.Y + src.Height + sep;
                    else
                        y = src.Pos.Y - sep - target.Height;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    var linkX = new Range(src.Pos.X, src.Pos.X + src.Width).Intersection(new Range(target.Pos.X, target.Pos.X + target.Width));

                    link = new Link(connPt, new Range(linkX.Begin, linkX.End - template.CorridorWidth).Random(rand));
                    break;

                case Direction.East:
                case Direction.West:
                    var minY = src.Pos.Y + template.CorridorWidth - target.Height;
                    var maxY = src.Pos.Y + src.Height - template.CorridorWidth;

                    y = rand.Next(minY, maxY + 1);

                    if (connPt == Direction.East)
                        x = src.Pos.X + src.Width + sep;
                    else
                        x = src.Pos.X - sep - target.Width;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    var linkY = new Range(src.Pos.Y, src.Pos.Y + src.Height).Intersection(new Range(target.Pos.Y, target.Pos.Y + target.Height));

                    link = new Link(connPt, new Range(linkY.Begin, linkY.End - template.CorridorWidth).Random(rand));
                    break;
            }

            collision.Add(target);
            return link;
        }

        private Link? PlaceRoomSourceFixed(FixedRoom src, Room target, int connPt, int sep)
        {
            var conn = src.ConnectionPoints[connPt];

            int x, y;
            Link? link = null;

            switch (conn.Item1)
            {
                case Direction.North:
                case Direction.South:
                    var minX = src.Pos.X + conn.Item2 + template.CorridorWidth - target.Width;
                    var maxX = src.Pos.X + conn.Item2;

                    x = rand.Next(minX, maxX + 1);

                    if (conn.Item1 == Direction.South)
                        y = src.Pos.Y + src.Height + sep;
                    else
                        y = src.Pos.Y - sep - target.Height;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    link = new Link(conn.Item1, src.Pos.X + conn.Item2);
                    break;

                case Direction.East:
                case Direction.West:
                    var minY = src.Pos.Y + conn.Item2 + template.CorridorWidth - target.Height;
                    var maxY = src.Pos.Y + conn.Item2;
                    y = rand.Next(minY, maxY + 1);

                    if (conn.Item1 == Direction.East)
                        x = src.Pos.X + src.Width + sep;
                    else
                        x = src.Pos.X - sep - target.Width;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    var linkY = new Range(src.Pos.Y, src.Pos.Y + src.Height).Intersection(new Range(target.Pos.Y, target.Pos.Y + target.Height));

                    link = new Link(conn.Item1, src.Pos.Y + conn.Item2);
                    break;
            }

            collision.Add(target);
            return link;
        }

        private Link? PlaceRoomTargetFixed(Room src, FixedRoom target, int connPt, int sep)
        {
            var targetDir = ((Direction)connPt).Reverse();
            var connPts = (Tuple<Direction, int>[])target.ConnectionPoints.Clone();

            rand.Shuffle(connPts);

            Tuple<Direction, int> conn = null;

            foreach (var pt in connPts)
            {
                if (pt.Item1 == targetDir)
                {
                    conn = pt;
                    break;
                }
            }

            if (conn == null)
                return null;

            int x, y;
            Link? link = null;

            switch (conn.Item1)
            {
                case Direction.North:
                case Direction.South:
                    var minX = src.Pos.X - conn.Item2;
                    var maxX = src.Pos.X + src.Width - template.CorridorWidth - conn.Item2;

                    x = rand.Next(minX, maxX + 1);

                    if (conn.Item1 == Direction.North)
                        y = src.Pos.Y + src.Height + sep;
                    else
                        y = src.Pos.Y - sep - target.Height;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    link = new Link((Direction)connPt, target.Pos.X + conn.Item2);
                    break;

                case Direction.East:
                case Direction.West:
                    var minY = src.Pos.Y - conn.Item2;
                    var maxY = src.Pos.Y + src.Height - template.CorridorWidth - conn.Item2;

                    y = rand.Next(minY, maxY + 1);

                    if (conn.Item1 == Direction.West)
                        x = src.Pos.X + src.Width + sep;
                    else
                        x = src.Pos.X - sep - target.Width;

                    target.Pos = new Point(x, y);

                    if (collision.HitTest(target))
                        return null;

                    link = new Link((Direction)connPt, target.Pos.Y + conn.Item2);
                    break;
            }

            collision.Add(target);
            return link;
        }

        private void RunStep()
        {
            switch (Step)
            {
                case GenerationStep.Initialize:
                    template.SetRandom(rand);
                    template.Initialize();
                    collision = new RoomCollision();
                    rootRoom = null;
                    rooms = new List<Room>();
                    break;

                case GenerationStep.TargetGeneration:
                    if (!GenerateTarget())
                    {
                        Step = GenerationStep.Initialize;
                        return;
                    }
                    break;

                case GenerationStep.SpecialGeneration:
                    GenerateSpecials();
                    break;

                case GenerationStep.BranchGeneration:
                    GenerateBranches();
                    break;
            }

            Step++;
        }
    }
}
