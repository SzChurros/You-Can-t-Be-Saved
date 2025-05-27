using Godot;
using System;
using System.IO;

public partial class player : CharacterBody2D
{
	[Export]
	private float Speed = 300.0f;
	[Export]
	private Sprite2D fakeray;
	[Export]
	private CollisionShape2D rotateable;
	[Export]
	private AnimatedSprite2D anim;
	[Export]
	private AudioStreamPlayer shotsfx;
	[Export]
	private AudioStreamPlayer stepsfx;
	[Export]
	private Sprite2D rifleroundpng;
	[Export]
	private RayCast2D realray;

	Byte frame = 0;

	bool playstepsfx = false;
	bool hasAmmo = true;

	private void _on_frametimer_timeout()
	{
		frame++;
		playstepsfx = true;
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("bullet"))
		{
			hasAmmo = true;
			area.QueueFree();
		}
		else
		{
			GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
		}
	}

	public override void _Ready()
	{
		fakeray.Visible = false;
	}
	
	private void _on_kill_timeout()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		rifleroundpng.Visible = hasAmmo;

		if (Input.IsKeyPressed(Key.Escape))
		{
			GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
		}

		if (Input.IsActionPressed("aim"))
		{
			fakeray.Visible = true;

			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / 15);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed / 15);

			if (Input.IsActionJustPressed("shoot") && hasAmmo)
			{
				frame = 1;
				shotsfx.Play();
				hasAmmo = false;

				GodotObject hit = realray.GetCollider();
				CharacterBody2D enem = hit as CharacterBody2D;

				if (enem != null)
				{
					if (enem.IsInGroup("1hitkill"))
					{
						enem.GetNode<AnimatedSprite2D>("rotateable/sprite").Frame = 1;
						enem.GetNode<Timer>("death").Start();
						enem.GetNode<Timer>("special").Start();
					}
					else if (enem.IsInGroup("2hitkill"))
					{
						enem.GetNode<Timer>("damage").Start();
					}
				}
			}
			else
			{
				frame = 0;
			}
			anim.Frame = frame;
			anim.Animation = "aim";
		}
		else
		{
			fakeray.Visible = false;

			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Y = direction.Y * Speed;

				if (frame > 7)
				{
					frame = 0;
				}
				anim.Frame = frame;
				anim.Animation = "walking";

				if (playstepsfx)
				{
					stepsfx.Play();

					playstepsfx = false;
				}
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);

				if (frame > 7)
				{
					frame = 0;
				}
				anim.Frame = frame;
				anim.Animation = "default";
			}
		}
		// Input.IsActionJustPressed("ui_accept")

		Velocity = velocity;
		MoveAndSlide();

		if (direction == Vector2.Zero)
		{
			return;
		}

		rotateable.Rotation = direction.Angle();
	}
}
