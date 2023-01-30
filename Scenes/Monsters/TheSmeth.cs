using bridge;
using events;
using Godot;
using state_machine;
using System;
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
		SetPhysicsProcess(false);
		SetProcess(false);
    }

	private void StartGame()
	{
		// Player is out of the gate and ready to play now, get moving and hunting.
		SetPhysicsProcess(true);
		SetProcess(true);
	}


	public void LoadPatrolPoints(List<Marker3D> markers)
	{
		state_machine.patrol_points.AddRange(markers);
	}

	private void OnKillBoxHitBody(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			EventBus.Instance.TriggerOnPlayerDie();
			QueueFree(); // juse jumpscare controlled model
		}
	}



}
