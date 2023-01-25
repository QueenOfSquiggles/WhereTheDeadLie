using bridge;
using Godot;
using System;

public partial class Chest : Node3D
{
	[Export] private PackedScene key_scene;
	[Export] private NodePath key_root_path;
	private Node3D key_root;

	public bool CanSpawnKey = false;

    public override void _Ready()
    {
		key_root = this.GetNodeCustom<Node3D>(key_root_path);
    }

	public void SpawnKey()
	{
		if (!CanSpawnKey) return;
		
		var key = key_scene.Instantiate();
		key_root.AddChild(key);
	}
}
