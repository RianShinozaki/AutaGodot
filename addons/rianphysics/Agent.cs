using Godot;
using System;

public partial class Agent : CharacterBody2D
{
	public enum CollisionMode {
		FLOOR,
		FREE,
		BOUNCE
	}

	[Export] int GroundedFloorCheckDist = 24;
	[Export] int AirFloorCheckDist = 16;
	[Export] int floorOffset = 16;
	[Export] public float slopeLeaveInfluence; //How much momentum is kept when running off a slope
	[Export] public float slopeJumpInfluence; //How much momentum is kept when jumping off a slope
	[Export] public float slopeLandInfluence; //How much momentum is added to horizontal speed when landing on a slope
	RayArray2D rayArray;
	float xRemainder = 0;
	float yRemainder = 0;
	public bool grounded;
	public bool previouslyGrounded;
	[Export] public float gravity;
	public float horSpeed;
	public float vertSpeed;
	public CollisionMode collisionMode;
	public float lerpRate = 1f;
	public Vector2 lastFloorNormal = Vector2.Up;
	[Export] public float bounceFactor = 1;


	public override void _Ready()
	{
		rayArray = GetNode<RayArray2D>("RayArray");
	}

	public void Move(float x, float y, float delta) {		
		if(grounded) {
			Vector2 moveDir = lastFloorNormal.Rotated(Mathf.DegToRad(90f));
			Velocity = new Vector2(x * moveDir.X, x * moveDir.Y);
		}
		else {
			Velocity = new Vector2(x, y);
		}

		if(collisionMode == CollisionMode.FLOOR) {
			MoveAndSlide();
			horSpeed = Velocity.X;
			if(!grounded)
				vertSpeed = Velocity.Y;
		}
		else if (collisionMode == CollisionMode.BOUNCE) {
			Vector2 vel = new Vector2(horSpeed, vertSpeed);
			KinematicCollision2D kc = MoveAndCollide(vel * delta, true);
			if(kc != null) {
				vel = -vel.Reflect(kc.GetNormal()) * bounceFactor;

				vel.Y += gravity * (float)delta;
			}
			horSpeed = vel.X;
			vertSpeed = vel.Y;
			MoveAndCollide(new Vector2(horSpeed, vertSpeed) * delta);
		}
		previouslyGrounded = grounded;
	}
	public void AccelerateHor(float x, float limit = int.MaxValue, bool signCorrect = false) {
		horSpeed = Mathf.MoveToward(horSpeed , limit, x * (signCorrect ? Mathf.Sign(limit) : 1));
	}
	public void AccelerateVert(float y, float limit = int.MaxValue, bool signCorrect = false) {
		horSpeed = Mathf.MoveToward(vertSpeed , limit, y * (signCorrect ? Mathf.Sign(limit) : 1));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		if(collisionMode == CollisionMode.FLOOR) {
			if(rayArray.IsColliding() && vertSpeed >= 0) {
				if(!grounded) {
					horSpeed += vertSpeed * lastFloorNormal.Rotated(Mathf.DegToRad(90f)).Y * slopeLandInfluence;
				}
				GlobalPosition = GlobalPosition.Lerp(new Vector2(GlobalPosition.X, rayArray.GetCollisionPoint().Y - floorOffset),lerpRate);
				lerpRate = Mathf.MoveToward(lerpRate, 1, (float)delta * 10f);
				grounded = true;
				rayArray.TargetPosition = new Vector2(0, GroundedFloorCheckDist);
				vertSpeed = 0;
				lastFloorNormal = rayArray.GetCollisionNormal();
			}
			else {
				if(grounded == true) {
					vertSpeed += horSpeed * lastFloorNormal.Rotated(Mathf.DegToRad(90f)).Y * slopeLeaveInfluence;
				}
				grounded = false;
				rayArray.TargetPosition = new Vector2(0, AirFloorCheckDist);
				vertSpeed += gravity * (float)delta;
				lerpRate = 0.5f;
			}
		} else {
			grounded = false;
			vertSpeed += gravity * (float)delta;
		}
		Move(horSpeed, vertSpeed, (float)delta);
		
	}
	public virtual void OnBounce(Vector2 normal) {}
}