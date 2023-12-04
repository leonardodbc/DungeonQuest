using Godot;
using System;
public partial class Testplayer : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 9.5f;
	Vector3 lastLookAt = new Vector3();
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private bool doubleJump = false;
    public override void _Ready()
    {

    }
    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			doubleJump = true;
		}

		if (Input.IsActionJustPressed("jump") && doubleJump && !IsOnFloor())
		{
			doubleJump = false;
			velocity.Y = JumpVelocity;
		}

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
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;

		MoveAndSlide();
	}

	private Vector3 LerpVector3(Vector3 valueToLerp, Vector3 valueToLerpTo, float weight) => new Vector3(
		Mathf.Lerp(valueToLerp.X, valueToLerpTo.X, weight), Mathf.Lerp(valueToLerp.Y, valueToLerpTo.Y, weight), Mathf.Lerp(valueToLerp.Z, valueToLerpTo.Z, weight)
	);
}
