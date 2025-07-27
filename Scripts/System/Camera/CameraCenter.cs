using Godot;
using System;

public enum CamMode {
	LockY,
	Free
}
public partial class CameraCenter : Area2D
{
	[Export] public Vector2 deadzone;
	[Export] public Vector2 cameraSize;
	public Vector2 boundsX = new Vector2(float.MinValue, float.MaxValue);
	public Vector2 boundsY = new Vector2(float.MinValue, float.MaxValue);

	float playerYCache;
	CamMode currentCamMode = CamMode.LockY;

	Godot.Collections.Array<CamRegion> camRegions;

	public override void _Ready()
	{
		base._Ready();
		GlobalPosition = PlayerController.Instance.GlobalPosition;
		camRegions = new Godot.Collections.Array<CamRegion>();
	}

	public override void _PhysicsProcess(double delta)
	{
		PlayerController player = PlayerController.Instance;
		Vector2 velocity = Vector2.Zero;
		Vector2 toPos = player.GlobalPosition;
		float x = Mathf.Clamp(toPos.X, boundsX.X, boundsX.Y);
		float y = Mathf.Clamp(toPos.Y, boundsY.X, boundsY.Y);
		toPos = new Vector2(x, y);

		velocity.X = (toPos.X - GlobalPosition.X) * 5;
		velocity.Y = (playerYCache - GlobalPosition.Y) * 5;
		if (true || player.grounded || currentCamMode == CamMode.Free)
		{
			playerYCache = toPos.Y;
			currentCamMode = CamMode.LockY;
		}
		if (Mathf.Abs(GlobalPosition.Y - toPos.Y) > 64)
		{
			currentCamMode = CamMode.Free;
		}
		GlobalPosition += velocity * (float)delta;

		//GlobalPosition = new Vector2(x, y);
		GetNode<CollisionShape2D>("CollisionShape2D").GlobalPosition = player.GlobalPosition;
	}

	public void resetBounds()
	{
		if (camRegions.Count > 0)
		{
			CamRegion cr = camRegions[0];
			boundsX = cr.boundsX + new Vector2(cameraSize.X/2, -cameraSize.X/2);
			boundsY = cr.boundsY + new Vector2(cameraSize.Y/2, -cameraSize.Y/2);
		}

	}
	private void _on_area_entered(Node2D area)
	{
		if (area is CamRegion cr && !camRegions.Contains(cr))
		{
			camRegions.Insert(0, cr);
			resetBounds();
		}
	}
	private void _on_area_exited(Node2D area)
	{
		if (area is CamRegion cr && camRegions.Contains(cr))
		{
			camRegions.Remove(cr);
			resetBounds();
		}
	}
}
