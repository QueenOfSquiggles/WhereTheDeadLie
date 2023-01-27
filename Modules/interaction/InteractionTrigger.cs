namespace interaction;

using data;
using Godot;
using System;

public partial class InteractionTrigger : Area3D, IInteractable
{
	[Signal] public delegate void InteractionEventHandler();
	[Export(PropertyHint.Enum, "Generator Phase, Puzzle Phase, Key Phase, All Phases")] private int active_phase = 0;
	[Export] private bool is_one_shot = false;
	private const int PHASE_ALL = 3;


    public void TriggerInteraction()
	{
		if(IsActive())
		{
			EmitSignal(nameof(Interaction));
			if (is_one_shot) QueueFree();
		}
	}

    public bool IsActive()
    {
        return (active_phase == PHASE_ALL) || (GameDataManager.instance.GamePhase == active_phase);
    }

}
