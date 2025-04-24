using Godot;
using System;

[GlobalClass]
public partial class ColliderSetActive : Node2D
{
	public CollisionShape2D collider;
	[Export] private bool autoHook = true;
    public override void _Ready()
    {
        base._Ready();
		collider = GetChild(0).GetChild<CollisionShape2D>(0);
		if(autoHook) {
			GetParent<StateComponentGroup>().StateBegun += makeColliderActive;
			GetParent<StateComponentGroup>().StateEnd += makeColliderInactive;
		}
    }

	private void makeColliderActive() {
		Callable.From(() => {
			collider.Disabled = false;
		}).CallDeferred();

	}
	private void makeColliderInactive() {
		Callable.From(() => {
			collider.Disabled = true;
		}).CallDeferred();


	}
}
