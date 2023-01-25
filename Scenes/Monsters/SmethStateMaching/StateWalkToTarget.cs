using bridge;
using Godot;
using state_machine;
using System;

public partial class StateWalkToTarget : State
{
	[Export] private int idle_state_index = 0;
	private bool target_reached = false;

  private Vector3 current_target;

    
  public override string GetStateAnimation()
  {
      return "WalkCycle";
  }

  public override void OnEnterState(StateData data)
  {
    var player = GetTree().GetFirstNodeInGroup("Player") as Node3D;
    float closest = float.MaxValue;
    float second = float.MaxValue;
    var close_target = data.machine.patrol_points[0];
    var second_target = data.machine.patrol_points[0];
    foreach (var pp in data.machine.patrol_points)
    {
      var distance = (player.GlobalPosition - pp.GlobalPosition).Length();
      if (distance < closest)
      {
        second_target = close_target;
        second = closest;
        close_target = pp;
        closest = distance;
      } else if (distance < second) {
        second_target = pp;
        second = distance;      
      }
    }
        
    data.nav_agent.TargetLocation = second_target.GlobalPosition;
    data.nav_agent.TargetReached += OnReachedTarget;
  }

  public override void OnExitState(StateData data)
  {
    data.nav_agent.TargetReached -= OnReachedTarget;
  }

  public override int StateMachineTick(StateData data, float delta)
  {
    if (target_reached) return idle_state_index;
    return RESULT_KEEP;
  }
	private void OnReachedTarget() => target_reached = true;

}
