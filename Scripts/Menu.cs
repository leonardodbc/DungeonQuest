using Godot;
using System;

public partial class Menu : CanvasLayer
{
	Panel about;
	AnimationPlayer knight;
	public override void _Ready()
	{
		knight = GetNode<AnimationPlayer>("%AnimationPlayerkinght");

		about = GetNode<Panel>("%AboutPanel");
		about.Hide();
	}
	public override void _Process(double delta) // Knight idle animation
	{
		knight.Play("Idle");
	}
	public void _on_play_btn_pressed() // Play Button
	{
		GetTree().ChangeSceneToFile("res://world.tscn");
	}
	public void _on_about_btn_pressed() // About Button
	{
		about.Show();
	}
	public void _on_return_btn_pressed() // Return from About Button
	{
		about.Hide();
	}
	public void _on_quit_btn_pressed() // Quit Button
	{
		GetTree().Quit();
	}
}
