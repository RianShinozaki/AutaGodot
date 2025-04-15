using Godot;
using System;

[GlobalClass]
public partial class AutaAbilityTransitions : StateScript
{
	[Export] public bool allowAttack;
	[Export] public float attackBufferTimeMax = 0.25f;
	[Export] public bool allowOrb;
	public float attackBufferTime = 0;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if(attackBufferTime > 0) attackBufferTime -= (float)delta;
		if(Input.IsActionJustPressed("Attack")) attackBufferTime = attackBufferTimeMax;

		if(!active) return;

		PlayerController player = (PlayerController)entity;
		//Orb Transition
		if(Input.IsActionJustPressed("Orb") && player.canOrb && allowOrb) {
			entity.SwitchState("OrbState");
			return;
		}

		if(attackBufferTime > 0 && allowAttack) {
			entity.SwitchState("AttackState");
			attackBufferTime = 0;
		}
		
	}
}
