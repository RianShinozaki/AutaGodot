using Godot;
using System;

public partial class PlayerController : StateEntity
{
	public AnimationTree anim;
	public AnimationPlayer animPlayer;
	public Sprite2D sprite;
	public bool orb;
	public bool canOrb;
	public override void _Ready()
	{
		anim = GetNode<AnimationTree>("Art/AnimationTree");
		animPlayer = GetNode<AnimationPlayer>("Art/AnimationPlayer");
		sprite = GetNode<Sprite2D>("Art");
		SwitchState("Normal");
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
