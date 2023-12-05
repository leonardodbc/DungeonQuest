using Godot;
using System;
using System.Threading;

public partial class SpikeArea : Area3D
{
	CharacterBody3D temp;
	public override void _Ready()
	{
		temp = GetNode<CharacterBody3D>("%Knight");
	}
	private void _on_body_entered(Node3D body)
	{
		if (body is Player)
		{
			temp.Call("death");
		}
	}

}
