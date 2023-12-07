using Godot;
using System;
using System.Data;

public partial class Lever : Area3D
{
	
	Label activelevertext;
	bool dentro = false;
	bool usou = false;
	AnimationPlayer animationlever;

	public override void _Ready()
	{
		activelevertext = GetNode<Label>("ActiveLever");
		RemoveChild(activelevertext);

		animationlever = GetNode<AnimationPlayer>("AnimationLever");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("action") && dentro && !usou)
		{
			animationlever.Play("LeverSwitch");
			GetNode<Node3D>("/root/Map1").Call("addkey");
			usou = true;
			activelevertext.Text = "";
		}
	}

	private void _on_body_entered(Node3D body)
	{
		if (body is Player) {
			dentro = true;
			AddChild(activelevertext);
		}
	}
	private void _on_body_exited(Node3D body)
	{
		if (body is Player) {
			dentro = false;
			RemoveChild(activelevertext);
		}
	}
}
