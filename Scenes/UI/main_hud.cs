using Godot;
using System;
using bridge;
using events;

public partial class main_hud : Control
{

	[Export] private NodePath interaction_prompt_path;
	private Control interaction_prompt;

	[Export] private NodePath puzzle_panel_path;
	private Control puzzle_panel;
	[Export] private NodePath path_subtitles;
	private Label subtitles;
	[Export] private NodePath path_subtitles_panel;
	private Control subtitles_panel;

	public override void _Ready()
	{
		interaction_prompt = this.GetNodeCustom<Control>(interaction_prompt_path);
		puzzle_panel = this.GetNodeCustom<Control>(puzzle_panel_path);
		subtitles = this.GetNodeCustom<Label>(path_subtitles);
		subtitles_panel = this.GetNodeCustom<Control>(path_subtitles_panel);
		
		interaction_prompt.Visible = false;
		puzzle_panel.Visible = false;

		SetSubtitlesValue("");
		EventBus.Instance.RequestSubtitle += SetSubtitlesValue;
		
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

	public void SetSubtitlesValue(string sub_text)
	{
		subtitles.Text = sub_text;
		subtitles_panel.Visible = sub_text != "";
	}

    public override void _ExitTree()
    {
		// Cleanup references
		EventBus.Instance.RequestSubtitle -= SetSubtitlesValue;
    }
}
