using Godot;
using System;
using System.Collections;

public partial class Breakable : Area2D
{
	bool broken = false;
	AudioStreamPlayer2D audio;
	public override void _Ready()
	{
		AreaEntered += _on_area_entered;
		audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
	}

	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area)
	{
		if (!broken)
		{
			broken = true;
			GetNode<AnimatedSprite2D>("BreakableLamp").Visible = false;
			GetNode<PointLight2D>("PointLight2D").Visible = false;
			RandomNumberGenerator rand = new RandomNumberGenerator();
			audio.PitchScale = rand.RandfRange(0.8f, 1.2f);
			audio.Play();
			foreach (Node node in GetNode<Node2D>("Shards").GetChildren())
			{
				BouncingParticle bp = (BouncingParticle)node;
				bp.OnSpawn();
			}
		}
	}
}
