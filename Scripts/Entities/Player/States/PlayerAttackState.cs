using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerAttackState : EntityState
{
	[Export] float gravity;
	
	float afterImgTimer;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	public float comboTimer = 0;
	[Export] public float comboTimeMax = 0.25f;
	public int comboNum = 0;

	public override void Start() {
		entity.anim.AnimationFinished += OnAnimationEnd;
		PlayerController player = (PlayerController)entity;
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if(inputDir != Vector2.Zero) {
			player.sprite.FlipH = (inputDir.X < 0);
		}
		var stateMachine = player.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Attack", true);
		stateMachine = player.anim.Get("parameters/Attack/playback").As<AnimationNodeStateMachinePlayback>();
		switch(comboNum) {
			case 0:
				stateMachine.Start("Attack_Combo1", true);
				//comboNum++;
				comboTimer = comboTimeMax;
				break;
			case 1:
				stateMachine.Start("Attack_Combo2", true);
				comboNum++;
				comboTimer = comboTimeMax;
				break;
			case 2:
				stateMachine.Start("Attack_Combo3", true);
				comboNum = 0;
				comboTimer = comboTimeMax;
				break;
		}
		base.Start();
	}
	public override void _Process(double delta) {
		if(!active) {
        	if(comboTimer > 0) comboTimer -= (float)delta;
			else comboNum = 0;
			return;
		}
		PlayerController player = (PlayerController)entity;
		
		if(Input.IsActionJustPressed("Orb") && player.canOrb) {
			player.SwitchState("OrbState");
			return;
		}

		player.vertProj = -player.vertSpeed;
		player.horProj = player.horSpeed;

	}
    public override void End()
    {
		entity.anim.AnimationFinished -= OnAnimationEnd;

        base.End();
    }

    public void OnAnimationEnd(StringName animName)
    {
		PlayerController player = (PlayerController)entity;
		player.SwitchState("NormalState");
    }
	public void _on_hitbox_area_entered(Area2D area) {
		GameManager.Instance.FrameFreeze(0.05f, 0.2f);
	}
}
