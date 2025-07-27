using Godot;
using System;
using System.Collections;

public partial class OrbBreakable : Area2D
{
	bool broken = false;
	AnimationPlayer anim;
	float bounceSpeed = 160;

	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		AreaEntered += _on_area_entered;
	}

	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area)
	{
		if (!broken)
		{
			broken = true;
			anim.Play("Burst");
			if (area.CollisionLayer == 32768) //It's the player orb
			{
				float totalSpeed = bounceSpeed;
				float angle = Mathf.Atan2(area.GlobalPosition.Y - GlobalPosition.Y, area.GlobalPosition.X - GlobalPosition.X);

				//PlayerController.Instance.SetHor(totalSpeed * Mathf.Cos(angle));
				//PlayerController.Instance.SetVert(totalSpeed * Mathf.Sin(angle));
				PlayerController.Instance.SetVert(-bounceSpeed);
			}
				
		}
	}

	public void Burst()
	{
		foreach (Node node in GetNode<Node2D>("Shards").GetChildren())
		{
			StarPetal bp = (StarPetal)node;
			bp.OnSpawn();
		}
	}
}
