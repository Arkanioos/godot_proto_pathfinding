using Godot;
using System;

public partial class CameraPosition : Node3D
{
	[Export]
	public Maze _maze;

	public override void _Ready()
	{
		_maze.GridMapPopulated += OnGridMapPopulated;
	}

	public override void _ExitTree()
	{
		_maze.GridMapPopulated -= OnGridMapPopulated;
	}


	private void OnGridMapPopulated(int width, int height)
	{
		GD.Print($"received signal with arguments: {width} - {height}");
		Position += new Vector3(width / 2, Math.Max(width, height) / 3 * 2, height / 2);
	}

}
