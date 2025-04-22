using Godot;
using System;

public partial class EntityGrabbedState : EntityState
{
	public override void Start() {
		entity.applyPhysics = false;
		base.Start();
	}

	public override void _Process(double delta) {

		if(!active) return;

		entity.SetHor(0);
		entity.SetVert(0);
		entity.Position = Vector2.Zero;
		base._Process(delta);
	}

	public void Thrown(float angle, float speed) {
		entity.SetHor(Mathf.Cos(angle) * speed);
		entity.SetVert(-Mathf.Sin(angle) * speed);
		entity.SwitchState("ThrownState");
	}
}
