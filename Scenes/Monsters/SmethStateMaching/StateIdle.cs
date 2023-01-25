using Godot;
using state_machine;
using System;

public partial class StateIdle : State
{

    private float countdown;

    [Export] private float min_countdown_time = 10.0f;
    [Export] private float max_countdown_time = 30.0f;
    [Export] private int state_walk_to_target_index = 1;
 
    public override string GetStateAnimation()
    {
        return "Idle";
    }

    public override void OnEnterState(StateData data)
    {
      var rand = new Random();
      var range = max_countdown_time - min_countdown_time;
      countdown = min_countdown_time + (rand.NextSingle() * range);
    }

    public override int StateMachineTick(StateData data, float delta)
    {
      if (countdown > 0f)
      {
        countdown -= delta;
    		return RESULT_KEEP;
      }
      return state_walk_to_target_index;
    }

}
