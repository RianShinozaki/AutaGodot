using Godot;
using System;

public partial class Trail : Line2D
{
	[Export] float length = 50;

    public override void _Process(double delta)
    {
		if (!Visible) return;

		GlobalPosition = Vector2.Zero;
		GlobalRotation = 0f;

		Vector2 point = GetParent<Node2D>().GlobalPosition + new Vector2(7, 0);
		AddPoint(point);
		while (GetPointCount() > length)
		{
			RemovePoint(0);
		}

        base._Process(delta);
    }

}
