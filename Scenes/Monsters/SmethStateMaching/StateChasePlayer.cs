using state_machine;

public partial class StateChasePlayer : State
{
	[Export] private int idle_state_index = 0;
	[Export] private float min_distance_for_reevaluation;
	private bool target_reached = false;
	private Node3D player = null;
    public override string GetStateAnimation() => "WalkCycle";

    public override void OnEnterState(StateData data)
    {
		target_reached = false;
		data.nav_agent.NavigationFinished += OnReachedTarget;
		player = GetTree().GetFirstNodeInGroup("Player") as Node3D;
		data.nav_agent.TargetPosition = player.GlobalPosition;
    }

    public override void OnExitState(StateData data) => 
		data.nav_agent.NavigationFinished -= OnReachedTarget;

    public override int StateMachineTick(StateData data, float delta)
    {
		if (target_reached) return idle_state_index;

		var player_delta = (player.GlobalPosition - data.nav_agent.TargetPosition).Length();
		if (player_delta > min_distance_for_reevaluation) data.nav_agent.TargetPosition = player.GlobalPosition;
		
		return RESULT_KEEP;
    }

	private void OnReachedTarget() => target_reached = true;

}
