using events;
using Godot;
using System;

public partial class JumpscareManager : Node
{

    [Export] private PackedScene jumpscare_scene;

    public override void _Ready()
    {
        EventBus.Instance.OnPlayerDie += HandleOnPlayerDie;
    }

    private void HandleOnPlayerDie()
    {
        EventBus.Instance.OnPlayerDie -= HandleOnPlayerDie;
        GetTree().ChangeSceneToPacked(jumpscare_scene);
    }
}
