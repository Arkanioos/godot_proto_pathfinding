using System.Collections.Generic;
using Godot;

public partial class Maze : GridMap
{
	[Export]
	private int _width, _height;

	[Signal]
	public delegate void GridMapPopulatedEventHandler(int width, int height);

	private Timer _timer;

	private Tile[,] _maze;

	private List<Transform3D> _transform;
	private List<Mesh> _meshes;

	public override void _Ready()
	{
		InitializeData();
		SubscribeEvents();
		InitializeMaze();
		PopulateGridMap();
	}

	private void InitializeData()
	{
		GD.Print("Initializing data");
		_timer = (Timer)GetNode("Timer");
		_timer.WaitTime = 2;
		_timer.OneShot = true;
		_timer.Start();
		GD.Print("Timer set");
		foreach (Variant variant in GetMeshes())
		{
			if (variant.VariantType == Variant.Type.Transform3D)
			{
				Transform3D transform = variant.As<Transform3D>();
				GD.Print(transform.Origin);
			}
			else
			{
				Mesh mesh = variant.As<Mesh>();
				GD.Print("mesh");
			}
		}
		GD.Print("Initialized data");
	}

	private void SubscribeEvents()
	{
		_timer.Timeout += ComputePath;
	}

	public override void _ExitTree()
	{
		_timer.Timeout -= ComputePath;
	}


	private void InitializeMaze()
	{
		GD.Print("Generating maze");
		_maze = GenerateMaze.Generate(_width, _height);
		GD.Print("Generated maze");
	}

	private void PopulateGridMap()
	{
		GD.Print("Populating grid map");
		int width = _maze.GetLength(0);
		int height = _maze.GetLength(1);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = _maze[i, j];
				Vector3I tilePosition = new(tile.X, 0, tile.Y);
				SetCellItem(tilePosition, (int)tile.TileType, 0);
			}
		}
		EmitSignal(SignalName.GridMapPopulated, width, height);
		GD.Print("Populated grid map");
	}

	private void ComputePath()
	{
		List<Tile> path = AStar.Compute(_maze);
		foreach (Tile tile in path)
		{
			Vector3I tilePosition = new(tile.X, 0, tile.Y);
			SetCellItem(tilePosition, -1, 0);
			SetCellItem(tilePosition, (int)MazeTile.Path, 0);
		}
	}
}
