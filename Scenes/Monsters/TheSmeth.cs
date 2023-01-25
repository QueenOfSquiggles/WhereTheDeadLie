using bridge;
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
    }


	public void LoadPatrolPoints(List<Marker3D> markers)
	{
		state_machine.patrol_points.AddRange(markers);
	}



}
