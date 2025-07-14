using Godot;
using System;

public partial class DogAttack : EntityState
{
	[Export] public float punchPower = 250;
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if (!active) return;

		int dir = PlayerController.Instance.GlobalPosition.X > GlobalPosition.X ? 1 : -1;
		GetNode<AccelAndDecel>("ActiveScriptGroup/AccelAndDecel").hor = dir;

		if (PlayerController.Instance.currentState.Name != "OrbState")
		{
			entity.SwitchState("NormalState");
			var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start("Grounded", true);
		}

	}

	private void range_entered(Area2D area)
	{
		if (!active) return;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("RunAttack", true);
	}
	private void range_exited(Area2D area)
	{
		if (!active) return;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Run", true);
	}

	private void attack_entered(Area2D area)
	{
		int dir = PlayerController.Instance.GlobalPosition.X > GlobalPosition.X ? 1 : -1;
		PlayerController.Instance.SetHor(dir * punchPower);
		PlayerController.Instance.SetVert(-80f);

		if (PlayerController.Instance.currentState.Name == "OrbState")
		{
			((PlayerOrbState)PlayerController.Instance.GetState("OrbState")).orbTime = 0;
		}
		
	}
}
