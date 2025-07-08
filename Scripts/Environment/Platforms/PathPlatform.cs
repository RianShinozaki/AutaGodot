using Godot;
using System;

public partial class PathPlatform : Node2D
{
	PathFollow2D pathFollow;
	CharacterBody2D platform;
	[Export] public float moveSpeedpxps;
	public Godot.Collections.Array<StateEntity> entitiesAbove = new Godot.Collections.Array<StateEntity>();
	
	
	public override void _Ready()
	{
		pathFollow = GetNode<PathFollow2D>("Path2D/PathFollow2D");
		platform = GetNode<CharacterBody2D>("Platform");
	}

	public override void _Process(double delta)
	{
		pathFollow.SetProgress(pathFollow.GetProgress() + moveSpeedpxps * (float)delta * 60);
		//Vector2 platDelta = pathFollow.GlobalPosition - platform.GlobalPosition;
		//platform.Velocity = platDelta * (1/(float)delta);
		//platform.MoveAndSlide();
		
	}
}
