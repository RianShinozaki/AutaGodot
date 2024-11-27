using Godot;
using System;

public partial class Agent : CharacterBody2D
{
	public enum CollisionMode {
		FLOOR,
		FREE,
		BOUNCE
	}

	// Set Variables
	[Export] int GroundedFloorCheckDist = 24;
	[Export] int AirFloorCheckDist = 16;
	[Export] int floorOffset = 16;
	[Export] public float slopeLeaveInfluence; //How much momentum is kept when running off a slope
	[Export] public float slopeJumpInfluence; //How much momentum is kept when jumping off a slope
	[Export] public float slopeLandInfluence; //How much momentum is added to horizontal speed when landing on a slope
	[Export] public float gravity;
	[Export] public CollisionMode collisionMode;

	[Export] public float bounceFactor = 1;

	// Member Variables
	RayArray2D rayArray;
	float xRemainder = 0;
	float yRemainder = 0;
	public bool grounded;
	public bool previouslyGrounded;
	public float horSpeed;
	public float vertSpeed;
	public float lerpRate = 1f;
	public Vector2 lastFloorNormal = Vector2.Up;


	public override void _Ready()
	{
		rayArray = GetNode<RayArray2D>("RayArray");
	}

	public void Move(float x, float y, float delta, bool writeback = false) {		
		if(grounded) {
			Vector2 moveDir = lastFloorNormal.Rotated(Mathf.DegToRad(90f));
			Velocity = new Vector2(x * moveDir.X, x * moveDir.Y);
		}
		else {
			Velocity = new Vector2(x, y);
		}

		if(collisionMode == CollisionMode.FLOOR) {
			MoveAndSlide();
			if(writeback) {
				horSpeed = Velocity.X;
				if(!grounded)
					vertSpeed = Velocity.Y;
			}
		}
		else if (collisionMode == CollisionMode.BOUNCE) {
			Vector2 vel = Velocity;
			KinematicCollision2D kc = MoveAndCollide(vel * delta, true);
			if(kc != null) {
				vel = -vel.Reflect(kc.GetNormal());
				vel.Y += gravity * (float)delta;

                Vector2 lossFactor = new Vector2( Mathf.Abs(kc.GetNormal().X), Mathf.Abs(kc.GetNormal().Y)) * (1-bounceFactor);
                GD.Print(lossFactor);
                vel.X -= lossFactor.X * vel.X;
                vel.Y -= lossFactor.Y * vel.Y;
            }
			if(writeback)
				horSpeed = vel.X;
				vertSpeed = vel.Y;

            MoveAndSlide();

			//MoveAndCollide(new Vector2(horSpeed, vertSpeed) * delta);
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
                    OnGrounded(lastFloorNormal, new Vector2(horSpeed, vertSpeed));
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


		Move(horSpeed, vertSpeed, (float)delta, true);
		
	}
	public virtual void OnBounce(Vector2 normal) {}
	public virtual void OnGrounded(Vector2 normal, Vector2 velocity) {}
}
