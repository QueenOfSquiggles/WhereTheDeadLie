using bridge;
using data;
using objects;

public partial class power_generator : Node3D, IHasViability
{

	[Export] private PackedScene activity_components_scene;

	[Export] private NodePath anim_player_path;
	private AnimationPlayer anim;
	private bool is_viable = false;

	public override void _Ready()
	{
		anim = this.GetNodeCustom<AnimationPlayer>(anim_player_path);
	}

	private void OnGeneratorStarted()
	{
		GameDataManager.instance.ActiveGenerators += 1;
		var components = activity_components_scene.Instantiate();
		AddChild(components);
	}

	public void OnInteract()
	{
		if (is_viable) 
		{
			anim.Play("ActivateViable");
			anim.AnimationFinished += OnAnimFinish;
		} else {
			anim.Play("ActivateNonViable");
		} 
	}

	private void OnAnimFinish(StringName anim_name)
	{
		anim.AnimationFinished -= OnAnimFinish;
		anim.Play("ActiveLoop");
	}

    public bool GetViability() => is_viable;
    public void SetViability(bool is_viable) => this.is_viable = is_viable;
}
