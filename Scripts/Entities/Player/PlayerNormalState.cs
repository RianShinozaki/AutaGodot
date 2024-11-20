using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerNormalState : EntityState
{
	[Export] float jumpVelocity;
	[Export] float speed;
	[Export] float accel;
	[Export] float decel;
	[Export] float decelTooFast;
	[Export] float airAccel;
	[Export] float airDecel;
	[Export] float airDecelTooFast;
	[Export] float gravity;
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
		float hor = Input.GetAxis("ui_left", "ui_right");

		if(player.grounded) player.canOrb = true;

		if(Mathf.Abs(hor) > 0.1f) {
			int sign = Mathf.Sign(hor);
			if(player.horSpeed * hor < speed)
				player.AccelerateHor((player.grounded ? accel : airAccel) * hor, speed * hor, true);
			else {
				player.AccelerateHor((player.grounded ? decelTooFast : airDecelTooFast) * hor, speed * hor, true);
			}
		}
		else {
			player.AccelerateHor(player.grounded ? decel : airDecel, 0);
		}
		
		if(Input.IsActionJustPressed("Jump") && player.grounded) {
			player.vertSpeed = -jumpVelocity;
		}
		if(Input.IsActionJustPressed("Orb") && player.canOrb) {
			player.SwitchState("Orb");
			return;
		}

		player.vertProj = -player.vertSpeed;
		player.horProj = player.horSpeed;

		JumpingState js = 	player.grounded ? JumpingState.Grounded : 
							player.vertProj > riseThresh ? JumpingState.Rising :
							player.vertProj < fallThresh ? JumpingState.Falling :
							JumpingState.Peak;
		

		//Apply gravity
		switch(js) {
			case JumpingState.Grounded:
				player.gravity = gravity;
				break;
			case JumpingState.Rising:
				player.gravity = gravity * risingGravScale;
				if(!Input.IsActionPressed("Jump"))
					player.gravity = gravity * shortHopGravScale;
				break;
			case JumpingState.Peak:
				player.gravity = gravity * normalGravScale;
				break;
			case JumpingState.Falling:
				player.gravity = gravity * fallingGravScale;
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
			bool toFlipH = (player.horProj < 0) ? true : false;
			if(toFlipH != player.sprite.FlipH) {
				var stateMachine = player.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("Turn", true);
			}
			player.sprite.FlipH = toFlipH;
		}

	}
	public override void End(Node entity) {
	}
	public override void OnGrounded(Node entity, Vector2 normal, Vector2 velocity)
    {
		PlayerController player = (PlayerController)entity;
        var stateMachine = player.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Land", true);
    }
	
}
