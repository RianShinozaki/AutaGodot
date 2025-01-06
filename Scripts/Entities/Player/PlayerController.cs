using Godot;
using System;

public partial class PlayerController : StateEntity
{

	public bool orb;
	public bool canOrb;
	public float attackBufferTime = 0;
	[Export] public float attackBufferTimeMax = 0.25f;


	public override void _Ready()
	{
		SwitchState("Normal");
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if(attackBufferTime > 0) attackBufferTime -= (float)delta;
		if(Input.IsActionJustPressed("Attack")) attackBufferTime = attackBufferTimeMax;
		flipPivot.Scale = new Vector2(sprite.FlipH ? -1 : 1, 1);
	}
	public void QueryAttack() {
		if(attackBufferTime > 0)
			SwitchState("Attack");

	}
	public override void DefaultHitboxEntered(Hurtbox hb) {
		//horKnockbackSpeed = 60f * (sprite.FlipH ? 1 : -1);
		GameManager.Instance.FrameFreeze(0.05f, 0.35f);
		//horSpeed = 0;
	}

}
