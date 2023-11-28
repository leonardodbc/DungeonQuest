using Godot;
using System;

public partial class FlagArea : Area3D
{
	CharacterBody3D temp;
	Label savetext;
	bool dentro = false;
	public override void _Ready()
	{
		savetext = GetNode<Label>("SaveText");
		RemoveChild(savetext);

		temp = GetNode<CharacterBody3D>("%Knight");	
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("save") && dentro)
		{
			((Player)temp).respawn = GlobalPosition;
		}
	}
	private void _on_body_entered(Node3D body)
	{
		if (body is Player) {
			dentro = true;
			AddChild(savetext);
		}
	}
	private void _on_body_exited(Node3D body)
	{
		if (body is Player) {
			dentro = false;
			RemoveChild(savetext);
		}
	}
}