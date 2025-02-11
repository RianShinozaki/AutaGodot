using Godot;
using System;

[GlobalClass]
public partial class Jump : StateScript
{
	enum JumpingState {
		Grounded,
		Rising,
		Peak,
		Falling
	}

	[Export] public bool allowInitiation;
	[Export] float jumpVelocity;
	[Export] float gravity;
	[Export] float normalGravScale;
	[Export] float risingGravScale;
	[Export] float fallingGravScale;
	[Export] float shortHopGravScale;
	[Export] public float riseThresh;
	[Export] float fallThresh;
	[Export] float fallSpeedMax;
	[Export] public float extraMult = 1;

    public override void _Process(double delta)
	{
		if(!active) return;

		if(allowInitiation && Input.IsActionJustPressed("Jump") && entity.grounded) {
			entity.vertSpeed = -jumpVelocity;
		}

		JumpingState js = entity.grounded ? JumpingState.Grounded : 
					entity.vertProj > riseThresh ? JumpingState.Rising :
					entity.vertProj < fallThresh ? JumpingState.Falling :
					JumpingState.Peak;

		//Set gravity
		switch(js) {
			case JumpingState.Grounded:
				entity.gravity = gravity;
				break;
			case JumpingState.Rising:
				entity.gravity = gravity * risingGravScale;
				if(!Input.IsActionPressed("Jump"))
					entity.gravity = gravity * shortHopGravScale;
				break;
			case JumpingState.Peak:
				entity.gravity = gravity * normalGravScale;
				break;
			case JumpingState.Falling:
				entity.gravity = gravity * fallingGravScale;
				break;
		}
		entity.gravity *= extraMult;
	}
}
