using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerDuckState : EntityState
{
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	[Export] public NodePath orbShape;
	[Export] public NodePath normalShape;
	[Export] public NodePath duckShape;
	[Export] public AudioStream slide;
	bool canUnDuck;
	Godot.Collections.Array<Node> staticBodies;

	public override void Start()
	{
		base.Start();
		GetNode<CollisionShape2D>(duckShape).Disabled = false;
		GetNode<CollisionShape2D>(normalShape).Disabled = true;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		stateMachine = entity.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Duck", true);
		PlayerController player = (PlayerController)entity;
		player.duck = true;

		if (Mathf.Abs(entity.horSpeed) > 100)
		{
			SFXController.PlaySound(slide, GlobalPosition, 0.7f);
		}
		canUnDuck = true;
		staticBodies = new Godot.Collections.Array<Node>();
	}
	public override void _Process(double delta)
	{

		if (!active) return;

		PlayerController player = (PlayerController)entity;

		if (player.grounded)
		{
			player.canOrb = true;
			player.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
		}
		float vert = Input.GetAxis("ui_down", "ui_up");
		if (vert != -1 && canUnDuck)
		{
			player.SwitchState("NormalState");
			var stateMachine = player.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start("FromDuck", true);
		}

	}
	public override void End()
	{
		GetNode<CollisionShape2D>(duckShape).Disabled = true;
		GetNode<CollisionShape2D>(normalShape).Disabled = false;
		PlayerController player = (PlayerController)entity;
		player.duck = false;

		base.End();
	}
	
	private void on_environment_body_entered(Node body)
	{
		canUnDuck = false;
		if (!staticBodies.Contains(body))
			staticBodies.Add(body);
	}
	private void on_environment_body_exited(Node body)
	{
		if (staticBodies.Contains(body))
			staticBodies.Remove(body);
		if (staticBodies.Count == 0)
		{
			canUnDuck = true;
		}
	}
}
