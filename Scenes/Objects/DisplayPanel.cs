using bridge;
using data;

public partial class DisplayPanel : Node3D
{
	[Export] private Texture2D tex_phase_0;
	[Export] private Texture2D tex_phase_1;
	[Export] private Texture2D tex_phase_2;
	[Export] private Texture2D tex_phase_end;

	[Export] private NodePath display_sprite_path;
	private Sprite3D display_sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		display_sprite = this.GetNodeCustom<Sprite3D>(display_sprite_path);
		GameDataManager.instance.OnPhaseChanged += UpdateDisplay;
		UpdateDisplay(GameDataManager.instance.GamePhase);
	}

	private void UpdateDisplay(int n_phase)
	{
		GD.Print($"Updating Phase Display: Phase #{n_phase}");
		Texture2D tex = null; // null texture basically hides the display. Desired functionality??
		switch(n_phase)
		{
			case GameData.PHASE_GENERATORS:
				tex = tex_phase_0;
				break;
			case GameData.PHASE_PUZZLE:
				tex = tex_phase_1;
				break;
			case GameData.PHASE_KEYS:
				tex = tex_phase_2;
				break;
			case GameData.PHASE_END:
				tex = tex_phase_end;
				break;
			// TODO add a game end icon???
		}
		display_sprite.Texture = tex;
	}

}
