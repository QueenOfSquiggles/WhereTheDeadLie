using bridge;
using data;
using events;

public partial class VictorySection : Node3D
{
    [ExportGroup("Animation")]
    [Export] private NodePath path_anim_player;
    [Export] private NodePath path_area_trigger;
    [Export] private NodePath path_intro_area;
    [Export(PropertyHint.File, "*.tscn")] private string main_menu_file = "";

    [ExportGroup("Audio")]
    [Export] private NodePath path_vo_player;
    private AudioStreamPlayer vo_player;
    private AnimationPlayer anim;
    private Area3D area_trigger;
    private Area3D area_intro;

    public override void _Ready()
    {
        vo_player = this.GetNodeCustom<AudioStreamPlayer>(path_vo_player);
        GameDataManager.instance.OnPhaseChanged += OnPhaseChange;
        anim = this.GetNodeCustom<AnimationPlayer>(path_anim_player);
        area_trigger = this.GetNodeCustom<Area3D>(path_area_trigger);
        area_intro = this.GetNodeCustom<Area3D>(path_intro_area);
        area_intro.BodyEntered += OnBodyEnterTrigger;
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("ui_accept") && vo_player.Playing)
        {
            vo_player.Stop();
            if (anim.IsPlaying())
            {
                var data = anim.GetAnimation(anim.CurrentAnimation);
                anim.Seek(data.Length, true);
            }
            GetViewport().SetInputAsHandled();
        }
    }

    private void OnPhaseChange(int phase)
    {
        switch (phase)
        {
            case GameData.PHASE_PUZZLE:
                anim.Play("PuzzlePrompt");
                break;
    
            case GameData.PHASE_KEYS:
                anim.Play("KeysPrompt");
                break;
    
            case GameData.PHASE_END:
                EndSequence();
                break;
        }
    }

    private async void EndSequence()
    {
        area_trigger.BodyEntered += OnBodyEnterTrigger;
        anim.Play("ExitPrompt");
        await ToSignal(anim, "animation_finished");
        anim.Play("GateOpen");

    }

    private async void OnBodyEnterTrigger(Node body)
    {
        if (!body.IsInGroup("Player")) return;
        
        switch (GameDataManager.instance.GamePhase)
        {
            case GameData.PHASE_GENERATORS:
                
                area_intro.BodyEntered -= OnBodyEnterTrigger;
                area_intro.QueueFree();

                anim.Play("GateShut");
                await ToSignal(anim, "animation_finished");
                
                EventBus.Instance.TriggerOnGameStart();
                anim.Play("GeneratorPrompt");
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
