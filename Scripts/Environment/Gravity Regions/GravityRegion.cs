using Godot;
using System;

public partial class GravityRegion : Area2D
{
	[Export] Vector2 direction;

	public virtual Vector2 GetGravity(Vector2 position) {
		return direction;
	}
}
