using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerAttackState : EntityState
{

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
	public float comboTimer = 0;
	[Export] public float comboTimeMax = 0.25f;
	public int comboNum = 0;
	enum JumpingState {
		Grounded,
		Rising,
		Peak,
		Falling
	}
	public override void Start(Node entity) {
		PlayerController player = (PlayerController)entity;
		var stateMachine = player.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Attack", true);
		stateMachine = player.anim.Get("parameters/Attack/playback").As<AnimationNodeStateMachinePlayback>();
		switch(comboNum) {
			case 0:
				stateMachine.Start("Attack_Combo1", true);
				//comboNum++;
				comboTimer = comboTimeMax;
				break;
			case 1:
				stateMachine.Start("Attack_Combo2", true);
				comboNum++;
				comboTimer = comboTimeMax;
				break;
			case 2:
				stateMachine.Start("Attack_Combo3", true);
				comboNum = 0;
				comboTimer = comboTimeMax;
				break;
		}
		base.Start(entity);
	}
	public override void Update(Node entity, Transform2D transform, double delta) {

		PlayerController player = (PlayerController)entity;
		float hor = Input.GetAxis("ui_left", "ui_right");

		if(player.grounded) player.canOrb = true;

		if(Mathf.Abs(hor) > 0.1f && !player.grounded) {
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
	}
    public override void PassiveUpdate(Node entity, Transform2D transform, double delta)
    {
		if(!active) {
        	if(comboTimer > 0) comboTimer -= (float)delta;
			else comboNum = 0;
		}
    }
    public override void End(Node entity) {
		base.End(entity);
	}
	public override void OnGrounded(Node entity, Vector2 normal, Vector2 velocity)
    {
    }
    public override void OnAnimationEnd(Node entity, string animName)
    {
		PlayerController player = (PlayerController)entity;
		player.SwitchState("Normal");
        base.OnAnimationEnd(entity, animName);
    }
	public override void OnHitboxEntered(Node entity, Hurtbox hurtB) {
		PlayerController player = (PlayerController)entity;
		player.DefaultHitboxEntered(hurtB);
	}
}
