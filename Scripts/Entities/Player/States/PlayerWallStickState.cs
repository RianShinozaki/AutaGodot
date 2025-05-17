using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerWallStickState : EntityState
{
	[Export] public float decel;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	public override void Start() {
		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		stateMachine = entity.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Duck", true);
		PlayerController player = (PlayerController)entity;
		player.duck = true;
		entity.collisionMode = Agent.CollisionMode.FREE;
		entity.horSpeed = 0;
		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;
		entity.grounded = true;

		player.AccelerateVert( -Mathf.Sign(player.vertSpeed) * decel * (float)delta);

	}
	public override void End() {

		PlayerController player = (PlayerController)entity;
		player.duck = false;

		base.End();
	}

	private void _on_jump_jumped() {
		entity.SwitchState("NormalState");
		entity.collisionMode = Agent.CollisionMode.FLOOR;
		entity.sprite.Rotation = 0;
	}
}
