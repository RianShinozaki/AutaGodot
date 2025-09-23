using Godot;
using System;

public partial class GameCamera : Camera2D
{
	[Export] public Node2D target;
	
	public static float screenshake = 0;
	float screenshakefalloff = 10;
	RandomNumberGenerator rand;
	public static GameCamera Instance;

	public static bool speedtranslock;
	Tween speedtransTween;


	[Signal]
	public delegate void SpeedLinesSuccessEventHandler(bool success);


    public override void _Ready()
    {
        base._Ready();
		rand = new RandomNumberGenerator();
		Instance = this;
		Instance.GetNode<AnimatedSprite2D>("SpeedLines").Modulate = new Color(1, 1, 1, 0);

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

		GetNode<AnimatedSprite2D>("SpeedLines").SpeedScale = 1.0f / (float)Engine.TimeScale;
	}
	public static void SetShowingSpeedLines(bool visible) {
		Instance.GetNode<AnimatedSprite2D>("SpeedLines").Visible = visible;
	}

	public void SetLinesAlpha(float alpha) {
		Instance.GetNode<AnimatedSprite2D>("SpeedLines").Modulate = new Color(1, 1, 1, alpha);
		speedtransTween.SetSpeedScale(1/(float)Engine.TimeScale);
		SetShowingSpeedLines(Instance.GetNode<AnimatedSprite2D>("SpeedLines").Modulate.A > 0 ? true : false);
	}

	public async void TransitionLinesAlpha(float linesAlpha, float transTime) {
		if (speedtranslock)
		{
			EmitSignal(SignalName.SpeedLinesSuccess, true);
			speedtransTween.Kill();
			//EmitSignal(SignalName.SpeedLinesSuccess, false);
		}
		//else
		//{
			speedtranslock = true;
			speedtransTween = GetTree().CreateTween().BindNode(this);
			speedtransTween.SetSpeedScale((float)(1 / Engine.TimeScale));
			speedtransTween.TweenMethod(Callable.From<float>(SetLinesAlpha), Instance.GetNode<AnimatedSprite2D>("SpeedLines").Modulate.A, linesAlpha, transTime);
			await ToSignal(speedtransTween, "finished");
			speedtranslock = false;
			EmitSignal(SignalName.SpeedLinesSuccess, true);
		//}
	}
}
