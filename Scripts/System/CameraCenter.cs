using Godot;
using System;

public partial class CameraCenter : RigidBody2D
{
	[Export] public Vector2 deadzone;

	public override void _PhysicsProcess(double delta)
	{	
		PlayerController player = PlayerController.Instance;
		Vector2 velocity = Vector2.Zero;	
		velocity.X = (player.GlobalPosition.X - GlobalPosition.X)*3;
		if(player.grounded) {
			velocity.Y = (player.GlobalPosition.Y - GlobalPosition.Y)*2;
		}
		LinearVelocity = velocity;
	}
}
