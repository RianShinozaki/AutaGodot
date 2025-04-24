using Godot;
using System;

//Attach these to a node underneath the sprite
[GlobalClass]
public partial class AfterImageGenerator : Node
{
	bool active;
	float afterImgTimer = 0;
	Sprite2D sprite;

	public override void _Ready()
	{
		sprite = GetParent<Sprite2D>();
	}

	public override void _Process(double delta)
	{
		if(active) afterImgTimer += (float)delta;
		if(afterImgTimer > 0.07f) {
			AfterImage afterImg = ObjectPool.Instance.Spawn("AfterImage") as AfterImage;
			if(afterImg != null) {
				afterImg.Texture = sprite.Texture;
				afterImg.Hframes = sprite.Hframes;
				afterImg.Vframes = sprite.Vframes;
				afterImg.FlipH = sprite.FlipH;
				afterImg.Frame = sprite.Frame;
				afterImg.GlobalPosition = sprite.GlobalPosition;
				afterImg.Offset = sprite.Offset;
				afterImg.Rotation = sprite.Rotation;
				ShaderMaterial mat = afterImg.Material as ShaderMaterial;
				if(sprite.Texture is CanvasTexture canv) {
					mat.SetShaderParameter("Texture", canv.DiffuseTexture);
				}
				else mat.SetShaderParameter("Texture", sprite.Texture);
			}
			afterImgTimer = 0;
		}
	}

	public void StartCreatingAfterImgs() {
		afterImgTimer = 0;
		active = true;
	}
	public void StopCreatingAfterImgs() {
		active = false;
	}
	
}
