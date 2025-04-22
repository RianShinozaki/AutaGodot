using Godot;
using System;

[GlobalClass]
public partial class ColliderSetActive : Area2D
{
	public CollisionShape2D collider;
	[Export] private bool autoHook;
    public override void _Ready()
    {
        base._Ready();
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		if(autoHook) {
			GetParent<StateComponentGroup>().StateBegun += makeColliderActive;
			GetParent<StateComponentGroup>().StateEnd += makeColliderInactive;
		}
    }

	private void makeColliderActive() {
		collider.Disabled = false;

	}
	private void makeColliderInactive() {
		collider.Disabled = true;

	}
}
