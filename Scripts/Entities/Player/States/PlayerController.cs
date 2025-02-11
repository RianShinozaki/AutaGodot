using Godot;
using System;

public partial class PlayerController : StateEntity
{
	//For animations
	public bool orb;
	public bool canOrb;

	public override void _Ready()
	{
		SwitchState("NormalState");
		base._Ready();
	}
}
