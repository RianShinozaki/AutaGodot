using Godot;
using System;

[GlobalClass]
public partial class FlipToFaceDirection : StateScript
{
	[Signal]
	public delegate void FlipSpriteEventHandler(bool newFlip);
	[Export] public bool mustBeGrounded;
	[Export] public Node2D flipObject;
	public override void _Process(double delta)
	{
		if(!active) return;
		//Flip sprite based on direction if grounded  
		if(Mathf.Abs(entity.horProj) > 0.1f && (!mustBeGrounded || (mustBeGrounded && entity.grounded))) {
			bool toFlipH = (entity.horProj < 0) ? true : false;
			if(toFlipH != entity.sprite.FlipH) {
				EmitSignal(SignalName.FlipSprite, toFlipH);
			}
			entity.sprite.FlipH = toFlipH;
			
		}
		if(flipObject != null) {
			flipObject.Scale = new Vector2(entity.sprite.FlipH ? -1 : 1, 1);
		}
	}
}
