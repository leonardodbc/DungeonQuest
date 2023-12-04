using Godot;
using System;

public partial class CoinCounter : Label
{
	private int Coins;
	public override void _Ready()
	{
		Coins = 0;
	}
	public override void _Process(double delta)
	{
		Text = Coins.ToString();
	}
	public void IncreaseCount()
	{
		Coins += 1;
	}
	public void ChestOpened()
	{
		Coins += 10;
	}
	public void ResetCounter()
	{
		Coins = 0;
	}
}