using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerGrabState : EntityState
{
	[Export] public float initVertSpeed = 0;
	[Export] public float timescale = 1f;
	[Export] public float minGrabTime = 0.25f;
	[Export] public float throwPower = 50;
	[Export] public float recoilPower = 100;
	Vector2 cachedInput = Vector2.Zero;
	float cachedDirection = 0;
	float cachedRot = 0;
	float grabTime = 0;
	Node2D cacheParent;
	StateEntity heldEntity;
	private Vector2 aimPos;
	StateComponentGroup compGroup;
    public override void _Ready()
    {
        base._Ready();
		compGroup = GetNode<StateComponentGroup>("ActiveComponentGroup");
		compGroup.Visible = false;
    }

	public override void Start() {
		PlayerController player = (PlayerController)entity;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("ThrowSpin", true);
		player.SetVert(-initVertSpeed);
		player.SetHor(player.horSpeed/2);
		GameManager.Instance.TransitionTimeScale(timescale, 0.1f);
		GameCamera.Instance.TransitionLinesAlpha(0.2f, 0.1f);

		aimPos = Vector2.Right * (entity.sprite.FlipH ? -48 : 48);
		compGroup.Visible = true;


		base.Start();	
	}
	public override void _Process(double delta) {

		if(!active) return;

		PlayerController player = (PlayerController)entity;

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		aimPos += inputDir*720f*(float)delta;

		compGroup.GetNode<Sprite2D>("Reticle").Position = aimPos;
		compGroup.GetNode<Sprite2D>("Arrow").Rotation = Mathf.Atan2(aimPos.Y, aimPos.X);

		if(grabTime >= minGrabTime && !Input.IsActionPressed("Orb")) {
			Throw();
		}
		grabTime += (float)delta;
	}
	private void Throw() {
		GD.Print("TryThrow");
		PlayerController player = (PlayerController)entity;	

		float angle = Mathf.Atan2(aimPos.Y, aimPos.X);
		entity.SetVert(-recoilPower);
		entity.SetHor(-Mathf.Cos(angle)*recoilPower);
		
		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Grounded", true);
					
		stateMachine = entity.anim.Get("parameters/Jump/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("ThrowFlip", true);

		Node2D fx = ObjectPool.Instance.Spawn("Windburst") as Node2D;
		if(fx != null) {
			fx.GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y-8);
		}

		((EntityGrabbedState)heldEntity.GetState("GrabbedState")).Thrown(angle, throwPower);

		player.canOrb = true;
		

		entity.SwitchState("NormalState");
	}
	private void OnEnemyGrabbed(Node2D node) {
		GD.Print("OnGrabbed");
		cacheParent = node.GetParent<Node2D>();
		heldEntity = (StateEntity)node;
		Callable.From(() => {
			heldEntity.Reparent(GetNode<Node2D>("FlipPivot/ThrowParent"));
		}).CallDeferred();
	}
	public override void End() {
		base.End();
		compGroup.Visible = false;
		GameManager.Instance.TransitionTimeScale(1.0f, 0.1f);
		GameCamera.Instance.TransitionLinesAlpha(0f, 0.25f);

		if(heldEntity != null) {
				Callable.From(() => {
				heldEntity.Reparent(cacheParent);
			}).CallDeferred();
		}

	}
}
