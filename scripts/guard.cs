using Godot;
using System;
using System.IO;

public partial class guard : CharacterBody2D
{
	[Export]
	private float Speed = 200.0f;
	[Export]
	private CollisionShape2D rotateable;
	[Export]
	private AudioStreamPlayer stepsfx;
	[Export]
	private RayCast2D realray;
	[Export]
	private Timer weapontimer;

	Vector2 direction = Vector2.Zero;
	Vector2 directionnorm = Vector2.Zero;

	bool playstepsfx = false;

	[Export]
	public bool alive = true;

	CharacterBody2D Player = null;

	private void _on_area_2d_area_entered(Area2D area)
	{
		Node areaParent = area.GetParent();
		if (areaParent == null)
		{
			return;
		}
		if (areaParent.IsInGroup("player"))
		{
			Player = areaParent as CharacterBody2D;
		}
	}

	private void _on_area_2d_area_exited(Area2D area)
	{
		Node areaParent = area.GetParent();
		if (areaParent == null)
		{
			return;
		}
		if (areaParent.IsInGroup("player"))
		{
			Player = null;
		}
	}

	private void _on_stepsfx_timeout()
	{
		playstepsfx = true;
	}
	
	private void _on_death_timeout()
	{
		this.QueueFree();
	}

	private void _on_special_timeout()
	{
		alive = false;
	}

	private void _on_timer_timeout()
	{
		GodotObject hit = realray.GetCollider();
		CharacterBody2D enem = hit as CharacterBody2D;

		if (enem != null)
		{
			if (enem.IsInGroup("player"))
			{
				enem.GetNode<Timer>("kill").Start();
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Player != null && alive)
		{
			directionnorm = direction.Normalized();

			direction = Player.Position - this.Position;

			velocity.X = directionnorm.X * Speed;
			velocity.Y = directionnorm.Y * Speed;

			GodotObject hit = realray.GetCollider();
			CharacterBody2D enem = hit as CharacterBody2D;

			if (enem != null)
			{
				if (enem.IsInGroup("player") && weapontimer.TimeLeft == 0)
				{
					weapontimer.Start();
				}
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / 15);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed / 15);
		}

		Velocity = velocity;
		MoveAndSlide();

		if (direction == Vector2.Zero)
		{
			return;
		}

		rotateable.Rotation = direction.Angle();
	}
}
