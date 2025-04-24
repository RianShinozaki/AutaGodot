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

	[Export] public Godot.Collections.Array<Area2D> ignoreAreas = new Godot.Collections.Array<Area2D>();
	[Export] public float knockbackRecovery;
	[Export] public bool canTuck;
	[Export] public string hurtState = "HurtState";
	[Export] public string hurtAnim = "Hurt";
	[Export] public string recoverState = "NormalState";
	[Export] public string recoverAnim = "Idle";
	[Export] public string tuckState = "TuckState";
	[Export] public string tuckAnim = "Tuck";


	public float horKnockbackSpeed;
	public bool canRecoverInAir;
	private bool tucked;

    public override void Init()
    {
        base.Init();

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
		if(area is Hitbox hitB){
			HitboxData dat = hitB.hitboxData;
			horKnockbackSpeed = dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1);
			entity.SetHor(dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1));
			entity.SetVert(-dat.yKnockback);
			entity.GetNode<EntityHealth>("Attributes/EntityHealth").ChangeHealth(-dat.damage);
			
			float hp = entity.GetNode<EntityHealth>("Attributes/EntityHealth").health;
			if(hp > 0) {
				entity.SwitchState(hurtState);
				var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("RESET", true);
				stateMachine.Start(hurtAnim, true);
				EmitSignal(SignalName.Hurt);
			}
			else if(hp <= 0 && !tucked && canTuck) {
				tucked = true;
				entity.SwitchState(tuckState);
				var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
				stateMachine.Start("RESET", true);
				stateMachine.Start(tuckAnim, true);
				EmitSignal(SignalName.Hurt);
			}
			else {
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


	
}
