using Godot;
using System;

[GlobalClass]
public partial class PlayAnimationOnSignal : StateScript
{
	[Export] public StringName animName;
	private void _on_signal() {
		if(active) {
			var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
			stateMachine.Start("RESET", true);

			//await ToSignal(entity.animPlayer, "animation_finished");
			stateMachine.Start(animName, true);
		}
	}
}
