using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerWallStickState : EntityState
{
	[Export] public float decel;
	[Export] RayArray2D wallRayArray;

	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	float cachedHSpeed = 0;

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
		base.Start();
	}
	public override void _Process(double delta)
	{

		if (!active) return;

		PlayerController player = (PlayerController)entity;
		player.grounded = true;
		player.SetHor(0);
		player.AccelerateVert(-Mathf.Sign(player.vertSpeed) * decel * (float)delta);
		if (wallRayArray.IsColliding())
		{
			entity.GlobalPosition = new Vector2(wallRayArray.GetCollisionPoint().X + 8 * (entity.sprite.FlipH ? 1 : -1), GlobalPosition.Y);
		}

	}
	public override void End() {

		PlayerController player = (PlayerController)entity;
		player.duck = false;

		base.End();
	}

	private void _on_jump_jumped()
	{
		entity.collisionMode = Agent.CollisionMode.FLOOR;
		entity.sprite.Rotation = 0;
		
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
