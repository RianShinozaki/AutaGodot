using Godot;
using System;

[GlobalClass]
public partial class StateScriptGroup : Node2D
{
	[Export] public bool autoHook = true;
	[Export] public bool runIfInactive = false;
	[Export] public bool runIfActive = true;
	[Export] public EntityState state;
	[Export] public StateEntity entity;
    public void Init()
    {
		if(autoHook) entity = GetParent<EntityState>().entity;
		state = GetParent<EntityState>();
		foreach(Node node in GetChildren()) {
			StateScript script = node as StateScript;
			script.Init();
		}
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
}
