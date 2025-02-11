using Godot;
using System;

[GlobalClass]
public partial class ProcessKnockback : StateScript
{
	[Export] public float knockbackRecovery;
	public float horKnockbackSpeed;
	public override void _PhysicsProcess(double delta)
	{
		if(!active) return;
		
		if(horKnockbackSpeed != 0) {
			entity.Move(horKnockbackSpeed, 0, (float)delta, false);
			entity.sprite.Skew = horKnockbackSpeed/200;
			if(knockbackRecovery != -1)
				horKnockbackSpeed = Mathf.MoveToward(horKnockbackSpeed, 0, knockbackRecovery * (float)delta);
			else
				horKnockbackSpeed = 0;
			if(horKnockbackSpeed == 0) entity.sprite.Skew = 0;
		}
	}
}
