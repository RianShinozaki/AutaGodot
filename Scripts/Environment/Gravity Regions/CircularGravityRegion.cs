using Godot;
using System;

public partial class CircularGravityRegion : GravityRegion
{
	public override Vector2 GetGravity(Vector2 position) {
		Vector2 dir = (position - GlobalPosition).Normalized();
		return dir;
	}
}
