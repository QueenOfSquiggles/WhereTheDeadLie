using Godot;
using interaction;
using System;
using bridge;
using data;

public partial class power_generator : Node3D
{

	[Export] private PackedScene activity_components_scene;

	[Export] private NodePath interaction_trigger_path;
	private InteractionTrigger interaction_trigger; 

	public override void _Ready()
	{
		interaction_trigger = this.GetNodeCustom<InteractionTrigger>(interaction_trigger_path);
	}

	private void OnGeneratorStarted()
	{
		GameDataManager.instance.ActiveGenerators += 1;
		var components = activity_components_scene.Instantiate();
		AddChild(components);
	}
}
