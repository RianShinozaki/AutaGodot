using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerOrbState : EntityState
{
	[Export] float speed;
	[Export] float initSpeed;
	[Export] float accel;
	[Export] float decel;
	[Export] float initYSpeed;
	[Export] float minBounceSpeed;

	[Export] float minOrbTime;
	[Export] float bounce;
	[Export] float gravity;
	[Export] float drag;
	[Export] float dragApplyMinThresh;
	[Export] float initialGravScale;
	[Export] float gravityMultWaitTime;
	[Export] float exitYSpeed;
	[Export] float exitMoveSpeed;
	float orbTime;
	Vector3 prevVelocity;

	Vector2 cachedInput;
	float cachedDirection;
	float cachedRot;

	public override void Start(Node entity) {
		PlayerController player = (PlayerController)entity;

		orbTime = 0;
		player.orb = true;
		player.collisionMode = Agent.CollisionMode.BOUNCE;

		player.animPlayer.Play("Orb");

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if(inputDir == Vector2.Zero) {
			inputDir = new Vector2(player.sprite.FlipH ? -1 : 1, 0);
		}
		player.horSpeed = inputDir.X * speed;
		player.vertSpeed = inputDir.Y * speed;
		if(player.vertSpeed == 0 && player.grounded) {
			player.vertSpeed = initYSpeed;
		}
		if(player.vertSpeed == 0 && !player.grounded) {
			player.vertSpeed = initYSpeed*0.3f;
		}
	}
	public override void Update(Node entity, Transform2D transform, double delta) {
		PlayerController player = (PlayerController)entity;

		float hor = Input.GetAxis("ui_left", "ui_right");

		if(player.grounded) player.canOrb = true;

		if(Mathf.Abs(hor) > 0.1f)
			player.AccelerateHor(accel * hor, speed * hor, true);
		else
			player.AccelerateHor(decel, 0);

		if(orbTime >= minOrbTime && !Input.IsActionPressed("Orb")) {
			player.SwitchState("Normal");
		}

		if(orbTime > gravityMultWaitTime) {
			player.gravity = gravity;
		} else {
			player.gravity = gravity * initialGravScale;
		}
		
		//Flip sprite based on direction if grounded  
		if(Mathf.Abs(player.horProj) > 0.1f && player.grounded) {
			player.sprite.FlipH = (player.horProj < 0) ? true : false;
		}

		orbTime += (float)delta;

	}
	public override void End(Node entity) {
		PlayerController player = (PlayerController)entity;
		player.orb = false;
		player.canOrb = false;
		player.collisionMode = Agent.CollisionMode.FLOOR;
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if(inputDir != Vector2.Zero) {
			int sign = Mathf.Sign(inputDir.X);
			player.horSpeed = sign * Mathf.Max(player.horSpeed * sign, exitMoveSpeed);

			sign = Mathf.Sign(inputDir.Y);
			if(sign == 0) sign = -1;
			player.vertSpeed = sign * Mathf.Max(player.vertSpeed * sign, exitMoveSpeed);
		}

		if(Mathf.Abs(player.horSpeed) > 0.1f && player.grounded) {
			player.sprite.FlipH = (player.horSpeed < 0) ? true : false;
		}
	}
	
}
