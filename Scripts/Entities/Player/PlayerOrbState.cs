using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerOrbState : EntityState
{
	[Export] float speed;
	[Export] float initSpeed;
	[Export] float accel;
	[Export] float initYSpeed;
	[Export] float minBounceSpeed;

	[Export] float minOrbTime;
	[Export] float bounce;
	[Export] float normalGravScale;
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

		/*AfterImage afterImg = ObjectPool.Instance.Spawn("DisintegratedImage") as AfterImage;
		if(afterImg != null) {
			afterImg.Texture = player.sprite.Texture;
			afterImg.Hframes = player.sprite.Hframes;
			afterImg.Vframes = player.sprite.Vframes;
			afterImg.FlipH = player.sprite.FlipH;
			afterImg.Frame = player.sprite.Frame;
			afterImg.GlobalPosition = player.sprite.GlobalPosition;

			ShaderMaterial mat = afterImg.MaterialOverride as ShaderMaterial;
			mat.SetShaderParameter("Frame", player.sprite.Frame);
			mat.SetShaderParameter("VFrame", player.sprite.Vframes);
			mat.SetShaderParameter("HFrame", player.sprite.Hframes);
		}*/
			
		player.orb = true;
		player.canOrb = false;

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (inputDir != Vector2.Zero)
		{
			player.LinearVelocity = new Vector2(inputDir.X, inputDir.Y) * initSpeed;
		}
		else
		{
			player.LinearVelocity = new Vector2(initSpeed * (player.sprite.FlipH ? -1 : 1), 0);
		}
		if(Mathf.Abs(player.LinearVelocity.Y) < 5f) {
			player.LinearVelocity += Vector2.Up*initYSpeed;
		}

		player.PhysicsMaterialOverride.Bounce = bounce;
		//player.StartCreatingAfterImgs();

		//GameCamera.Instance.Freeze(0.2f);
		player.animPlayer.Play("Orb");
	}
	public override void Update(Node entity, Transform2D transform, double delta) {
		PlayerController player = (PlayerController)entity;
		float mass = player.Mass;
		Vector2 up = player.gravityUp;
		Vector2 vel = player.LinearVelocity;

		float angleUp = Mathf.RadToDeg(up.Angle()) + 90f;
		player.RotationDegrees = angleUp;

		orbTime += (float)delta;

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
		Vector2 hor = up.Rotated(Mathf.DegToRad(90));
		player.horProj = vel.Dot(hor);

		if(cachedDirection != 0) {

			player.ApplyForce(cachedDirection * hor * (accel * mass));
			if(player.horProj > speed) {
				player.ApplyForce(-2 * hor * (accel * mass));
			}
			if(player.horProj < -speed) {
				player.ApplyForce(2 * hor * (accel * mass));
			}
		}
		
		if(orbTime < gravityMultWaitTime)
			player.GravityScale = initialGravScale;
		else
			player.GravityScale = normalGravScale;
		

		if(orbTime >= minOrbTime && !Input.IsActionPressed("Orb")) {
			player.SwitchState("Normal");
		}
		
		//Flip sprite based on direction if grounded  
		if(Mathf.Abs(player.horProj) > 0.1f && player.grounded) {
			player.sprite.FlipH = (player.horProj < 0) ? true : false;
		}

	}
	public override void End(Node entity) {
		PlayerController player = (PlayerController)entity;
		player.orb = false;
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		player.PhysicsMaterialOverride.Bounce = 0;

		if (inputDir != Vector2.Zero)
		{
			player.LinearVelocity = new Vector2(inputDir.X, inputDir.Y) * exitMoveSpeed;
		}

	}
	
}
