using Godot;
using System;

[GlobalClass]
public partial class ProcessDeathEnemyBasic : Node
{
	public StateEntity entity;
	[Signal]
	public delegate void DiedEventHandler();
	
	public override void _Ready()
	{
		entity = GetParent<Node2D>().GetParent<StateEntity>();	
	}

	public void Death() {
		Node2D fx = ObjectPool.Instance.Spawn("Burst") as Node2D;
		if(fx != null) {
			fx.GlobalPosition = new Vector2(entity.GlobalPosition.X, entity.GlobalPosition.Y);
		}
		EmitSignal(SignalName.Died);
		entity.QueueFree();
	}
				
}
