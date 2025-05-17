using Godot;
using System;
[GlobalClass]
public partial class EntityState : Node2D
{
	[Export] public bool autoHook = true;
	public StateEntity entity;

	[Signal]
	public delegate void StateBegunEventHandler();
	[Signal]
	public delegate void StateEndEventHandler();

	[Export] public bool active = false;
    public override void _Ready()
    {
        base._Ready();
		if(autoHook) {
			entity = GetParent().GetParent<StateEntity>();
		}
		foreach(Node node in GetChildren()) {
			if(node is StateScriptGroup scriptGroup)
				scriptGroup.Init();
			if(node is StateComponentGroup compGroup)
				compGroup.Init();
		}
    }
    public virtual void Start() { 
		active = true;

		EmitSignal(SignalName.StateBegun);
	}
	public virtual void End() {
		active = false;

		EmitSignal(SignalName.StateEnd);
	}
	
}

