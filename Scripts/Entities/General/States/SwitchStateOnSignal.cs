using Godot;
using System;

[GlobalClass]
public partial class SwitchStateOnSignal : StateScript
{
	[Export] public string toState;
	private void _on_signal() {
		if(active)
			entity.SwitchState(toState);
	}
}
