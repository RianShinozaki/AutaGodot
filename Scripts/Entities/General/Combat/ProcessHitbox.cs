using Godot;
using System;

[GlobalClass]
public partial class ProcessHitbox : StateScript
{
	ProcessKnockback procKnock;
	RandomNumberGenerator rand;

	[Signal]
	public delegate void HurtEventHandler();

    public override void Init()
    {
		procKnock = GetParent().GetNode<ProcessKnockback>("ProcessKnockback");
		rand = new RandomNumberGenerator();
        base.Init();
    }
    public virtual void _on_hurtbox_area_entered(Area2D area) {
		if(!active) return;
		
		if(area is Hitbox hitB){
			HitboxData dat = hitB.hitboxData;
			procKnock.horKnockbackSpeed = dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1);
			entity.SetHor(dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1));
			entity.vertSpeed = -dat.yKnockback;
			entity.GetNode<EntityHealth>("Attributes/EntityHealth").ChangeHealth(-dat.damage);
			SFXController.PlaySound(dat.impactSound, GlobalPosition, 1.0f, rand.RandfRange(1.0f, 1.4f));
			if(entity.GetNode<EntityHealth>("Attributes/EntityHealth").health > 0)
				EmitSignal(SignalName.Hurt);

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
