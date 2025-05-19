using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerWallStickState : EntityState
{
	public enum WallStickSubState
	{
		STICK,
		UP,
		DOWN
	}
	[Export] public float decel;
	[Export] public float upInitSpeed;
	[Export] public float upMinSpeed;
	[Export] public float decelUp;
	[Export] public float decelUpSlow;
	[Export] public float UpSlowThreshold;

	[Export] RayArray2D wallRayArray;

	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	float cachedHSpeed = 0;
	WallStickSubState subState;

	public override void Start()
	{
		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		stateMachine = entity.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Duck", true);
		PlayerController player = (PlayerController)entity;
		player.duck = true;
		entity.collisionMode = Agent.CollisionMode.FREE;
		cachedHSpeed = player.GetState("SpeedState").GetNode<AccelAndDecel>("ActiveScriptGroup/AccelAndDecel").speed;
		player.SetHor(0);
		subState = WallStickSubState.STICK;
		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StartCreatingAfterImgs();
		base.Start();
	}
	public override void _Process(double delta)
	{

		if (!active) return;

		PlayerController player = (PlayerController)entity;
		player.grounded = true;
		player.SetHor(0);
		switch (subState)
		{
			case WallStickSubState.STICK:
				player.AccelerateVert(-Mathf.Sign(player.vertSpeed) * decel * (float)delta);
				float vert = Input.GetAxis("ui_down", "ui_up");

				if (vert > 0.2f)
				{
					subState = WallStickSubState.UP;
					var stateMachine = entity.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
					stateMachine.Start("FastRun", true);
					player.SetVert(-upInitSpeed);
				}
				break;

			case WallStickSubState.UP:
				player.AccelerateVert(-Mathf.Sign(player.vertSpeed) * (player.vertSpeed > -UpSlowThreshold ? decelUpSlow : decelUp) * (float)delta);
				if (player.vertSpeed > -upMinSpeed)
				{
					entity.SwitchState("NormalState");
				}
				break;
		}
		if (wallRayArray.IsColliding())
		{
			entity.GlobalPosition = new Vector2(wallRayArray.GetCollisionPoint().X + 8 * (entity.sprite.FlipH ? 1 : -1), GlobalPosition.Y);
		}

	}
	public override void End() {

		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
		PlayerController player = (PlayerController)entity;
		player.duck = false;
		entity.collisionMode = Agent.CollisionMode.FLOOR;
		entity.sprite.Rotation = 0;

		base.End();
	}

	private void _on_jump_jumped()
	{
		
		float hor = Input.GetAxis("ui_left", "ui_right");

		if (Mathf.Sign(hor) == (entity.sprite.FlipH ? -1 : 1))
		{
			entity.SwitchState("NormalState");
		}
		else
		{
			entity.sprite.FlipH = !entity.sprite.FlipH;
			wallRayArray.RotationDegrees = entity.sprite.FlipH ? 90 : -90;
			wallRayArray.ForceRaycastTransform();
			entity.SetHor(Mathf.Abs(cachedHSpeed) * (entity.sprite.FlipH ? -1 : 1));
			//entity.Move(entity.sprite.FlipH ? -10 : 10, 0, 1, false);
			entity.SwitchState("SpeedState");
		}
	}
}
