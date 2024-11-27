using Godot;
using System;
[GlobalClass]
public partial class EntityState : Resource
{
	public bool active = false;
	public virtual void Start(Node entity) { active = true;}
	public virtual void Update(Node entity, Transform2D transform, double delta) {}
	public virtual void PassiveUpdate(Node entity, Transform2D transform, double delta) {}
	public virtual void End(Node entity) {active = false;}
	public virtual void OnGrounded(Node entity, Vector2 normal, Vector2 velocity) {}
	public virtual void OnAnimationEnd(Node entity, string animName) {}
	public virtual void OnHurtboxEntered(Node entity, Hitbox hitB) {}
	public virtual void OnHitboxEntered(Node entity, Hurtbox hurtB) {}
}

