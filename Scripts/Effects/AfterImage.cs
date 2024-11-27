using Godot;
using System;

public partial class AfterImage : Sprite2D
{
	ShaderMaterial mat;
	float t;
	ObjectPoolObject opo;
	bool active;
	public override void _Ready()
	{
		mat = Material as ShaderMaterial;
		t = 0.01f;
		opo = GetNode<ObjectPoolObject>("ObjectPoolObject");
		active = false;
		
	}
	public void OnSpawn() {
		active = true;
		t = 0.01f;
		Visible = true;
	}

	public override void _Process(double delta)
	{
		if(!active) return;

		mat.SetShaderParameter("t", t);
		t += (float)delta * 2;
		if(t >= 1) {
			Visible = false;
			opo.Despawn();
			active = false;
		}
	}
}
