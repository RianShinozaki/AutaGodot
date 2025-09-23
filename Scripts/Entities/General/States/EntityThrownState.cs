using Godot;
using System;

public partial class EntityThrownState : EntityState
{
	float maxCannonballTime = 2f;
	float cannonballTime = 0;
	float normalGravity = 120f;
	float noGravTime = 0.2f;
	float rotSpeed = Mathf.Pi*2*5;
	[Export] AudioStream wallCrashSound;
	public override void Start()
	{
		entity.applyPhysics = true;
		entity.collisionMode = Agent.CollisionMode.BOUNCE;
		entity.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StartCreatingAfterImgs();
		entity.Bounce += _on_bounce;
		base.Start();
	}
	[Export] public Godot.Collections.Array<Area2D> ignoreAreas = new Godot.Collections.Array<Area2D>();


	public override void _Process(double delta) {

		if(!active) return;
		entity.sprite.Rotate(rotSpeed * (float)delta * (entity.sprite.FlipH ? 1 : -1));
		cannonballTime += (float)delta;
		if(cannonballTime > noGravTime) { 
			entity.gravity = normalGravity;
		}
		else {
			entity.gravity = 0;
		}
		if(cannonballTime > maxCannonballTime) {
			entity.GetNode<ProcessDeathEnemyBasic>("Attributes/ProcessDeathEnemyBasic").Death();
		}
		entity.sprite.Skew = 0;
		base._Process(delta);
	}
	private void _on_attack_area_entered(Area2D area) {
		if(ignoreAreas.Contains(area)) {
			return;
		}
		GameManager.Instance.FrameFreeze(0.05f, 0.2f);
		float totalSpeed = new Vector2(entity.horSpeed, entity.vertSpeed).Length();
		float angle = Mathf.Atan2(GlobalPosition.Y - area.GlobalPosition.Y, GlobalPosition.X - area.GlobalPosition.X);

		entity.SetHor(-totalSpeed * Mathf.Cos(angle));
		entity.SetVert(-totalSpeed * Mathf.Sin(angle));
	}
	private void _on_bounce(Vector2 normal, Vector2 velocity) {

		float speed = normal.Dot(velocity);
		GD.Print(speed);
		if (speed > 20) {
			SFXController.PlaySound(wallCrashSound, GlobalPosition, 1.0f);
			//Node2D fx = ObjectPool.Instance.Spawn("WallSlam") as Node2D;
			//if(fx != null) {
				//fx.GlobalPosition = GlobalPosition - normal*4;
				//fx.Rotation = Mathf.Atan2(normal.X, -normal.Y);
			//}
			GetNode<ScreenshakeEmitter>("ActiveScriptGroup/ScreenshakeEmitter").EmitShake();
		}
		
	}
}
