using Godot;
using System;
using bridge;
using data;
using interaction;

public partial class player_character : CharacterBody3D
{

	[ExportGroup("Controls")]
	[Export] private float mouse_sensitivity = 0.003f;

	[ExportGroup("Movement")]
	[Export] private float speed = 5.0f;
	[Export] private float acceleration = 2.0f;
	[Export] private float deacceleration = 10.0f;


	[ExportGroup("Node Refs")]
	[Export] private NodePath cam_arm_path;
	private Node3D cam_arm;
	
	[Export] private NodePath anim_tree_path;
	private AnimationTree anim_tree;

	[Export] private NodePath raycast_path;
	private RayCast3D raycast;

	[Signal] public delegate void OnInteractionFoundEventHandler();
	[Signal] public delegate void OnInteractionLostEventHandler();


	public override void _Ready()
	{
		cam_arm = this.GetNodeCustom<Node3D>(cam_arm_path);
		anim_tree = this.GetNodeCustom<AnimationTree>(anim_tree_path);
		raycast = this.GetNodeCustom<RayCast3D>(raycast_path);
		
	}

    public override void _PhysicsProcess(double delta)
    {
		if (Input.MouseMode != Input.MouseModeEnum.Captured) return;

		var input_dir = Input.GetVector("move_left", "move_right", "move_back", "move_forward");
		var intent_vec = new Vector3();
		intent_vec += cam_arm.GlobalTransform.basis.x * input_dir.x;
		intent_vec += cam_arm.GlobalTransform.basis.z * input_dir.y * -1;
		intent_vec.y = 0f;

		bool actively_moving = intent_vec.LengthSquared() > 0.1f;
	
		if (!actively_moving) intent_vec = Vector3.Zero;
		else intent_vec = intent_vec.Normalized();
		anim_tree.Set("parameters/MovementCycle/blend_position", intent_vec.Length());

		var accel = Mathf.Lerp(deacceleration, acceleration, intent_vec.Dot(Velocity.Normalized()) * 0.5f + 0.5f);
		if (!actively_moving) accel = deacceleration;

		Velocity = Velocity.Lerp(intent_vec * speed, accel * (float)delta);
		if (!IsOnFloor())
		{
			var vel = Velocity;
			vel.y = -9.8f;
			Velocity = vel;
		} 
		MoveAndSlide();

		CheckInteractionRay();
    }

	private bool was_colliding_interactable = false;
	private void CheckInteractionRay()
	{
		var collider = raycast.GetCollider();
		bool flag = collider is not null and IInteractable && (collider as IInteractable).IsActive();
		if (flag != was_colliding_interactable)
		{
			if (flag) EmitSignal(nameof(OnInteractionFound));
			else EmitSignal(nameof(OnInteractionLost));
		}
		was_colliding_interactable = flag;
	}

    public override void _UnhandledInput(InputEvent e)
    {
		bool handled = false;
		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			handled = handled || InputMouseLook(e);
			handled = handled || InputInteract(e);

			if (e.IsActionPressed("shoot"))
			{
				anim_tree.Set("parameters/conditions/shoot", true);
				handled = true;
			}
			
		}

		if (e.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = (Input.MouseMode == Input.MouseModeEnum.Captured)? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
			handled = true;
		}

		if (handled) GetViewport().SetInputAsHandled();
    }

	private bool InputMouseLook(InputEvent e)
	{
		if (e is InputEventMouseMotion)
		{ 
			var mm = e as InputEventMouseMotion;
			var delta = mm.Relative * -1;
			RotateY(delta.x * mouse_sensitivity);

			var rot = cam_arm.Rotation;
			rot.x += delta.y * mouse_sensitivity;
			var cl = Mathf.DegToRad(89.0f);
			rot.x = Mathf.Clamp(rot.x, -cl, cl);
			cam_arm.Rotation = rot;
			return true;
		}
		return false;
	}

	private bool InputInteract(InputEvent e)
	{
		if (!e.IsActionPressed("interact")) return false;
		
		raycast.ForceRaycastUpdate();
        if (raycast.GetCollider() is Node collider && collider is IInteractable)
        { // TODO is this extra check necessary? Or could I just use the previous checks. Can this run out of sync with PhysicsProcess??? Probably
            (collider as IInteractable).TriggerInteraction();

        }
        return true;
	}

}
