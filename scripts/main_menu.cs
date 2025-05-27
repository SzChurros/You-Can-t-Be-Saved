using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class main_menu : Node2D
{
	[Export]
	private Label title;
	[Export]
	private Timer timer;
	[Export]
	private AudioStreamPlayer quitbutton;
	[Export]
	private AudioStreamPlayer playbutton;
	[Export]
	private AudioStreamPlayer credbutton;

	string[] titletext = new string[4];

	Byte titlenum = 0;

	private void _on_timer_timeout()
	{
		title.Text = titletext[titlenum];
		titlenum++;

		if (titlenum == 4)
		{
			titlenum = 0;
		}
	}

	private void _on_button_3_pressed()
	{
		credbutton.Play();
	}


	private void _on_sfx_3_finished()
	{
		GetTree().ChangeSceneToFile("res://scenes/credits.tscn");
	}
	
	private void _on_button_2_pressed()
	{
		playbutton.Play();
	}


	private void _on_sfx_2_finished()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level0.tscn");
	}
	
	private void _on_sfx_finished()
	{
		GetTree().Quit();
	}
	
	private void _on_button_pressed()
	{
		quitbutton.Play();
	}

	public override void _Ready()
	{

		titletext[0] = "You Can't Be\nSaved";
		titletext[1] = "You Can't Be\nSaved.";
		titletext[2] = "You Can't Be\nSaved..";
		titletext[3] = "You Can't Be\nSaved...";

		timer.Start();
	}
}
