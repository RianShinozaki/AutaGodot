using Godot;
using System;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class TriggerEnterAct : StateScript
{
	[Export] public string toState;
	[Export] public StringName playAnim;
	[Export] public bool faceTarget;
	[Export] public Vector2 triggerWaitRange;
	[Export] public Vector2 SubseqTriggerWaitRange;
	RandomNumberGenerator rand;
	Node2D inContact;
	public float waitTime;
	public float t;
	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").BodyEntered += _on_body_entered;
		GetNode<Area2D>("Area2D").BodyExited += _on_body_exited;
		GetNode<Area2D>("Area2D").AreaEntered += _on_area_entered;
		GetNode<Area2D>("Area2D").AreaExited += _on_area_exited;
		rand = new RandomNumberGenerator();
	}

	private void _on_body_entered(Node2D body) {
		waitTime = rand.RandfRange(triggerWaitRange.X, triggerWaitRange.Y);
		inContact = body;
		
	}
	private void _on_body_exited(Node2D body) {
		inContact = null;
	}

	private void _on_area_entered(Area2D area)
	{
		waitTime = rand.RandfRange(triggerWaitRange.X, triggerWaitRange.Y);
		inContact = area;
		
	}
	private void _on_area_exited(Area2D area) {
		inContact = null;
	}

    public override void _Process(double delta)
	{
		if (!active) return;
		if (inContact != null)
		{
			t += (float)delta;
			if (t > waitTime)
			{
				if (toState != "") entity.SwitchState(toState);
				if (playAnim != "")
				{
					var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
					stateMachine.Start(playAnim, true);
				}
				if (faceTarget)
				{
					entity.sprite.FlipH = inContact.GlobalPosition.X > GlobalPosition.X ? false : true;
					GetParent().GetNode<AccelAndDecel>("AccelAndDecel").BurstSpeed(5, 0, true);
					GetParent().GetNode<AccelAndDecel>("AccelAndDecel").hor = 0;
				}
				waitTime = rand.RandfRange(SubseqTriggerWaitRange.X, SubseqTriggerWaitRange.Y);
				t = 0;
			}

		}
	}
}
