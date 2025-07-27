using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyHurt : StateScript
{

	[Signal] public delegate void HurtEventHandler();
	[Signal] public delegate void knockbackRecoveredEventHandler();
	[Signal] public delegate void DiedEventHandler();
	[Signal] public delegate void TuckedEventHandler();

	[Export] bool autoHookToHitBox = true;
	[Export] public Godot.Collections.Array<Area2D> ignoreAreas = new Godot.Collections.Array<Area2D>();
	[Export] public float knockbackRecovery;
	[Export] public bool canTuck;
	[Export] public string hurtState = "HurtState";
	[Export] public string hurtAnim = "Hurt";
	[Export] public string recoverState = "NormalState";
	[Export] public string recoverAnim = "Idle";
	[Export] public string tuckState = "TuckState";
	[Export] public string tuckAnim = "Tuck";

	RandomNumberGenerator rand;


	public float horKnockbackSpeed;
	public bool canRecoverInAir;
	private bool tucked;

	public override void Init()
	{
		base.Init();
		if (autoHookToHitBox)
		{
			entity.GetNode<Area2D>("Hurtbox").AreaEntered += _on_hurtbox_area_entered;
		}
		rand = new RandomNumberGenerator();
    }

	//Knockback proc
	public override void _PhysicsProcess(double delta)
	{
		if(!active) return;
		
		if(horKnockbackSpeed != 0) {
			//entity.Move(horKnockbackSpeed, 0, (float)delta, false);
			entity.sprite.Skew = horKnockbackSpeed/200;
			if(knockbackRecovery != -1 && (canRecoverInAir || entity.grounded))
				horKnockbackSpeed = Mathf.MoveToward(horKnockbackSpeed, 0, knockbackRecovery * (float)delta);

			if(horKnockbackSpeed == 0) {
				entity.sprite.Skew = 0;
				if(entity.GetNode<EntityHealth>("Attributes/EntityHealth").health > 0) {
					entity.SwitchState(recoverState);
					var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
					stateMachine.Start("RESET", true);
					stateMachine.Start(recoverAnim, true);

					EmitSignal(SignalName.knockbackRecovered);
				}
			}
		}
	}

    public virtual void _on_hurtbox_area_entered(Area2D area) {
		if(!active) return;
		if(ignoreAreas.Contains(area)) {
			return;
		}
		if(area is Hitbox hitB) {
			HitstunShader();
			HitboxData dat = hitB.hitboxData;
			horKnockbackSpeed = dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1);
			entity.SetHor(dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1));
			entity.SetVert(-dat.yKnockback);
			entity.GetNode<EntityHealth>("Attributes/EntityHealth").ChangeHealth(-dat.damage);
			SFXController.PlaySound(dat.impactSound, GlobalPosition, 1.0f, rand.RandfRange(1.0f, 1.4f));

			float hp = entity.GetNode<EntityHealth>("Attributes/EntityHealth").health;
			if(hp > 0) {
				entity.SwitchState(hurtState);
				var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("RESET", true);
				stateMachine.Start(hurtAnim, true);
				EmitSignal(SignalName.Hurt);
			}
			// Become a cannonball if killed by a heavybash
			else if(hp <= 0 && canTuck && dat.damageType == DamageType.HeavyBash) { 
				entity.SwitchState(tuckState);
				float totalSpeed = 250; //This shouldn't be hard set but whatever.
				float angle = Mathf.Atan2(GlobalPosition.Y - area.GlobalPosition.Y, GlobalPosition.X - area.GlobalPosition.Y);

				entity.SetHor(-totalSpeed * Mathf.Cos(angle));
				entity.SetVert(-totalSpeed * Mathf.Sin(angle));
				entity.SwitchState("ThrownState");

				tucked = true;
				var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("RESET", true);
				stateMachine.Start(tuckAnim, true);
				
				EmitSignal(SignalName.Hurt);
			}
			else if(hp <= 0 && !tucked && canTuck) {
				tucked = true;
				var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("RESET", true);
				stateMachine.Start(tuckAnim, true);
				entity.SwitchState(tuckState);
				
				EmitSignal(SignalName.Hurt);
			}
			else {
				if(! (dat.damageType == DamageType.HeavyBash && tucked))
				entity.GetNode<ProcessDeathEnemyBasic>("Attributes/ProcessDeathEnemyBasic").Death();
			}

			Node2D fx;
			for(int i = 0; i < dat.damage; i++) {
				fx = ObjectPool.Instance.Spawn("BloodGem") as Node2D;
				if(fx != null) {
					fx.GlobalPosition = new Vector2(entity.GlobalPosition.X, area.GlobalPosition.Y);
				}
			}

			fx = ObjectPool.Instance.Spawn(dat.HitFX) as Node2D;
			if(fx != null) {
				fx.GlobalPosition = new Vector2(entity.GlobalPosition.X, area.GlobalPosition.Y);
			}
		}
	}

	public async void HitstunShader() {
		ShaderMaterial mat = entity.sprite.Material as ShaderMaterial;
		mat.SetShaderParameter("stunFX", 1.0f);
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		mat.SetShaderParameter("stunFX", 0.0f);

	}
}
