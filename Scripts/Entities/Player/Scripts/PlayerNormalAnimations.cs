using Godot;
using System;

[GlobalClass]
public partial class PlayerNormalAnimations : StateScript
{
	public override void Init()
	{
		base.Init();
		entity.Grounded += _on_grounded;
	}

	public void _on_flip_sprite(bool newFlip) {
		var stateMachine = entity.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Turn", true);
	}
	public void _on_grounded(Vector2 normal, Vector2 velocity)
    {
		PlayerController player = (PlayerController)entity;
        var stateMachine = player.anim.Get("parameters/Grounded/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Land", true);
    }
}
