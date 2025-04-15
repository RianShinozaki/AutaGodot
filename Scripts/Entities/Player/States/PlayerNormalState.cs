using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerNormalState : EntityState
{
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	public override void Start() {
		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;

		if(player.grounded) {
			player.canOrb = true;
			player.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
			float vert = Input.GetAxis("ui_down", "ui_up");
			if(vert == -1) {
				player.SwitchState("DuckState");
			}
		}

	}
	public override void End() {
		base.End();
	}
}
