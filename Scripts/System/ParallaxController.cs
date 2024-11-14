using Godot;
using System;

public partial class ParallaxController : Node2D
{
	[Export] public Camera2D cam;
	[Export] public float ScrollScale;
	public Vector2 origPosition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = GetViewport().GetCamera2D();
		origPosition = GlobalPosition*ScrollScale;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = origPosition + cam.GlobalPosition * (1 - ScrollScale);
		GD.Print(GlobalPosition);
	}
}