using Godot;
using System;

public partial class dummy : CharacterBody2D
{
	[Export] private bool end;
	private void _on_timer_timeout()
	{
		if (end)
		{
			GetTree().ChangeSceneToFile("res://scenes/credits.tscn");
		}
		else
		{
			this.QueueFree();
		}
	}
}
