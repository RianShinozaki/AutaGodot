using Godot;
using System;

public partial class SimpleShake : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public float shakeX;
	[Export] public float shakeY;
	[Export] public float shakeScale;
	Vector2 origPos;
	Vector2 origScale;
	RandomNumberGenerator rand;
	public override void _Ready()
	{
		rand = new RandomNumberGenerator();
		origPos = GlobalPosition;
		origScale = GlobalScale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = origPos + new Vector2(rand.RandfRange(-shakeX, shakeX), rand.RandfRange(-shakeY, shakeY));
		float scaleShake = rand.RandfRange(-shakeScale, shakeScale);
		GlobalScale = origScale + new Vector2(scaleShake, scaleShake);
	}
}
