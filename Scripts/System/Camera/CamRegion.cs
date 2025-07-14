using Godot;
using System;

public partial class CamRegion : Area2D
{
	[Export] public Vector2 boundsX;
	[Export] public Vector2 boundsY;
	public override void _Ready()
	{
		float sizeX = ((RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.X;
		boundsX = new Vector2(GetNode<CollisionShape2D>("CollisionShape2D").GlobalPosition.X - sizeX / 2, GetNode<CollisionShape2D>("CollisionShape2D").GlobalPosition.X + sizeX / 2);

		float sizeY = ((RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.Y;
		boundsY = new Vector2(GetNode<CollisionShape2D>("CollisionShape2D").GlobalPosition.Y - sizeY / 2, GetNode<CollisionShape2D>("CollisionShape2D").GlobalPosition.Y + sizeY / 2);

	}

}
