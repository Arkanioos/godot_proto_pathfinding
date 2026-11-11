using System;
using System.Collections.Generic;
using System.Transactions;
using Godot;

public sealed class AStar
{
    // heuristic -> h(n)=∣xn​−xg​∣+∣yn​−yg​∣

    public static List<Tile> Compute(Tile[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        Tuple<int, int>[] directions =
        [
            new(0,-1), //UP
            new(1,0), //RIGHT
            new(0,1), //BOTTOM
            new(-1,0) //LEFT
        ];

        PriorityQueue<Tile, int> openSet = new();
        HashSet<Tile> closedSet = [];
        Tile start = maze[0, 0];
        Tile goal = maze[width - 1, height - 1];
        start.HCost = ManhattanDistance(start, goal);

        openSet.Enqueue(start, start.FCost);
        while (openSet.Count > 0)
        {
            Tile current = openSet.Dequeue();
            if (closedSet.Contains(current)) continue;
            if (current == goal) return ReconstructPath(goal);
            closedSet.Add(current);
            foreach (Tuple<int, int> direction in directions)
            {
                int x = current.X + direction.Item1;
                if (x < 0 || x >= width) continue;
                int y = current.Y + direction.Item2;
                if (y < 0 || y >= height) continue;
                Tile neighbor = maze[x, y];
                if (neighbor.TileType == MazeTile.Wall) continue;
                if (closedSet.Contains(neighbor)) continue;
                int neighborGCost = current.GCost + 1;
                if (neighborGCost >= neighbor.GCost) continue;
                neighbor.Parent = current;
                neighbor.GCost = neighborGCost;
                neighbor.HCost = ManhattanDistance(neighbor, goal);
                openSet.Enqueue(neighbor, neighbor.FCost);
            }
        }

        return null;
    }

    private static int ManhattanDistance(Tile a, Tile b)
    {
        return Math.Abs(a.X - b.X) +
               Math.Abs(a.Y - b.Y);
    }

    private static List<Tile> ReconstructPath(Tile goal)
    {
        List<Tile> path = [];
        Tile current = goal;
        while (current != null)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}