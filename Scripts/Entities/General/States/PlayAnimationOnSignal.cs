using Godot;
using System;

[GlobalClass]
public partial class PlayAnimationOnSignal : StateScript
{
	[Export] public StringName animName;
	private void _on_signal() {
		GD.Print("signal");
		if(active) {
			var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start(animName, true);
		}
	}
}
