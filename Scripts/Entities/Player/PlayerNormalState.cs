using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerNormalState : EntityState
{
	[Export] float jumpVelocity;
	[Export] float speed;
	[Export] float accel;
	[Export] float airAccel;
	[Export] float normalGravScale;
	[Export] float risingGravScale;
	[Export] float fallingGravScale;
	[Export] float shortHopGravScale;
	[Export] float riseThresh;
	[Export] float fallThresh;
	[Export] float fallSpeedMax;
	float afterImgTimer;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	enum JumpingState {
		Grounded,
		Rising,
		Peak,
		Falling
	}
	public override void Start(Node entity) {
		afterImgTimer = 0;
	}
	public override void Update(Node entity, Transform2D transform, double delta) {
		PlayerController player = (PlayerController)entity;
		float mass = player.Mass;
		Vector2 up = player.gravityUp;
		Vector2 vel = player.LinearVelocity;
		bool grounded = player.shapeCast.IsColliding();
		/*if(grounded) {
			float offsets = 100;
			for(int i = 0; i < player.shapeCast.GetCollisionCount(); i++) {
				float newOffset = player.shapeCast.GetCollisionPoint(i).Y - player.shapeCast.GlobalPosition.Y;
				if(newOffset < offsets) {
					offsets = newOffset;
				}
			}
			player.GlobalPosition = new Vector2(player.GlobalPosition.X, player.shapeCast.GetCollisionPoint(0).Y - 10);
		}*/

		player.floorNormal = new Vector2(1, 0);
		float angleUp = Mathf.RadToDeg(up.Angle()) + 90f;
		player.RotationDegrees = angleUp;

		//Grounded abilities
		if(grounded) {
			player.canOrb = true;
			player.floorNormal = player.shapeCast.GetCollisionNormal(0);

			//Jump
			if (Input.IsActionJustPressed("Jump")) {
				player.ApplyImpulse(up * jumpVelocity);
			}
		}

		//Execute ground movement
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_down", "ui_up");
		if(inputDir != cachedInput || Mathf.Abs(Mathf.AngleDifference(Mathf.DegToRad(angleUp), Mathf.DegToRad(cachedRot))) > Mathf.DegToRad(95f)) {
			cachedInput = inputDir;
			if(inputDir.Length() < 0.1f) cachedDirection = 0;
			else {
				Vector2 inputDirRot = inputDir.Rotated(Mathf.DegToRad(angleUp));
				if(Mathf.Abs(inputDirRot.X) > 0.5f)
					cachedDirection = Mathf.Sign(inputDirRot.X);
			}
		}
		cachedRot = angleUp;

		float thisAccel = player.grounded ? accel : airAccel;

		Vector2 hor = up.Rotated(Mathf.DegToRad(90));

		player.horProj = vel.Dot(hor);

		if(cachedDirection != 0) {
			player.PhysicsMaterialOverride.Friction = 0f;

			player.ApplyForce(cachedDirection * hor * (thisAccel * mass));
			if(player.horProj > speed) {
				player.ApplyForce(-2 * hor * (thisAccel * mass));
			}
			if(player.horProj < -speed) {
				player.ApplyForce(2 * hor * (thisAccel * mass));
			}
		} else {
			player.PhysicsMaterialOverride.Friction = 0.9f;
		}
		
		//if(!grounded) player.PhysicsMaterialOverride.Friction = 0;
		
		player.vertProj = vel.Dot(up);

		JumpingState js = 	grounded ? JumpingState.Grounded : 
							player.vertProj > riseThresh ? JumpingState.Rising :
							player.vertProj < fallThresh ? JumpingState.Falling :
							JumpingState.Peak;
		
		//Apply gravity
		switch(js) {
			case JumpingState.Grounded:
				player.GravityScale = normalGravScale;
				break;
			case JumpingState.Rising:
				player.GravityScale = risingGravScale;
				if(!Input.IsActionPressed("Jump"))
					player.GravityScale = shortHopGravScale;
				break;
			case JumpingState.Peak:
				player.GravityScale = normalGravScale;
				break;
			case JumpingState.Falling:
				player.GravityScale = fallingGravScale;
				break;
		}
		
		//Orb ability
		if(player.canOrb) {
			if (Input.IsActionJustPressed("Orb")) {
				player.SwitchState("Orb");
			}
		}

		//Flip sprite based on direction if grounded  
		if(Mathf.Abs(player.horProj) > 0.1f && player.grounded) {
			player.sprite.FlipH = (player.horProj < 0) ? true : false;
		}

		player.grounded = grounded;
	}
	public override void End(Node entity) {
	}
	
}
