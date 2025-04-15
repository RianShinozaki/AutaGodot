using Godot;
using System;

public partial class StateScript : Node2D
{
	[Export] bool autoHook = true;
	[Export] public StateEntity entity;
	public bool active {
		get {
			return GetParent<StateScriptGroup>().GetActive();
		}
	}
	public virtual void Init()
	{
		if(autoHook) entity = GetParent<StateScriptGroup>().entity;
	}
}
