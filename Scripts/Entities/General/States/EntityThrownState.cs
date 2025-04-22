using Godot;
using System;

public partial class EntityThrownState : EntityState
{
	float maxCannonballTime = 2f;
	float cannonballTime = 0;
	public override void Start() {
		entity.applyPhysics = true;
		entity.collisionMode = Agent.CollisionMode.BOUNCE; 
		base.Start();
	}

	public override void _Process(double delta) {

		if(!active) return;
		cannonballTime += (float)delta;
		if(cannonballTime > maxCannonballTime) {
			entity.GetNode<ProcessDeathEnemyBasic>("Attributes/ProcessDeathEnemyBasic").Death();
		}
		base._Process(delta);
	}
}
