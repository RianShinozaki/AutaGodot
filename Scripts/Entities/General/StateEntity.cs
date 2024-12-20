using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StateEntity : Agent
{
	[Export] public Godot.Collections.Dictionary<String, EntityState> states;
	[Export] public Vector2 gravityUp = new Vector2(0, -1);
	[Export] public Godot.Collections.Array gravQueue = new Godot.Collections.Array();
	[Export] public float knockbackRecovery;
	public AnimationTree anim;
	public AnimationPlayer animPlayer;
	public Sprite2D sprite;
	public Node2D flipPivot;
	EntityState currentState;
	EntityState lastState;
	public Vector2 floorNormal;
	public float horProj;
	public float vertProj;
	public GravityRegion currentGravRegion;
	public GravityRegion prevGravRegion;
	public float gravChangeTimer = 0;
	[Export] public float horKnockbackSpeed;
	float afterImgTimer = 0;
	bool afterImg = false;

	public override void _Ready() {
		anim = GetNode<AnimationTree>("Art/AnimationTree");
		animPlayer = GetNode<AnimationPlayer>("Art/AnimationPlayer");
		sprite = GetNode<Sprite2D>("Art");
		flipPivot = GetNode<Node2D>("FlipPivot");
		base._Ready();
	}
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
		foreach(KeyValuePair<String, EntityState>state in states) {
			state.Value.PassiveUpdate(this, Transform, delta);
		}

		base._PhysicsProcess(delta);
		
		if(horKnockbackSpeed != 0) {
			Move(horKnockbackSpeed, 0, (float)delta, false);
			if(knockbackRecovery != -1)
				horKnockbackSpeed = Mathf.MoveToward(horKnockbackSpeed, 0, knockbackRecovery * (float)delta);
			else
				horKnockbackSpeed = 0;

		}
		if(afterImg) afterImgTimer += (float)delta;

		if(afterImgTimer > 0.07f) {
			AfterImage afterImg = ObjectPool.Instance.Spawn("AfterImage") as AfterImage;
			if(afterImg != null) {
				afterImg.Texture = sprite.Texture;
				afterImg.Hframes = sprite.Hframes;
				afterImg.Vframes = sprite.Vframes;
				afterImg.FlipH = sprite.FlipH;
				afterImg.Frame = sprite.Frame;
				afterImg.GlobalPosition = sprite.GlobalPosition;
				afterImg.Offset = sprite.Offset;
			}
			afterImgTimer = 0;
		}

    }
	private void _on_hurtbox_area_entered(Area2D area) {
		if(area is GravityRegion grav) {
			gravQueue.Insert(0, grav);
		}
		if(area is Hitbox hb) {
			if(currentState != null)
        		currentState.OnHurtboxEntered(this, hb);
			else
				DefaultHurtboxEntered(hb);
		}
	}
	public virtual void DefaultHurtboxEntered(Hitbox hb) {
		HitboxData dat = hb.hitboxData;
		horKnockbackSpeed = dat.xKnockback * (dat.flip ? hb.GlobalScale.Y : 1);
	}
	private void _on_hurtbox_area_exited(Area2D area) {
		if(area is GravityRegion grav) {
			gravQueue.Remove(grav);
		}
	}
	private void _on_hitbox_area_entered(Area2D area) {
		if(area is Hurtbox hurtB) {
			if(currentState != null)
        		currentState.OnHitboxEntered(this, hurtB);
			else
				DefaultHitboxEntered(hurtB);
		}
	}
	public virtual void DefaultHitboxEntered(Hurtbox hb) {
		horKnockbackSpeed = 60f * (sprite.FlipH ? 1 : -1);
	}
	
    public override void OnGrounded(Vector2 normal, Vector2 velocity)
    {
		if(currentState != null)
        	currentState.OnGrounded(this, normal, velocity);
    }
	private void _on_animation_tree_animation_finished(StringName anim_name) {
		if(currentState != null)
			currentState.OnAnimationEnd(this, anim_name);	
	}

	public void StartCreatingAfterImgs() {
		afterImgTimer = 0;
		afterImg = true;
	}
	public void StopCreatingAfterImgs() {
		afterImg = false;
	}
}
