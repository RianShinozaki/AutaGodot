using Godot;
using System;

[GlobalClass]
public partial class SetGravityThresh : StateScript
{
	enum JumpingState {
		Grounded,
		Rising,
		Peak,
		Falling
	}

	[Export] float gravity = 230f;
	[Export] float normalGravScale = 1;
	[Export] float risingGravScale = 1.6f;
	[Export] float fallingGravScale = 1.2f;
	[Export] public float riseThresh = 30f;
	[Export] float fallThresh = -10f;
	[Export] float fallSpeedMax;
	[Export] public float extraMult = 1;

    public override void _Process(double delta)
	{
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
