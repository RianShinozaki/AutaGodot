using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerGrabState : EntityState
{
	[Export] public float initVertSpeed = 0;
	[Export] public float timescale = 1f;
	[Export] public float minGrabTime = 0.25f;
	[Export] public float throwPower = 50;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	float grabTime = 0;
	Node2D cacheParent;
	StateEntity heldEntity;

	public override void Start() {
		PlayerController player = (PlayerController)entity;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("ThrowSpin", true);
		player.SetVert(-initVertSpeed);
		player.SetHor(player.horSpeed/2);
		GameManager.Instance.TransitionTimeScale(timescale, 0.1f);
		
		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;

		if(grabTime >= minGrabTime && !Input.IsActionPressed("Orb")) {
			Throw();
		}
		grabTime += (float)delta;
	}
	private void Throw() {
		Callable.From(() => {
			heldEntity.Reparent(cacheParent);
		}).CallDeferred();
		entity.SwitchState("NormalState");
		entity.SetVert(-initVertSpeed);
		((EntityGrabbedState)heldEntity.GetState("GrabbedState")).Thrown(Mathf.Pi/6, throwPower);
	}
	private void OnEnemyGrabbed(Node2D node) {
		GD.Print("OnGrabbed");
		cacheParent = node.GetParent<Node2D>();
		heldEntity = (StateEntity)node;
		Callable.From(() => {
			heldEntity.Reparent(GetNode<Node2D>("FlipPivot/ThrowParent"));
		}).CallDeferred();
	}
	public override void End() {
		base.End();
		GameManager.Instance.TransitionTimeScale(1.0f, 0.1f);
	}
}
