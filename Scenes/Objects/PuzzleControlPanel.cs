using bridge;

public partial class PuzzleControlPanel : Node3D
{
	[Signal] public delegate void DisplayPuzzleEventHandler();

	[Export] private NodePath path_sfx_audio;
	private AudioStreamPlayer3D sfx_audio;

    public override void _Ready() => 
		sfx_audio = this.GetNodeCustom<AudioStreamPlayer3D>(path_sfx_audio);

    private void OnInteracted()
	{
		sfx_audio.Play();
		EmitSignal(nameof(DisplayPuzzle));
	}
}
