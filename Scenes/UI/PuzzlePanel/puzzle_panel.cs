using bridge;
using data;

public partial class puzzle_panel : PanelContainer
{


	[Signal] public delegate void PanelCloseRequestEventHandler();


	[Export] private int correct_combination = 0;

	[ExportGroup("Node Paths")]
	[Export] private NodePath animation_player_path;
	[Export] private NodePath puzzle_slot_0_path;
	[Export] private NodePath puzzle_slot_1_path;
	[Export] private NodePath puzzle_slot_2_path;

	private puzzle_slot[] slots = new puzzle_slot[3];
	private AnimationPlayer anim_player;
	private bool disabled = false;

    public override void _Ready()
    {
		slots[0] = this.GetNodeCustom<puzzle_slot>(puzzle_slot_0_path);
		slots[1] = this.GetNodeCustom<puzzle_slot>(puzzle_slot_1_path);
		slots[2] = this.GetNodeCustom<puzzle_slot>(puzzle_slot_2_path);
		anim_player = this.GetNodeCustom<AnimationPlayer>(animation_player_path);
		anim_player.AnimationFinished += OnAnimCompleted;
    }

	private int GetCombo()
	{
		int combo = 0;
		combo += slots[0].Index;
		combo += slots[1].Index * 10;
		combo += slots[2].Index * 100;
		return combo;
	}

	private void VerifyCombo()
	{
		if (disabled) return;
		if (GetCombo() == correct_combination)
		{
			RunWinSequence();
		} else {
			RunFailSequence();
		}
	}

	private void RunWinSequence()
	{
		disabled = true;
		anim_player.Play("PuzzleCorrect");
	}

	private void RunFailSequence()
	{
		disabled = true;
		anim_player.Play("PuzzleWrong");
	}

	private void OnAnimCompleted(StringName anim_name)
	{
		if (anim_name == "PuzzleCorrect")
		{
			EmitSignal(nameof(PanelCloseRequest));
			GameDataManager.instance.PuzzleSolved = true;
			disabled = false;
			anim_player.Play("RESET");
		} else if (anim_name == "PuzzleWrong")
		{
			disabled = false;
			anim_player.Play("RESET");
		}
	}

	private void ExitPanel()
	{
		EmitSignal(nameof(PanelCloseRequest));
	}

}
