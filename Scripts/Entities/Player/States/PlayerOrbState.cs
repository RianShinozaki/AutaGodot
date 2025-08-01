using Godot;
using System;
using System.Data;

[GlobalClass]
public partial class PlayerOrbState : EntityState
{
	[Signal] public delegate void GrabbedEntityEventHandler(Node2D node);
	[Export] float speed;
	[Export] float initSpeed;
	[Export] float initYSpeed;
	[Export] float minBounceSpeed;
	[Export] float minOrbTime;
	[Export] float bounce;
	[Export] float gravity;
	[Export] float drag;
	[Export] float dragApplyMinThresh;
	[Export] float initialGravScale;
	[Export] float gravityMultWaitTime;
	[Export] float exitYSpeed;
	[Export] float exitMoveSpeed;
	[Export] float minSpeedThresholdForSpeedState = 90f;
	[Export] public NodePath orbShape;
	[Export] public NodePath normalShape;
	[Export] public NodePath duckShape;
	[Export] public AudioStream orbThrow;
	[Export] public AudioStream orbBounce;
	[Export] public AudioStream orbReform;
	public float orbTime;
	Vector3 prevVelocity;

	Vector2 cachedInput;
	float cachedDirection;
	float cachedRot;
	bool canDeOrb;
	Godot.Collections.Array<Node> staticBodies;
	public override void _Ready()
	{
		base._Ready();
		entity.Bounce += on_bounce;
		staticBodies = new Godot.Collections.Array<Node>();
	}

	public override void Start()
	{
		PlayerController player = (PlayerController)entity;

		orbTime = 0;
		canDeOrb = true;
		player.collisionMode = Agent.CollisionMode.BOUNCE;

		var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		stateMachine.Start("Orb", true);

		if (entity.lastState.Name == "DuckState")
		{
			Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			inputDir.X = Mathf.Sign(inputDir.X);
			if (inputDir.X == 0) inputDir.X = player.sprite.FlipH ? -1 : 1;

			if (inputDir.X > 0.5f) player.horSpeed = Mathf.Max(player.horSpeed, inputDir.X * speed);
			else if (inputDir.X < -0.5f) player.horSpeed = Mathf.Min(player.horSpeed, inputDir.X * speed);
			else player.horSpeed = inputDir.X * speed;
		}
		else
		{
			Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (inputDir == Vector2.Zero) inputDir.X = player.sprite.FlipH ? -1 : 1;

			if (inputDir.X > 0.5f) player.horSpeed = Mathf.Max(player.horSpeed, inputDir.X * speed);
			else if (inputDir.X < -0.5f) player.horSpeed = Mathf.Min(player.horSpeed, inputDir.X * speed);
			else player.horSpeed = inputDir.X * speed;

			if (inputDir.Y > 0.5f) player.vertSpeed = Mathf.Max(player.vertSpeed, inputDir.Y * speed);
			else if (inputDir.Y < -0.5f) player.vertSpeed = Mathf.Min(player.vertSpeed, inputDir.Y * speed);
			else player.vertSpeed = inputDir.Y * speed;

			if (player.vertSpeed == 0 && player.grounded)
			{
				player.vertSpeed = initYSpeed;
			}
			if (player.vertSpeed == 0 && !player.grounded)
			{
				player.vertSpeed = initYSpeed * 0.3f;
			}
		}

		GetNode<CollisionShape2D>(orbShape).Disabled = false;
		GetNode<CollisionShape2D>(normalShape).Disabled = true;
		GetNode<CollisionShape2D>(duckShape).Disabled = true;

		player.sprite.GetNode<AfterImageGenerator>("AfterImageGenerator").StartCreatingAfterImgs();

		Node2D fx = ObjectPool.Instance.Spawn("OrbBurst") as Node2D;
		if (fx != null)
		{
			fx.GlobalPosition = GlobalPosition;
		}

		/*AfterImage afterImg = ObjectPool.Instance.Spawn("DisintegratedImage") as AfterImage;
		if(afterImg != null) {
			afterImg.Texture = player.sprite.Texture;
			afterImg.Hframes = player.sprite.Hframes;
			afterImg.Vframes = player.sprite.Vframes;
			afterImg.FlipH = player.sprite.FlipH;
			afterImg.Frame = player.sprite.Frame;
			afterImg.GlobalPosition = player.sprite.GlobalPosition;
			afterImg.Offset = player.sprite.Offset;

			ShaderMaterial mat = afterImg.Material as ShaderMaterial;
			mat.SetShaderParameter("Frame", player.sprite.Frame);
			mat.SetShaderParameter("VFrame", player.sprite.Vframes);
			mat.SetShaderParameter("HFrame", player.sprite.Hframes);
		}*/
		staticBodies.Clear();

		SFXController.PlaySound(orbThrow, GlobalPosition, 1.4f);

		base.Start();
	}
	public override void _Process(double delta)
	{
		PlayerController player = (PlayerController)entity;
		if (player.grounded) player.canOrb = true;

		if (!active) return;

		if (orbTime >= minOrbTime && !Input.IsActionPressed("Orb") )
		{
			float vert = Input.GetAxis("ui_down", "ui_up");
			if (entity.IsOnFloor() && (vert == -1 || !canDeOrb))
			{
				player.SwitchState("DuckState");
			}
			else if(canDeOrb)
			{
				if (Mathf.Abs(entity.horSpeed) > minSpeedThresholdForSpeedState && false)
				{
					player.SwitchState("SpeedState");
				}
				else
				{
					var stateMachine = entity.anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
					stateMachine.Start("Grounded", true);
					player.SwitchState("NormalState");
				}
			}
		}

		if (orbTime > gravityMultWaitTime)
		{
			player.gravity = gravity;
		}
		else
		{
			player.gravity = gravity * initialGravScale;
		}

		orbTime += (float)delta;
	}
	public override void End()
	{
		PlayerController player = (PlayerController)entity;
		player.orb = false;
		player.canOrb = false;
		player.collisionMode = Agent.CollisionMode.FLOOR;
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (inputDir != Vector2.Zero)
		{
			int sign = Mathf.Sign(inputDir.X);
			player.horSpeed = sign * Mathf.Max(player.horSpeed * sign, exitMoveSpeed);

			sign = Mathf.Sign(inputDir.Y);
			player.vertSpeed = sign * Mathf.Max(player.vertSpeed * sign, exitMoveSpeed);
		}

		Callable.From(() =>
		{
			GetNode<CollisionShape2D>(orbShape).Disabled = true;
			GetNode<CollisionShape2D>(normalShape).Disabled = false;
		}).CallDeferred();

		SFXController.PlaySound(orbReform, GlobalPosition, 1.4f);
		base.End();
	}

	private void _on_orb_grab_area_entered(Area2D area)
	{
		if (!active) return;

		StateEntity enemyEnt = area.GetParent().GetParent<StateComponentGroup>().entity;
		enemyEnt.SwitchState("GrabbedState");
		entity.SwitchState("GrabState");
		EmitSignal(SignalName.GrabbedEntity, (Node2D)enemyEnt);
	}

	private void on_bounce(Vector2 normal, Vector2 velocity)
	{
		float speed = normal.Dot(velocity);
		GD.Print(speed);
		if (speed > 20)
			SFXController.PlaySound(orbBounce, GlobalPosition, 1.0f);
	}

	private void on_environment_body_entered(Node body)
	{
		canDeOrb = false;
		if (!staticBodies.Contains(body))
			staticBodies.Add(body);
	}
	private void on_environment_body_exited(Node body)
	{
		if (staticBodies.Contains(body))
			staticBodies.Remove(body);
		if (staticBodies.Count == 0)
		{
			canDeOrb = true;
		}
	}
	
}
