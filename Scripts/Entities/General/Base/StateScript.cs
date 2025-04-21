using Godot;
using System;

public partial class StateScript : Node2D
{
	[Export] bool autoHook = true;
	[Export] public StateEntity entity;
	[Export] public bool enabled = true;
	public bool active {
		get {
			return GetParent<StateScriptGroup>().GetActive() && enabled;
		}
	}
	public virtual void Init()
	{
		if(autoHook) entity = GetParent<StateScriptGroup>().entity;
	}
}
