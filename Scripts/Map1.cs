using Godot;
using System;

public partial class Map1 : Node3D
{
	CanvasLayer pausemenu;
	Area3D key;
	public override void _Ready()
	{
		key = GetNode<Area3D>("%Key");
		key.Connect("SignalPlayerHaveKey", new Callable(GetNode<Area3D>("%ChestAreaMap1"), "SetKeyTrue") );
		GetNode<Node3D>("Map").RemoveChild(key);

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

	public void addkey()
	{
		GetNode<Node3D>("Map").AddChild(key);
	}

}
