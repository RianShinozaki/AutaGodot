using Godot;
using System;

[GlobalClass]
public partial class ObjectPoolObject : Node
{
	public ObjectPool myPool;
	public String myName;
	public virtual void Spawn(ObjectPool pool, String name) {
		myPool = pool;
		myName = name;
		GetParent().Call("OnSpawn");
	}
	public virtual void Despawn() {
		myPool.ReturnToPool(GetParent(), myName);
	}
}
