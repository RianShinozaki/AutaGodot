using Godot;
using System;

[GlobalClass]
public partial class StateComponentGroup : Node2D
{
	[Export] public bool autoHook = true;
	[Export] public bool runIfInactive = false;
	[Export] public bool runIfActive = true;
	[Export] public EntityState state;
	[Export] public StateEntity entity;

	[Signal]
	public delegate void StateBegunEventHandler();
	[Signal]
	public delegate void StateEndEventHandler();

    public void Init()
    {
		if(autoHook) entity = GetParent<EntityState>().entity;
		state = GetParent<EntityState>();
		GetParent<EntityState>().StateBegun += onStateBegun;
		GetParent<EntityState>().StateEnd += onStateEnd;
		EmitSignal(SignalName.StateEnd);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
    public bool GetActive() {
		if(runIfInactive && !state.active) return true;
		if(runIfActive && state.active) return true;
		return false;
	}

	public virtual void onStateBegun() { 
		EmitSignal(SignalName.StateBegun);
	}
	public virtual void onStateEnd() {
		EmitSignal(SignalName.StateEnd);

	}
}
