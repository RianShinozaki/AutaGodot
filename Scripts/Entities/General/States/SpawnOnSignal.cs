using Godot;
using System;

[GlobalClass]
public partial class SpawnOnSignal : StateScript
{
	[Export] public string pooledObjectName;
	[Export] public Vector2 offset;
	private void _on_signal() {
		//Node2D fx = ObjectPool.Instance.Spawn(pooledObjectName) as Node2D;
		//if(fx != null) {
			//fx.GlobalPosition = entity.GlobalPosition + offset;
		//}
	}
}
