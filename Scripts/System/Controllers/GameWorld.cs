using Godot;
using System;

public partial class GameWorld : CanvasModulate
{
	[Export] AudioStream bgMusic;
	public override void _Ready()
	{
		Callable.From(() => {
			MusicController.StartMusic(bgMusic, 0.5f, -5f);
		}).CallDeferred();
		
	}

	public override void _Process(double delta)
	{
	}
}
