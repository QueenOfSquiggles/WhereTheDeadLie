using Godot;
using System;
using bridge;

public partial class main_hud : Control
{

	[Export] private NodePath interaction_prompt_path;
	private Control interaction_prompt;

	[Export] private NodePath puzzle_panel_path;
	private Control puzzle_panel;

	public override void _Ready()
	{
		interaction_prompt = this.GetNodeCustom<Control>(interaction_prompt_path);
		interaction_prompt.Visible = false;
		puzzle_panel = this.GetNodeCustom<Control>(puzzle_panel_path);
		puzzle_panel.Visible = false;
	}

	public void OnInteractionStart() {
		interaction_prompt.Visible = true;
	}

	public void OnInteractionEnd() {
		interaction_prompt.Visible = false;
	}

	public void DisplayPuzzlePanel()
	{
		puzzle_panel.Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	public void HidePuzzlePanel()
	{
		puzzle_panel.Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
