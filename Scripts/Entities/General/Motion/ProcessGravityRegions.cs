using Godot;
using System;

[GlobalClass]
public partial class ProcessGravityRegions : StateScript
{
	[Export] public GravityRegion currentGravRegion;
	[Export] public GravityRegion prevGravRegion;
	[Export] public Godot.Collections.Array gravQueue = new Godot.Collections.Array();
	[Export] public float gravChangeTimer = 0;


	//<HOOK THIS INTO HITBOX SIGNALS>
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
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
			entity.gravityUp = currentGravRegion.GetGravity(entity.GlobalPosition);
		else 
			entity.gravityUp = Vector2.Up;
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
}
