using Godot;
using System;
using System.Runtime.ExceptionServices;

[GlobalClass]
public partial class ProcessKnockback : StateScript
{
	[Export] public float knockbackRecovery;
	[Signal]
	public delegate void knockbackRecoveredEventHandler();
	public float horKnockbackSpeed;
	public bool canRecoverInAir;
	public override void _PhysicsProcess(double delta)
	{
		if(!active) {
			entity.sprite.Skew = 0;
			return;
		}
		
		if(horKnockbackSpeed != 0) {
			//entity.Move(horKnockbackSpeed, 0, (float)delta, false);
			entity.sprite.Skew = horKnockbackSpeed/200;
			if(knockbackRecovery != -1 && (canRecoverInAir || entity.grounded))
				horKnockbackSpeed = Mathf.MoveToward(horKnockbackSpeed, 0, knockbackRecovery * (float)delta);

			if(horKnockbackSpeed == 0) {
				entity.sprite.Skew = 0;
				if(entity.GetNode<EntityHealth>("Attributes/EntityHealth").health > 0)
					EmitSignal(SignalName.knockbackRecovered);
			}
		}
	}
}
