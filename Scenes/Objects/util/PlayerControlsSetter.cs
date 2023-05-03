using events;

public partial class PlayerControlsSetter : Node
{

	public void SetPlayerCanMove(bool can_move)
	{
		EventBus.Instance.TriggerSetPlayerCanMove(can_move);
	}
}
