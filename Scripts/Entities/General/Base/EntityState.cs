using Godot;
using System;
[GlobalClass]
public partial class EntityState : Node2D
{
	[Export] public bool autoHook = true;
	public StateEntity entity;

	[Signal]
	public delegate void StateStatusChangedEventHandler(bool active);

	public bool active = false;
    public override void _Ready()
    {
        base._Ready();
		if(autoHook) {
			entity = GetParent().GetParent<StateEntity>();
		}
		foreach(Node node in GetChildren()) {
			if(node is StateScriptGroup scriptGroup)
				scriptGroup.Init();
		}
    }
    public virtual void Start() { 
		active = true;
		EmitSignal(SignalName.StateStatusChanged, true);
	}
	public virtual void End() {
		active = false;
		EmitSignal(SignalName.StateStatusChanged, false);
	}
}

