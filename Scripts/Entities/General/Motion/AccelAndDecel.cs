using Godot;
using System;

[GlobalClass]
public partial class AccelAndDecel : StateScript
{
	[Export] public float hor;
	[Export] public bool horReset;
	[Export] bool directControl;
	[Export] public float speed;
	[Export] public float initSpeed;
	[Export] public float initSpeedAir;
	[Export] float accel;
	[Export] float decel;
	[Export] float decelTooFast;
	[Export] float airAccel;
	[Export] float airDecel;
	[Export] float airDecelTooFast;

	public override void _Process(double delta)
	{
		if(!active) return;
		
		if(directControl) hor = Input.GetAxis("ui_left", "ui_right");

		if(Mathf.Abs(hor) > 0.1f && !(entity.grounded && accel == 0)) {
			int sign = Mathf.Sign(hor);
			if(entity.horSpeed * hor < speed)
				entity.AccelerateHor((entity.grounded ? accel : airAccel) * hor * (float)delta, speed * hor, true);

				float thisInitSpeed = entity.grounded ? initSpeed : initSpeedAir;
				if( Mathf.Abs(entity.horSpeed) < thisInitSpeed) {
					entity.SetHor(hor*thisInitSpeed);
				}
			else {
				entity.AccelerateHor((entity.grounded ? decelTooFast : airDecelTooFast) * hor * (float)delta, speed * hor, true);
			}
		}
		else {
			entity.AccelerateHor( (entity.grounded ? decel : airDecel) * (float)delta, 0);
		}

		if(horReset)
			hor = 0;
	}

	public void BurstSpeed(float horSpeed, float vertSpeed, bool directionRelative) {
		entity.horSpeed = horSpeed;
		if(directionRelative) {
			if(entity.sprite.FlipH) entity.horSpeed = -horSpeed;
		}
		entity.vertSpeed = vertSpeed;
	}
}
