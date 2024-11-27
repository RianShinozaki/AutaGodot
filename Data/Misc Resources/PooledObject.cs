using Godot;
using System;

[GlobalClass]
public partial class PooledObject : Resource {
	[Export] public String objectToPool;
	[Export] public int number;
}

