using bridge;

public partial class VisibilityChecker : Node3D
{

	[Signal] public delegate void OnVisibleEventHandler();
	[Signal] public delegate void OnNoLongerVisibleEventHandler();

	[Export] private StringName target_group_name = "";

	[Export] private NodePath path_raycast;
	private RayCast3D raycast;


	private bool is_in_area = false;
	private bool visible_in_area = false;


    public override void _Ready() => 
		raycast = this.GetNodeCustom<RayCast3D>(path_raycast);

    private void OnBodyEnterArea(Node3D body){
		if (body.IsInGroup(target_group_name)) 
		{
			is_in_area = true;
			// GD.Print("Target entered bounding box");
		} 
	}

	private void OnBodyExitArea(Node3D body){
		if (body.IsInGroup(target_group_name)) 
		{
			is_in_area = false;
			visible_in_area = false;
			// GD.Print("Target left bounding box");
		} 
	}

    public override void _PhysicsProcess(double _d)
    { // Nesting is wayy too deep, but lazy right now. 
		// TODO refactor our this method for shallower nesting
		if (!is_in_area) return;
		var target = GetClosestTarget();
        raycast.LookAt(target.GlobalPosition, Vector3.Up);
		// delta *= 1.1f; // vector goes towards target and slightly further
		// raycast.TargetPosition = delta;

		raycast.ForceRaycastUpdate();
		if (raycast.IsColliding())
		{
            if (raycast.GetCollider() is Node3D coll && coll.IsInGroup(target_group_name) )
            {
                if (!visible_in_area)
				{
					EmitSignal(nameof(OnVisible));
					// GD.Print("Found target!");
				}
                visible_in_area = true;
            } else {
				var as_node = raycast.GetCollider() as Node3D;
				if (visible_in_area) 
				{
					// GD.Print($"Target lost within bounding box; Object obstucting view: {as_node.Name}");
					EmitSignal(nameof(OnNoLongerVisible));
				}
				visible_in_area = false;
			}
        } else {
			if (visible_in_area) 
			{
				// GD.Print("Target lost within bounding box; Max distance reached");
				EmitSignal(nameof(OnNoLongerVisible));
			}
			visible_in_area = false;
		}
    }

	private Node3D GetClosestTarget()
	{
		var targets = GetTree().GetNodesInGroup(target_group_name);
		Node3D closest = null;
		var distance = float.MaxValue;
		foreach (var t in targets)
		{
			if (closest == null) {
				closest = t as Node3D;
			} else if(t is Node3D node) {
				var delta = raycast.GlobalTransform.Origin - node.GlobalTransform.Origin;
				if (delta.Length() < distance)
				{
					closest = node;
					distance = delta.Length();
				}
			}
		}
		return closest;
	}


}
