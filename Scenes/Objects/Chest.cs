using bridge;
using Godot;
using objects;
using System;

public partial class Chest : Node3D, IHasViability
{
	[Export] private PackedScene key_scene;
	[Export] private NodePath key_root_path;
	private Node3D key_root;
	[Export] private NodePath path_anim_player;
	private AnimationPlayer anim;

	public bool CanSpawnKey = false;

    public override void _Ready()
    {
		key_root = this.GetNodeCustom<Node3D>(key_root_path);
		anim = this.GetNodeCustom<AnimationPlayer>(path_anim_player);
    }

	public void OnInteract()
	{
		if (CanSpawnKey) anim.Play("Open");
		else anim.Play("OpenNonViable");
	}

	public void SpawnKey()
	{
		var key = key_scene.Instantiate();
		key_root.AddChild(key);
	}

    public bool GetViability() => CanSpawnKey;
    public void SetViability(bool is_viable) => CanSpawnKey = is_viable;
}
