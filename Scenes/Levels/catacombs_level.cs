using bridge;
using data;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class catacombs_level : Node3D
{

	[Signal] public delegate void OpenPuzzlePanelEventHandler();

	[Export] private NodePath chest_root_path;

	[Export] private NodePath path_smeth_patrol_points_parent;
	[Export] private NodePath path_smeth;
	private TheSmeth the_smeth;


	public override void _Ready()
	{
		SelectViableChests(this.GetNodeCustom<Node3D>(chest_root_path).GetChildren());
		the_smeth = this.GetNodeCustom<TheSmeth>(path_smeth);
		AddSmethPatrolPoints(this.GetNodeCustom<Node3D>(path_smeth_patrol_points_parent).GetChildren());
	}

	private void SelectViableChests(Array<Node> nodes)
	{
		List<Chest> chests = new();
		foreach (var c in nodes)
		{
            if (c is Chest chest && chest.Visible) chests.Add(chest);
        }

		var rand = new Random();
		int num_keys = GameData.REQUIRED_KEYS;
		for(int i = 0; i < num_keys; i++)
		{
			int index = rand.Next() % chests.Count;
			chests[index].CanSpawnKey = true;
			chests.RemoveAt(index);
		}

	}

	private void AddSmethPatrolPoints(Array<Node> nodes)
	{
		List<Marker3D> markers = new();
		foreach (Node n in nodes)
		{
			if (n is Marker3D point && point.Visible)
			{
				markers.Add(point);
			}
		}
		the_smeth.LoadPatrolPoints(markers);
	}


	public void TriggerOpenPanel()
	{
		EmitSignal(nameof(OpenPuzzlePanel));
	}

}
