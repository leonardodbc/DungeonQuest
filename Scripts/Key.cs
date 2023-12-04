using Godot;
using System;

public partial class Key : Area3D
{
	[Signal]
	public delegate void SignalPlayerHaveKeyEventHandler();

	public override void _Process(double delta)
	{
		Rotate(Vector3.Up, 0.01f);
	}

	private void _on_body_entered(Node3D body)
	{
		if (body is Player)
		{
			EmitSignal("SignalPlayerHaveKey");
			QueueFree();
		}
	}
}
