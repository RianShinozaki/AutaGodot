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
	[Export] public bool requireGrounded = true;
	[Export] AudioStream jumpSound;
	bool canShortHop;

	[Signal]
	public delegate void JumpedEventHandler();

    public override void _Process(double delta)
	{
		if(!active) {
			canShortHop = false; 

			//Why?
			if(allowInitiation && Input.IsActionJustPressed("Jump") && (entity.grounded || !requireGrounded)) {
				canShortHop = true;
				//EmitSignal(SignalName.Jumped);
			}

			return;
		}

		if(allowInitiation && Input.IsActionJustPressed("Jump") && (entity.grounded || !requireGrounded)) {
			entity.vertSpeed = -jumpVelocity;
			entity.vertProj = -entity.vertSpeed;
			entity.grounded = false;
			canShortHop = true;
			if (jumpSound != null) SFXController.PlaySound(jumpSound, GlobalPosition);
			EmitSignal(SignalName.Jumped);
		}

		JumpingState js = entity.grounded ? JumpingState.Grounded : 
					entity.vertProj > riseThresh ? JumpingState.Rising :
					entity.vertProj < fallThresh ? JumpingState.Falling :
					JumpingState.Peak;

		//Set gravity
		switch(js) {
			case JumpingState.Grounded:
				entity.gravity = gravity;
				canShortHop = false;
				break;
			case JumpingState.Rising:
				entity.gravity = gravity * risingGravScale;
				if(!Input.IsActionPressed("Jump") && canShortHop)
					entity.gravity = gravity * shortHopGravScale;
				break;
			case JumpingState.Peak:
				canShortHop = false;
				entity.gravity = gravity * normalGravScale;
				break;
			case JumpingState.Falling:
				entity.gravity = gravity * fallingGravScale;
				canShortHop = false;
				break;
		}
		entity.gravity *= extraMult;
	}
}
