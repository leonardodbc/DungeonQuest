using Godot;
using System;

public partial class ChestArea : Area3D
{

	AnimationPlayer animationchest;
	Label chesttext;
	bool dentro = false;
	bool havekey = false;
	bool abriu = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		animationchest = GetNode<AnimationPlayer>("%AnimationChest");

		chesttext = GetNode<Label>("ChestText");
		chesttext.Hide();

		GetNode<CpuParticles3D>("Particles").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("action") && dentro && havekey && !abriu)
		{
			animationchest.Play("lid_open");
			GetNode<CpuParticles3D>("Particles").Show();
			abriu = true;
			havekey = false;
			chesttext.Text = "";
		}
		
	}

	public void SetKeyTrue()
	{
		havekey = true;
	}

	private void _on_body_entered(Node3D body)
	{
		if (body is Player) {
			dentro = true;
			if (!havekey && !abriu)
			{
				chesttext.Text = "Você não tem uma chave!";
			}
			if (havekey)
			{
				chesttext.Text = "Pressione E para abrir o baú!";
			}
			chesttext.Show();
		}
	}

	private void _on_body_exited(Node3D body)
	{
		if (body is Player)
		{
			chesttext.Hide();
		}
	}
}
