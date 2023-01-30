using bridge;
using Godot;
using System;

public partial class SceneManager : Node
{

    private static SceneManager _instance = null;

    [Export] private NodePath path_anim_player;
    private AnimationPlayer anim;

    public override void _Ready()
    {
        if (_instance != null)
        {
            QueueFree();
            return;
        }
        _instance = this;
        anim = this.GetNodeCustom<AnimationPlayer>(path_anim_player);
    }

    public static void LoadSceneFile(string scene_path) => _instance._LoadSceneFile(scene_path);
    public void _LoadSceneFile(string scene_path)
    {
        if (ResourceLoader.Load(scene_path) is not PackedScene packed)
        {
            GD.PushError($"Failed to load packed scene from file path: {scene_path}");
            return;
        }
        LoadScenePacked(packed);
    }

    public static void LoadScenePacked(PackedScene scene) => _instance._LoadScenePacked(scene);
    public async void _LoadScenePacked(PackedScene scene)
    {
        anim.Play("FadeOut");
        await ToSignal(anim, "animation_finished");
        GetTree().ChangeSceneToPacked(scene);
        anim.PlayBackwards("FadeOut");
    }


}
