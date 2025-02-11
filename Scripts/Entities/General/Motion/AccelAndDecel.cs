using Godot;
using System;

[GlobalClass]
public partial class AccelAndDecel : StateScript
{
	[Export] float speed;
	[Export] float accel;
	[Export] float decel;
	[Export] float decelTooFast;
	[Export] float airAccel;
	[Export] float airDecel;
	[Export] float airDecelTooFast;

	public override void _Process(double delta)
	{
		if(!active) return;
		
		float hor = Input.GetAxis("ui_left", "ui_right");

		if(Mathf.Abs(hor) > 0.1f) {
			int sign = Mathf.Sign(hor);
			if(entity.horSpeed * hor < speed)
				entity.AccelerateHor((entity.grounded ? accel : airAccel) * hor, speed * hor, true);
			else {
				entity.AccelerateHor((entity.grounded ? decelTooFast : airDecelTooFast) * hor, speed * hor, true);
			}
		}
		else {
			entity.AccelerateHor(entity.grounded ? decel : airDecel, 0);
		}
	}
}
