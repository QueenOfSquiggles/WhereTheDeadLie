using state_machine;

public partial class StateInvestigate : State
{
	[Export] private int idle_state_index = 0;
	private bool target_reached = false;
	public override string GetStateAnimation()
    {
        return "WalkCycle";
    }

    public override void OnEnterState(StateData data)
    {
		target_reached = false;
		data.nav_agent.NavigationFinished += OnReachedTarget;
    }

    public override void OnExitState(StateData data)
    {
		data.nav_agent.NavigationFinished -= OnReachedTarget;
    }

    public override int StateMachineTick(StateData data, float delta)
    {
		if (target_reached) return idle_state_index;
		return RESULT_KEEP;
    }

	private void OnReachedTarget() => target_reached = true;
    
}
