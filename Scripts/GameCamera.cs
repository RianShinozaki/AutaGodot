using Godot;
using System;

public partial class GameCamera : Camera2D
{
	[Export] public Node2D target;

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition , 0.2f);
	}
}
