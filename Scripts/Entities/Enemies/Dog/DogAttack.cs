using Godot;
using System;

public partial class DogAttack : EntityState
{
	[Export] public float punchPower = 250;
	[Export] public float maxRunTime = 4.0f;
	float runTime;
	bool slipped = false;
	public override void _Ready()
	{
		base._Ready();
	}
	public override void Start()
	{
		base.Start();
		runTime = 0;
		slipped = false;
    }

	public override void _Process(double delta)
	{
		if (!active) return;

		if (!slipped)
		{
			int dir = PlayerController.Instance.GlobalPosition.X > GlobalPosition.X ? 1 : -1;
			GetNode<AccelAndDecel>("ActiveScriptGroup/AccelAndDecel").hor = dir;
		}

		if (PlayerController.Instance.currentState.Name != "OrbState")
		{
			entity.SwitchState("NormalState");
			var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start("Grounded", true);
		}

		runTime += (float)delta;
		if (runTime > maxRunTime && !slipped)
		{
			var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start("Slip", true);
			slipped = true;
			entity.SwitchState("SlipState");
		}
	}

	private void range_entered(Area2D area)
	{
		if (!active || slipped) return;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("RunAttack", true);
	}
	private void range_exited(Area2D area)
	{
		if (!active || slipped) return;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Run", true);
	}

	private void attack_entered(Area2D area)
	{
		int dir = PlayerController.Instance.GlobalPosition.X > GlobalPosition.X ? 1 : -1;
		PlayerController.Instance.SetHor(dir * punchPower);
		PlayerController.Instance.SetVert(-80f);
		GameManager.Instance.FrameFreeze(0.05f, 0.2f);


		if (PlayerController.Instance.currentState.Name == "OrbState")
		{
			((PlayerOrbState)PlayerController.Instance.GetState("OrbState")).orbTime = 0;
		}
		
	}
}
