using Godot;
using System;

public partial class KinematicPlatform : AnimatableBody2D
{
	[Export] public Vector2 velocity;	
	public override void _Ready()
	{
	}

	public void CheckForAbovePassengers()
	{
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
