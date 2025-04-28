using Godot;
using System;

[GlobalClass]
public partial class ScreenshakeEmitter : StateScript
{
	[Export] public float shake;
	[Export] public Curve distanceFalloffCurve;
	[Export] float maxRange;
	[Export] float maxTime;
	[Export] public Curve timeFalloffCurve;

	float time;
	bool shaking;

	public void EmitShake()
	{
		if(!active) return;

		time = 0;
		shaking = true;
	}
	public void StopShake() {
		shaking = false;
	}
	public override void _Process(double delta)
	{
		if(!active) return;
		if(!shaking) return;
		float len = (GameCamera.Instance.GlobalPosition - entity.GlobalPosition).Length();
		float calcShake = shake * timeFalloffCurve.Sample(time/maxTime) 
								* distanceFalloffCurve.Sample(len/maxRange);
		if(GameCamera.screenshake > calcShake) return;
		GameCamera.screenshake = calcShake;
		time = Mathf.MoveToward(time, maxTime, (float)delta);
		if(time == maxTime) StopShake();
	}
}
