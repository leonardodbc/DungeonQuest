using Godot;
using System;
using System.Threading;

public partial class SpikeArea : Area3D
{	
	private void _on_body_entered(Node3D body)
	{
		if (body is Player)
		{
			body.Call("death");
			GetNode<Label>("%HUD/LabelCoin").Call("ResetCounter");
		}
	}

}
