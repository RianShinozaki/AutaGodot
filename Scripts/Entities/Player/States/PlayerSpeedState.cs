using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerSpeedState : EntityState
{
	[Export] public float minSpeedThreshold = 90;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	public override void Start() {
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
		if(Mathf.Abs(entity.horSpeed) <= minSpeedThreshold) {
			player.SwitchState("NormalState");
		}

	}
	public override void End() {
		base.End();
		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
	}
}
