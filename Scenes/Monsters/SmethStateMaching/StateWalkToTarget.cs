using bridge;
using data;
using Godot;
using state_machine;
using System;

public partial class StateWalkToTarget : State
{
	[Export] private int idle_state_index = 0;

  // TODO: this value should be determined by the aggression level
  [Export] private Curve chance_exact_targeting_curve;
  private float chance_use_closest_position = 0.25f; 
	private bool target_reached = false;

  private Vector3 current_target;

  private readonly Random random = new();


  public override void _Ready()
  {
      var sample_pos = GameDataManager.instance.GameAggression / 10.0f;
      chance_use_closest_position = chance_exact_targeting_curve.SampleBaked(sample_pos);
  }
  public override string GetStateAnimation()
  {
      return "WalkCycle";
  }

  public override void OnEnterState(StateData data)
  {
    target_reached = false;

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
    // randomly select between the first and second closest positions relative to the player
    var use_target = (random.NextSingle() < chance_use_closest_position)? close_target : second_target;
    data.nav_agent.TargetPosition = use_target.GlobalPosition;
    data.nav_agent.NavigationFinished += OnReachedTarget;
  }

  public override void OnExitState(StateData data)
  {
    data.nav_agent.NavigationFinished -= OnReachedTarget;
  }

  public override int StateMachineTick(StateData data, float delta)
  {
    if (target_reached) 
    {
      return idle_state_index;
    }
    return RESULT_KEEP;
  }
	private void OnReachedTarget() => target_reached = true;

}
