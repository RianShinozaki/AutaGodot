using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerInteractingState : EntityState
{
	public BasicInteractable theInteractable;
	public AccelAndDecel accelAndDecel;

	public override void Start()
	{
		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
		accelAndDecel = GetNode<AccelAndDecel>("ActiveScriptGroup/AccelAndDecel");
		accelAndDecel.horReset = true;
		base.Start();
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;

		if (theInteractable != null && theInteractable.repositionPlayer)
		{
			float targetX = theInteractable.GlobalPosition.X + theInteractable.Scale.X * 16;
			if (targetX - GlobalPosition.X > 8)
			{
				accelAndDecel.hor = 1;
			}
			else if (targetX - GlobalPosition.X < -8)
			{
				accelAndDecel.hor = -1;
			}
			else {
				entity.sprite.FlipH = !(theInteractable.GlobalPosition.X > GlobalPosition.X);
			}
		}

	}
	public override void End() {
		base.End();
	}
}
