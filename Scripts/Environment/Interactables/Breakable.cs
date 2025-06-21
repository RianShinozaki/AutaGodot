using Godot;
using System;
using System.Collections;

public partial class Breakable : Area2D
{
	bool broken = false;
	public override void _Ready()
	{
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

			foreach (Node node in GetNode<Node2D>("Shards").GetChildren())
			{
				BouncingParticle bp = (BouncingParticle)node;
				bp.OnSpawn();
			}
		}
	}
}
