using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerSpeedState : EntityState
{
	[Export] public float minSpeedThreshold = 90;
	[Export] RayArray2D wallRayArray;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	private enum StickDir {
		LEFT,
		RIGHT
	}

	public override void Start() {
		PlayerController player = (PlayerController)entity;
		player.skating = true;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StartCreatingAfterImgs();
		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;

		if(player.grounded) {
			player.canOrb = true;
			float vert = Input.GetAxis("ui_down", "ui_up");
			if(vert == -1) {
				player.SwitchState("DuckState");
			}
		}
		wallRayArray.RotationDegrees = entity.sprite.FlipH ? 90 : -90;
		if(wallRayArray.IsColliding()) {
			WallStick(StickDir.RIGHT);
			GlobalPosition = new Vector2(wallRayArray.GetCollisionPoint().X - 8, GlobalPosition.Y);
			return;
		}
		if(Mathf.Abs(entity.horSpeed) <= minSpeedThreshold) {
			player.SwitchState("NormalState");
		}

	}
	public override void End() {
		PlayerController player = (PlayerController)entity;
		player.skating = false;

		base.End();
		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
	}

	private void WallStick(StickDir dir) {
		PlayerController player = (PlayerController)entity;
		player.SwitchState("WallStickState");
		player.horSpeed = 0;
		entity.sprite.RotationDegrees = entity.sprite.FlipH ? 90 : -90;
	}
}
