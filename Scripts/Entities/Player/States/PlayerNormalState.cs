using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerNormalState : EntityState
{
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;

	public override void Start() {
		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;
		player.vertProj = -player.vertSpeed;
		player.horProj = player.horSpeed;

		if(player.grounded) {
			player.canOrb = true;
			player.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StopCreatingAfterImgs();
		}

		//Orb Transition
		if(Input.IsActionJustPressed("Orb") && player.canOrb) {
			player.SwitchState("OrbState");
			return;
		}
		
		player.QueryAttack();

	}
	public override void End() {
		base.End();
	}
}
