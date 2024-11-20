using Godot;
using System;
using System.Linq;

public partial class StateEntity : Agent
{
	[Export] public Godot.Collections.Dictionary<String, EntityState> states;
	EntityState currentState;
	EntityState lastState;
	public Vector2 floorNormal;
	[Export] public ShapeCast2D shapeCast;
	[Export] public Vector2 gravityUp = new Vector2(0, -1);
	[Export] public Godot.Collections.Array gravQueue = new Godot.Collections.Array();
	public float horProj;
	public float vertProj;
	public GravityRegion currentGravRegion;
	public GravityRegion prevGravRegion;
	public float gravChangeTimer = 0;

	public void SwitchState(String newState) {
		if(currentState != null) {
			currentState.End(this);
		}
		lastState = currentState;
		currentState = states[newState];
		currentState.Start(this);
	}
    public override void _PhysicsProcess(double delta)
    {
		float GRAVCHANGECANCELTIME = 0.15f;

		if(gravChangeTimer <= 0) {
			if(gravQueue.Count == 0) {
				if(currentGravRegion != null)
					gravChangeTimer = GRAVCHANGECANCELTIME;
				currentGravRegion = null;
			} else {
				if(currentGravRegion != (GravityRegion)gravQueue[0])
					gravChangeTimer = GRAVCHANGECANCELTIME;
				currentGravRegion = (GravityRegion)gravQueue[0];
			}
		} else {
			gravChangeTimer -= (float)delta;
		}

		if(currentGravRegion != null)
			gravityUp = currentGravRegion.GetGravity(GlobalPosition);
		else 
			gravityUp = Vector2.Up;
		if(currentState != null) {
        	currentState.Update(this, Transform, delta);
		}

		base._PhysicsProcess(delta);
    }

	private void _on_hurtbox_area_entered(Area2D area) {
		if(area is GravityRegion grav) {
			gravQueue.Insert(0, grav);
		}
	}
	private void _on_hurtbox_area_exited(Area2D area) {
		if(area is GravityRegion grav) {
			gravQueue.Remove(grav);
		}
	}
    public override void OnGrounded(Vector2 normal, Vector2 velocity)
    {
        currentState.OnGrounded(this, normal, velocity);
    }
}
