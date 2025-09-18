using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class StateEntity : Agent
{
	//[ExportGroup("Manual References")]
	
	//[ExportGroup("Manual Variables")]

	
	[ExportGroup("Internal References")]
	[Export] public EntityState currentState;
	[Export] public EntityState lastState;
	[Export] public Sprite2D sprite;
	[Export] public AnimationPlayer animPlayer;
	[Export] public AnimationTree anim;

	[ExportGroup("Internal Variables")]
	[Export] public Vector2 gravityUp = new Vector2(0, -1);
	[Export] public Vector2 floorNormal;
	[Export] public float horProj;
	[Export] public float vertProj;

	public override void _Ready() {
		anim = GetNode<AnimationTree>("Art/AnimationTree");
		animPlayer = GetNode<AnimationPlayer>("Art/AnimationPlayer");
		sprite = GetNode<Sprite2D>("Art");
		if(currentState != null) SwitchState(currentState.Name);
		base._Ready();
	}
	public void SwitchState(String newState) {
		if(currentState != null) {
			currentState.End();
		}
		lastState = currentState;
		currentState = GetNodeOrNull<EntityState>("States/" + newState);
		if(currentState != null) currentState.Start();
	}
	public EntityState GetState(String stateName) {
		return GetNode<EntityState>("States/" + stateName);
	}
    public override void _PhysicsProcess(double delta)
    {
		//Holdover from gravity changes
		vertProj = -vertSpeed;
		horProj = horSpeed;
		base._PhysicsProcess(delta);
	
    }
}
