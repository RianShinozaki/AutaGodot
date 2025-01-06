using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	public override void _Ready()
	{
		Instance = this;
	}

	public async void FrameFreeze(float timescale, float duration) {
		Engine.TimeScale = timescale;
		await ToSignal(GetTree().CreateTimer(duration * timescale), "timeout");
		Engine.TimeScale = 1.0f;
	}
}
