using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float Acceleration = 4.0f;
	public const float JumpVelocity = 9.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 2f * ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	[Export] public float MouseSensitivity = 0.0016f;
	private AnimationTree animations;
	private AnimationNodeStateMachinePlayback stateMachine;
	private bool canJump = false;
	private bool jumping = true;
	private bool lastFloor = true;
	private bool sprinting = false;
	public Vector3 respawn;
	private float _rotationX = 0f;
	Vector3 lastLookAt = new Vector3();
	public override void _Ready()
	{

		respawn = new Vector3(0, 0, 0);

		animations = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animations.Get("parameters/playback");

	}


	private Vector3 LerpVector3(Vector3 valueToLerp, Vector3 valueToLerpTo, float weight) => new Vector3(
	Mathf.Lerp(valueToLerp.X, valueToLerpTo.X, weight), Mathf.Lerp(valueToLerp.Y, valueToLerpTo.Y, weight), Mathf.Lerp(valueToLerp.Z, valueToLerpTo.Z, weight)
);
	public override void _PhysicsProcess(double delta)
	{

		if (GlobalPosition.Y < -10)
		{

			GlobalPosition = respawn;
			GetNode<Label>("%LabelCoin").Call("ResetCounter");

		}
		Vector3 velocity = Velocity;


		CameraControl cameraControl = GetTree().GetNodesInGroup("CameraControl")[0] as CameraControl;
		Vector3 lookAt = cameraControl.GetNode<Node3D>("LookAt").GlobalPosition;
		lookAt = new Vector3(lookAt.X, GlobalPosition.Y, lookAt.Z);

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			Vector3 lerpDirection = LerpVector3(lastLookAt, lookAt, 0.075f);
			LookAt(lerpDirection);
			lastLookAt = lerpDirection;

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}


		velocity.Y -= gravity * (float)delta;
		velocity = GetMoveInput(delta);
		velocity = HandleJump(velocity, delta);
		Velocity = velocity;
		MoveAndSlide();
	}

	private Vector3 GetMoveInput(double delta)
	{
		Vector3 velocity = Velocity;
		float vy = Velocity.Y;
		velocity.Y = 0;

		Vector2 input = Input.GetVector("left", "right", "forward", "backward");
		Vector3 dir = new Vector3(input.X, 0, input.Y).Rotated(Vector3.Up, Rotation.Y);

		if (Input.IsActionPressed("sprint"))
		{
			sprinting = true;
			if (sprinting && IsOnFloor())
			{
				velocity = velocity.Lerp(dir * Speed * 2, +Acceleration * (float)delta);
			}
		}
		else if (Input.IsActionJustReleased("sprint"))
		{
			sprinting = false;
		}

		if (Input.IsActionJustPressed("dodge"))
		{
			int i = 0;
			while (i < 2)
			{
				velocity += velocity.Lerp(dir * Speed * 4, +Acceleration * (float)delta);
				i++;
			}
		}


		velocity = velocity.Lerp(dir * Speed, Acceleration * (float)delta);
		HandleAnimation(velocity);

		velocity.Y = vy;
		return velocity;
	}

	private Vector3 HandleJump(Vector3 velocity, double delta)
	{
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			canJump = true;
		}

		if (Input.IsActionJustPressed("jump") && canJump && !IsOnFloor())
		{
			canJump = false;
			velocity.Y = JumpVelocity;
			stateMachine.Travel("Jump_Start");
			animations.Set("parameters/conditions/grounded", false);

		}

		return velocity;
	}

	public override void _Input(InputEvent @event) // Câmera fiz
	{
		if (Input.IsActionJustPressed("esc"))
			Input.MouseMode = Input.MouseModeEnum.Visible;

		if (!Input.IsMouseButtonPressed(MouseButton.Right))
			return;

		if (@event is InputEventMouseMotion mouseMotion)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;

			_rotationX += mouseMotion.Relative.X * MouseSensitivity;

			Transform3D transform = Transform;
			transform.Basis = Basis.Identity;
			Transform = transform;

			var rotation = Rotation;
			rotation.Y -= mouseMotion.Relative.X * MouseSensitivity;
			Rotation = rotation;

			Rotate(Vector3.Up, _rotationX * -1);
		}
	}

	private void HandleAnimation(Vector3 velocity)
	{
		var vl = velocity * Transform.Basis;
		animations.Set("parameters/IWR/blend_position", new Vector2(vl.X, -vl.Z) / Speed);
		HandleJumpAnimation();
	}

	private void HandleJumpAnimation()
	{
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			jumping = true;
			animations.Set("parameters/conditions/grounded", false);
		}

		animations.Set("parameters/conditions/jumping", jumping);

		if (IsOnFloor() && !lastFloor)
		{
			jumping = false;
			animations.Set("parameters/conditions/grounded", true);
		}

		lastFloor = IsOnFloor();

		if (!IsOnFloor() && !jumping)
		{
			stateMachine.Travel("Jump_Idle");
			animations.Set("parameters/conditions/grounded", false);
		}
	}
}
