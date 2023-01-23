using Godot;
using System;
using bridge;

public partial class main_hud : Control
{

	[Export] private NodePath interaction_prompt_path;
	private Control interaction_prompt;

	public override void _Ready()
	{
		interaction_prompt = this.GetNodeCustom<Control>(interaction_prompt_path);
		interaction_prompt.Visible = false;
	}

	public void OnInteractionStart() {
		interaction_prompt.Visible = true;
	}

	public void OnInteractionEnd() {
		interaction_prompt.Visible = false;
	}
}
