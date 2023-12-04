using Godot;
using System;

public partial class CameraControl : Node3D
{
	[Export]
	public int MouseSensitivity = 4;
	CharacterBody3D character;
	public override void _Ready()
	{

		Input.MouseMode = Input.MouseModeEnum.Captured;

		character = GetTree().GetNodesInGroup("playertest")[0] as CharacterBody3D;

	}
	public override void _Process(double delta)
	{
		
		GlobalPosition = character.GlobalPosition;

		GetNode<Camera3D>("%Camera").LookAt(character.GetNode<Node3D>("LookAt").GlobalPosition);

	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{

			float Xrotation = Mathf.Clamp(Rotation.X - mouseMotion.Relative.Y / 1000 * MouseSensitivity, -1f, -.05f);

			float Yrotation = Rotation.Y - mouseMotion.Relative.X / 1000 * MouseSensitivity;

			Rotation = new Vector3(Xrotation, Yrotation, 0);
			
		}
    }
}
