using Godot;
using System;

[GlobalClass]
public partial class FlipToFacePlayer : StateScript
{
	[Signal]
	public delegate void FlipSpriteEventHandler(bool newFlip);
	[Export] public bool mustBeGrounded;
	[Export] public Node2D flipObject;
	public override void _Process(double delta)
	{
		if(!active) return;
		
		DoFlip();
	}
	private void DoFlip() {
		bool toFlipH = PlayerController.Instance.GlobalPosition.X > GlobalPosition.X ? false : true;
		if(toFlipH != entity.sprite.FlipH) {
			EmitSignal(SignalName.FlipSprite, toFlipH);
		}
		entity.sprite.FlipH = toFlipH;
		if(flipObject != null) {
			flipObject.Scale = new Vector2(entity.sprite.FlipH ? -1 : 1, 1);
		}
	}
}