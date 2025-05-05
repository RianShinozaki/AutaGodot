using Godot;
using System;

public enum CamMode {
	LockY,
	Free
}
public partial class CameraCenter : RigidBody2D
{
	[Export] public Vector2 deadzone;
	float playerYCache;
	CamMode currentCamMode = CamMode.LockY;

	public override void _PhysicsProcess(double delta)
	{	
		PlayerController player = PlayerController.Instance;
		Vector2 velocity = Vector2.Zero;	
		velocity.X = (player.GlobalPosition.X - GlobalPosition.X)*3;
		velocity.Y = (playerYCache - GlobalPosition.Y)*2;
		if(player.grounded) {
			playerYCache = player.GlobalPosition.Y;
			currentCamMode = CamMode.LockY;
		}
		LinearVelocity = velocity;
	}
}
