public sealed class Tile(int x, int y, MazeTile tile, bool visited = false, int gCost = int.MaxValue, int hCost = 0, Tile parent = null)
{
    public readonly int X = x, Y = y;
    public MazeTile TileType = tile;
    public bool Visited = visited;

    // for a*

    public int GCost = gCost;
    public int HCost = hCost;
    public int FCost => GCost + HCost;
    public Tile Parent = parent;
}