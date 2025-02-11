using Godot;
using System;

[GlobalClass]
public partial class ProcessHitbox : StateScript
{
	ProcessKnockback procKnock;
    public override void Init()
    {
		procKnock = GetParent().GetNode<ProcessKnockback>("ProcessKnockback");
        base.Init();
    }
    public virtual void _on_hurtbox_area_entered(Area2D area) {
		if(!active) return;

		if(area is Hitbox hitB){
			HitboxData dat = hitB.hitboxData;
			procKnock.horKnockbackSpeed = dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1);

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
