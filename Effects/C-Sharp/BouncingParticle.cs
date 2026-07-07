using Godot;
using System;

public partial class BouncingParticle : RigidBody2D
{
	float t;
	Node opo;
	bool active;
	RandomNumberGenerator rand;
	public override void _Ready()
	{
		t = 0.01f;
		if(GetNodeOrNull<Node>("ObjectPoolObject") != null)
			opo = GetNode<Node>("ObjectPoolObject");
		active = false;
		rand = new RandomNumberGenerator();
		Visible = false;
		SetDeferred("freeze", true);
	}
	public void OnSpawn() {
		active = true;
		t = 0.01f;
		Visible = true;
		SetDeferred("freeze", false);
		float dir = rand.RandfRange(0, 2*Mathf.Pi);
		float speed = rand.RandiRange(80, 130);
		LinearVelocity = Godot.Vector2.Zero;
		LinearVelocity = (speed*new Godot.Vector2(Mathf.Cos(dir), Mathf.Sin(dir)));
	}
	public override void _PhysicsProcess(double delta) {
		if(!active) return;
		t += (float)delta;
		Scale = (1-t) * Vector2.One;
		if(t > 1) {
			Scale = Godot.Vector2.One;
			if (opo != null)
				opo.Call("Despawn");
			else
				CallDeferred(MethodName.QueueFree);
			SetDeferred("freeze", true);
			Visible = false;
			active = false;
		}	
	}
}
