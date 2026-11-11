using System;
using System.Collections.Generic;

public sealed class GenerateMaze
{
    public static Tile[,] Generate(int cellWidth = 5, int cellHeight = 5)
    {
        int mazeWidth = cellWidth * 2 - 1;
        int mazeHeight = cellHeight * 2 - 1;
        Random random = new();
        Tile[,] maze = new Tile[mazeWidth, mazeHeight];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                bool isPath = i % 2 == 0 && j % 2 == 0;
                maze[i, j] = new Tile(i, j, isPath ? MazeTile.Cell : MazeTile.Wall);
            }
        }

        Tuple<int, int>[] directions =
        [
            new(0,-2), //UP
            new(2,0), //RIGHT
            new(0,2), //BOTTOM
            new(-2,0) //LEFT
        ];

        List<Tile> frontier = [];

        Tile startTile = maze[0, 0];
        startTile.Visited = true;
        startTile.TileType = MazeTile.Cell;

        foreach (Tuple<int, int> direction in directions)
        {
            int wallX = startTile.X + direction.Item1 / 2;
            if (wallX < 0 || wallX >= mazeWidth) continue;
            int wallY = startTile.Y + direction.Item2 / 2;
            if (wallY < 0 || wallY >= mazeHeight) continue;
            if (maze[wallX, wallY].TileType == MazeTile.Wall) frontier.Add(maze[wallX, wallY]);
        }

        while (frontier.Count > 0)
        {
            int randomWall = random.Next(0, frontier.Count);
            Tile deletedWall = frontier[randomWall];
            frontier.RemoveAt(randomWall);

            Tile unvisitedCell = null;

            foreach (Tuple<int, int> direction in directions)
            {
                int x = deletedWall.X + direction.Item1 / 2;
                if (x < 0 || x >= mazeWidth) continue;
                int y = deletedWall.Y + direction.Item2 / 2;
                if (y < 0 || y >= mazeHeight) continue;
                int inverse_x = deletedWall.X - direction.Item1 / 2;
                if (inverse_x < 0 || inverse_x >= mazeWidth) continue;
                int inverse_y = deletedWall.Y - direction.Item2 / 2;
                if (inverse_y < 0 || inverse_y >= mazeHeight) continue;
                Tile a = maze[x, y];
                Tile b = maze[inverse_x, inverse_y];
                if (a.Visited != b.Visited)
                {
                    unvisitedCell = a.Visited ? b : a;
                    break;
                }
            }
            if (unvisitedCell != null)
            {
                maze[deletedWall.X, deletedWall.Y].TileType = MazeTile.Cell;
                unvisitedCell.Visited = true;
                foreach (Tuple<int, int> direction in directions)
                {
                    int wallX = unvisitedCell.X + direction.Item1 / 2;
                    if (wallX < 0 || wallX >= mazeWidth) continue;
                    int wallY = unvisitedCell.Y + direction.Item2 / 2;
                    if (wallY < 0 || wallY >= mazeHeight) continue;
                    if (maze[wallX, wallY].TileType == MazeTile.Wall && !frontier.Contains(maze[wallX, wallY])) frontier.Add(maze[wallX, wallY]);
                }
                deletedWall.Visited = true;
            }
        }

        return maze;
    }
}