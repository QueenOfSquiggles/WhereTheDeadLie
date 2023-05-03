using data;

public partial class Key : Node3D
{
	
	private void PickupKey()
	{
		GameDataManager.instance.FoundKeys += 1;
		QueueFree();
	}
}
