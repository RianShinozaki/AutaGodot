using Godot;
using System;

public partial class StarPetal : RigidBody2D
{
	float t;
	ObjectPoolObject opo;
	bool active;
	RandomNumberGenerator rand;
	bool collectable = false;
	bool attracted = false;
	float attractionSpeed = 720f;
	public override void _Ready()
	{
		t = 0.01f;
		if (GetNodeOrNull<ObjectPoolObject>("ObjectPoolObject") != null)
			opo = GetNode<ObjectPoolObject>("ObjectPoolObject");
		active = false;
		rand = new RandomNumberGenerator();
		Visible = false;
		SetDeferred("freeze", true);
	}
	public void OnSpawn()
	{
		active = true;
		t = 0.01f;
		Visible = true;
		SetDeferred("freeze", false);
		float dir = rand.RandfRange(0, 2f * Mathf.Pi);
		float speed = rand.RandiRange(80, 100);
		LinearVelocity = Godot.Vector2.Zero;
		LinearVelocity = (speed * new Godot.Vector2(Mathf.Cos(dir), Mathf.Sin(dir)));
		GetNode<CollisionShape2D>("RangeDetector/CollisionShape2D").Disabled = false;

		DelayAttraction();
		
	}

	public async void DelayAttraction()
	{
		await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);
		collectable = true;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (!active) return;

		if (!attracted) LinearVelocity = new Vector2(LinearVelocity.X, Mathf.Min(LinearVelocity.Y, 20f));

		//t += (float)delta;
		Scale = (1 - t) * Vector2.One;
		if (t > 1)
		{
			Scale = Godot.Vector2.One;
			if (opo != null)
				opo.Despawn();
			else
				CallDeferred(MethodName.QueueFree);
			SetDeferred("freeze", true);
			Visible = false;
			active = false;
		}

		if (attracted && collectable)
		{
			GetNode<Line2D>("Line2D").Visible = true;
			LinearVelocity += (PlayerController.Instance.GlobalPosition - GlobalPosition).Normalized() * attractionSpeed * (float)delta;
			attractionSpeed += (float)delta * 720;
			LinearDamp += (float)delta * 2;
		}
	}

	private void _on_collision_detector_body_entered(Node body)
	{
		if (collectable)
			CallDeferred(MethodName.QueueFree);
	}

	private void _on_range_detector_area_entered(Area2D area)
	{
		attracted = true;
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true); 
	}

}
