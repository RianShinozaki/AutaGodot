using Godot;
using System;

[GlobalClass]
public partial class StickToSurface : StateScript
{
	[Signal]
	public delegate void AttachedToSurfaceEventHandler(Vector2 surfaceNormal, Vector2 velocity);
	[Signal]
	public delegate void DetachedFromSurfaceEventHandler(Vector2 lastSurfaceNormal);
	[Export] public RayArray2D rayArray;
	[Export] public bool rayArrayIsChild = false;
	[Export] public float attachedCheckDistance;
	[Export] public float detachedCheckDistance;
	[Export] public float attachedOffsetDistance;
	[Export] public float stickLerpRateInit;
	[Export] public float stickLerpRateRamp;
	bool isTouching = false;
	Vector2 lastSurfaceNormal;
	float lerpRate;
	float rayArrayAngle
	{
		get => rayArray.Rotation + MathF.PI/2;
		set => rayArray.Rotation = value-MathF.PI/2;
	}
    public override void _Ready()
    {
		if (rayArrayIsChild) rayArray = GetNode<RayArray2D>("RayArray2D");
        base._Ready();
    }
	Vector2 rayArrayDirection {
		get => new Vector2(Mathf.Cos(rayArrayAngle), -Mathf.Sin(rayArrayAngle));
	}
	float projectToRayArray
	{
		get => entity.Velocity.Project(rayArrayDirection).Length();
	}

	public override void _Process(double delta)
	{
		if (!active) return;
		//Make sure raycast data is up-to-date
		rayArray.ForceUpdateTransform();
		rayArray.ForceRaycastTransform();

		//Check if we're moving towards the surface or already on it
		if (rayArray.IsColliding() && (isTouching || projectToRayArray >= 0))
		{
			GD.Print("RayArrayAngle: " + rayArrayAngle.ToString());
			GD.Print("rayArrayDirection: " + rayArrayDirection.ToString());
			if (!isTouching)
			{
				EmitSignal(SignalName.AttachedToSurface, rayArray.GetCollisionNormal(), new Vector2(entity.horSpeed, entity.vertSpeed));
				lerpRate = stickLerpRateInit;
			}
			entity.GlobalPosition = GlobalPosition.Lerp(rayArray.GetCollisionPoint() - rayArrayDirection * attachedOffsetDistance, lerpRate);
			//entity.GlobalPosition = GlobalPosition.Lerp(new Vector2(GlobalPosition.X, rayArray.GetCollisionPoint().Y - attachedOffsetDistance), lerpRate);
			lerpRate = Mathf.MoveToward(lerpRate, 1, (float)delta * stickLerpRateRamp);
			isTouching = true;
			rayArray.TargetPosition = new Vector2(0, attachedCheckDistance);
			lastSurfaceNormal = rayArray.GetCollisionNormal();
		}
		else
		{
			if (isTouching == true)
			{
				EmitSignal(SignalName.DetachedFromSurface, lastSurfaceNormal);
			}
			isTouching = false;
			rayArray.TargetPosition = new Vector2(0, detachedCheckDistance);
		}
	}
}
