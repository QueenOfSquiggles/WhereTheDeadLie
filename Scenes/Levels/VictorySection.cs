using bridge;
using data;
using events;
using Godot;
using System;

public partial class VictorySection : Node3D
{
    [Export] private NodePath path_anim_player;
    [Export] private NodePath path_area_trigger;
    [Export] private NodePath path_intro_area;
    [Export(PropertyHint.File, "*.tscn")] private string main_menu_file = "";
    private AnimationPlayer anim;
    private Area3D area_trigger;
    private Area3D area_intro;

    public override void _Ready()
    {
        GameDataManager.instance.OnPhaseChanged += OnPhaseChange;
        anim = this.GetNodeCustom<AnimationPlayer>(path_anim_player);
        area_trigger = this.GetNodeCustom<Area3D>(path_area_trigger);
        area_intro = this.GetNodeCustom<Area3D>(path_intro_area);
        area_intro.BodyEntered += OnBodyEnterTrigger;
    }

    private void OnPhaseChange(int phase)
    {
        if (phase != GameData.PHASE_END) return;
        area_trigger.BodyEntered += OnBodyEnterTrigger;
        anim.Play("GateOpen");
    }

    private async void OnBodyEnterTrigger(Node body)
    {
        if (!body.IsInGroup("Player")) return;
        
        switch (GameDataManager.instance.GamePhase)
        {
            case GameData.PHASE_GENERATORS:
                area_intro.BodyEntered -= OnBodyEnterTrigger;
                EventBus.Instance.TriggerOnGameStart();
                area_intro.QueueFree();
                anim.Play("GateShut");  
                break;

            case GameData.PHASE_END:
                area_trigger.BodyEntered -= OnBodyEnterTrigger;
                anim.Play("GateShut");  
                await ToSignal(anim, "animation_finished");
                anim.Play("EndSequence");
                break;

            default:
                return; // invalid phase. Effectively should never happen
        }

        // shut the gate either way, no reason to double the call
    }

    private void ReturnToMainMenu() => SceneManager.LoadSceneFile(main_menu_file);

}
