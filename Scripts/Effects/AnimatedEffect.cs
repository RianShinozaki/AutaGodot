using Godot;
using System;

public partial class AnimatedEffect : Sprite2D
{
	ObjectPoolObject opo;
	RandomNumberGenerator rand;
	[Export] public float rotRange;
	public override void _Ready()
	{
		opo = GetNode<ObjectPoolObject>("ObjectPoolObject");
		rand = new RandomNumberGenerator();
		Visible = false;
	}
	public void OnSpawn() {
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Effect");
		RotationDegrees = rand.RandfRange(-rotRange, rotRange);
		Visible = true;
	}
	public void _on_animation_player_animation_finished(StringName anim_name) {
		opo.Despawn();
		Visible = false;
	}	
}
