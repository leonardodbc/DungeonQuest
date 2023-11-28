using Godot;
using System;

public partial class World : Node3D
{
	CanvasLayer pausemenu;
	public override void _Ready()
	{
		pausemenu = GetNode<CanvasLayer>("%PauseMenu");
		pausemenu.Hide();
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("esc"))
		{
			GetTree().Paused = true;
			pausemenu.Show();
		}
	}
	public void _on_resume_btn_pressed()
	{
		GetTree().Paused = false;
		pausemenu.Hide();
	}
	public void _on_quit_menu_btn_pressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}
}
