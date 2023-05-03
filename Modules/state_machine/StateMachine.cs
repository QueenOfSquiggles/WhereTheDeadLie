namespace state_machine;

using bridge;
using data;
using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	struct DebugTargets
	{
		public Node3D destination;
		public Node3D waypoint;
		public Node3D direction;

	}

	private DebugTargets debug_targets = new();

	[Export(PropertyHint.Range, "0,100")] private int starting_index = 0;

	private readonly List<State> states = new();
	private int index = 0;
	[Export] private NodePath path_char_body;

	[Export] private NodePath path_nav_agent;

	[Export] private NodePath path_anim;

	[Export] private Curve movement_speed_curve;

	public State.StateData data;

	public List<Marker3D> patrol_points = new();

	[Export] public int CurrentIndex {
		get {
			return index;
		}
		set {}
	}

    public override void _Ready()
    {
		foreach (var node in GetChildren())
		{
			if(node is State state) states.Add(state);
		}
		index = starting_index;

		data.char_body = this.GetNodeCustom<CharacterBody3D>(path_char_body);
		data.nav_agent = this.GetNodeCustom<NavigationAgent3D>(path_nav_agent);
		data.anim_tree = this.GetNodeCustom<AnimationTree>(path_anim);
		data.machine = this;
		var curve_pos = GameDataManager.instance.GameAggression / 10.0f;
		data.move_speed = movement_speed_curve.SampleBaked(curve_pos);
		debug_targets.destination = this.GetNodeCustom<Node3D>("../CurrentTargetLocation");
		debug_targets.waypoint = this.GetNodeCustom<Node3D>("../CurrentWaypoint");
		debug_targets.direction = this.GetNodeCustom<Node3D>("../CurrentIntentDirection");
		debug_targets.destination.TopLevel = true;
		debug_targets.waypoint.TopLevel = true;
		debug_targets.direction.TopLevel = true;
    }

    public override void _PhysicsProcess(double delta)
    {
		var n_index = states[index].StateMachineTick(data, (float)delta);		
		if (n_index != -1) ForceNewState(n_index);
		MoveToCurrentTarget((float)delta);
		data.char_body.Velocity += Vector3.Down * 9.8f * (float)delta;
		data.char_body.MoveAndSlide();
    }

	private void MoveToCurrentTarget(float delta)
	{

		// if (data.nav_agent.IsNavigationFinished()) return;

		Vector3 target = data.nav_agent.GetNextPathPosition();
		if (target == data.char_body.GlobalPosition) return; // no path yet

		data.char_body.LookAt(target, Vector3.Up);
		var rot = data.char_body.GlobalRotation;
		rot.X = 0f;
		rot.Z = 0f;
		data.char_body.GlobalRotation = rot;

		var delta_motion = data.anim_tree.GetRootMotionPosition() / delta;
		var localized_motion = data.char_body.GlobalTransform.Basis.Z * delta_motion.Z;
		data.char_body.Velocity = data.char_body.Velocity.MoveToward(localized_motion * data.move_speed, 0.25f);
		UpdateDebugMarkers(data.nav_agent.TargetPosition, target, data.char_body.GlobalPosition + data.char_body.Velocity);
	}

	private void UpdateDebugMarkers(Vector3 destination, Vector3 waypoint, Vector3 direction)
	{
		debug_targets.destination.GlobalPosition = destination;
		debug_targets.waypoint.GlobalPosition = waypoint + (Vector3.Up * 0.5f);
		debug_targets.direction.GlobalPosition = direction;
	}

	public void ForceNewState(int n_index)
	{
		states[index].OnExitState(data);
		index = n_index;
		states[index].OnEnterState(data);
		// anim.Play(states[index].GetStateAnimation());
		var anim_machine = (AnimationNodeStateMachinePlayback)data.anim_tree.Get("parameters/playback");
		anim_machine.Travel(states[index].GetStateAnimation());
		// GD.Print($"Active State: {states[index].Name}");
	}
}
