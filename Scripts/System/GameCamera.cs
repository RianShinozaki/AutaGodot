using Godot;
using System;

public partial class GameCamera : Camera2D
{
	[Export] public Node2D target;
	
	public static float screenshake = 0;
	float screenshakefalloff = 10;
	RandomNumberGenerator rand;
	public static GameCamera Instance;
    public override void _Ready()
    {
        base._Ready();
		rand = new RandomNumberGenerator();
		Instance = this;
    }


	public override void _PhysicsProcess(double delta)
	{
		if(screenshake > 0) screenshake -= screenshakefalloff * (float)delta;
		GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition 
											+ new Vector2(rand.RandfRange(-screenshake, screenshake), 
											rand.RandfRange(-screenshake, screenshake)), 
											0.2f);
		
		Callable.From(() => {
			screenshake = 0;
		}).CallDeferred();
	}
}
