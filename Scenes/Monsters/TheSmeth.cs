using bridge;
using events;
using state_machine;
using System.Collections.Generic;

public partial class TheSmeth : CharacterBody3D
{
	[Export] private NodePath path_state_machine;
	private StateMachine state_machine;

    public override void _Ready()
    {
		state_machine = this.GetNodeCustom<StateMachine>(path_state_machine);
		EventBus.Instance.OnGameStart += StartGame;
		// effectively pauses The Smeth
		state_machine.SetPhysicsProcess(false);
		state_machine.SetProcess(false);
    }

	private void StartGame()
	{
		// Player is out of the gate and ready to play now, get moving and hunting.
		state_machine.SetPhysicsProcess(true);
		state_machine.SetProcess(true);
	}


    public void LoadPatrolPoints(List<Marker3D> markers) => 
		state_machine.patrol_points.AddRange(markers);

    private void OnKillBoxHitBody(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			EventBus.Instance.TriggerOnPlayerDie();
			QueueFree(); // juse jumpscare controlled model
		}
	}



}
