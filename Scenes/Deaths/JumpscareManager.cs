using events;

public partial class JumpscareManager : Node3D
{

    [Export] private PackedScene jumpscare_scene;

    public override void _Ready() => 
        EventBus.Instance.OnPlayerDie += HandleOnPlayerDie;

    private void HandleOnPlayerDie()
    {
        EventBus.Instance.OnPlayerDie -= HandleOnPlayerDie;
        var node = jumpscare_scene.Instantiate();
        AddChild(node);
        Input.MouseMode = Input.MouseModeEnum.Visible;
    } 
}
