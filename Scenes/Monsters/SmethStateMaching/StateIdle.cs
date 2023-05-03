using data;
using state_machine;

public partial class StateIdle : State
{

    private float countdown;

    [Export] private float curve_range_width = 0.1f;
    [Export] private int state_walk_to_target_index = 1;
    [Export] private Curve countdown_time_curve;

    private float min_countdown_time = 10.0f;
    private float max_countdown_time = 30.0f;
    public override void _Ready()
    {
      var h_width = curve_range_width / 2.0f;
      var sample_pos = GameDataManager.instance.GameAggression / 10.0f;
      // basically we sample upper and lower relative to curve position. the width determines how wildly the min/max vary
      min_countdown_time = countdown_time_curve.SampleBaked(Mathf.Clamp(sample_pos + h_width, 0.0f, 1.0f));
      max_countdown_time = countdown_time_curve.SampleBaked(Mathf.Clamp(sample_pos - h_width, 0.0f, 1.0f));

      if (max_countdown_time < min_countdown_time)
      { 
        // For safety in case the curve has a mistake, swap so the math works out.
        (min_countdown_time, max_countdown_time) = (max_countdown_time, min_countdown_time);
      }
    }

    public override string GetStateAnimation() => "Idle";

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
