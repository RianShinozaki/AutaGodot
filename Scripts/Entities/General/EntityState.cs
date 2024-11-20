using Godot;
using System;
[GlobalClass]
public partial class EntityState : Resource
{
	public virtual void Start(Node entity) {}
	public virtual void Update(Node entity, Transform2D transform, double delta) {}
	public virtual void End(Node entity) {}
	public virtual void OnGrounded(Node entity, Vector2 normal, Vector2 velocity) {}
}

