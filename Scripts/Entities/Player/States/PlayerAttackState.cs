using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerAttackState : EntityState
{
	[Export] float gravityFloatMult;
	
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
		float hor = Input.GetAxis("ui_left", "ui_right");
		float vert = Input.GetAxis("ui_up", "ui_down");
		if(hor != 0) {
			player.sprite.FlipH = (hor < 0);
		}
		var stateMachine = player.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Attack", true);
		stateMachine = player.anim.Get("parameters/Attack/playback").As<AnimationNodeStateMachinePlayback>();
		
		if(vert <= -0.75f) {
			stateMachine.Start("Attack_Launch", true);
			comboNum = 0;
		}
		else {
			switch(comboNum) {
			case 0:
				stateMachine.Start("Attack_Combo1", true);
				comboNum++;
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
		}
		
		base.Start();
	}
	public override void _Process(double delta) {
		if(!active) {
        	if(comboTimer > 0) comboTimer -= (float)delta;
			else comboNum = 0;
			return;
		}

	}
    public override void End()
    {
		entity.anim.AnimationFinished -= OnAnimationEnd;
		GetNode<Jump>("ActiveScriptGroup/Jump").extraMult = 1;
        base.End();
    }

    public void OnAnimationEnd(StringName animName)
    {
		if(!active) return;
			PlayerController player = (PlayerController)entity;
			player.SwitchState("NormalState");
    }
	public void _on_hitbox_area_entered(Area2D area) {
		GameManager.Instance.FrameFreeze(0.05f, 0.2f);
		if(!entity.grounded) {
			entity.vertSpeed = -GetNode<Jump>("ActiveScriptGroup/Jump").riseThresh;
			GetNode<Jump>("ActiveScriptGroup/Jump").extraMult = gravityFloatMult;
		}
	}
	private void _on_process_hitbox_hurt() {
		foreach(Node2D node in GetNode<Node2D>("Attacks").GetChildren()) {
			node.GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer").CallDeferred("play", "RESET");
		}
	}
}
