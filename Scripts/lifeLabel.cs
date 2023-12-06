using Godot;
using System;

public partial class lifeLabel : Label
{
  private Player player;
  public override void _Ready()
  {
    player = GetNode<Player>("%Knight");

  }
  public override void _Process(double delta)
  {
    Text = "vida: " + player.life.ToString();
  }

}