using Godot;
using System;

public partial class ammo : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.RotationDegrees = GD.Randf() * 360;
	}
}
