using data;
using Godot;
using System;

public partial class PuzzleControlPanel : Node3D
{
	private void OnInteracted()
	{
		GameDataManager.instance.PuzzleSolved = true;	
	}
}
