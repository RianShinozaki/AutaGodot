using Godot;
using System;

[GlobalClass]
public partial class ProcessHitbox : StateScript
{
	public virtual void _on_hurtbox_area_entered(Area2D area) {
		GD.Print("owch");
		if(area is Hitbox hitB){
			HitboxData dat = hitB.hitboxData;
			entity.horKnockbackSpeed = dat.xKnockback * (dat.flip ? hitB.GlobalScale.Y : 1);
		}
	}
}
