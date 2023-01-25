using data;
using Godot;
using System;

public partial class PuzzleControlPanel : Node3D
{
	[Signal] public delegate void DisplayPuzzleEventHandler();

	private void OnInteracted()
	{
		EmitSignal(nameof(DisplayPuzzle));
	}
}
