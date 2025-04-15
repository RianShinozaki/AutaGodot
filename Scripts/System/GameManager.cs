using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	public static float gameSpeed = 1.0f;
	public bool timeLock;
	
	[Signal]
	public delegate void TSChangeSuccessEventHandler(bool success);
	Tween timeScaleTween;
	
	public override void _Ready()
	{
		Instance = this;
	}

	public async void FrameFreeze(float timeScale, float duration) {
		if(timeLock) EmitSignal(SignalName.TSChangeSuccess, false);
		else {
			timeLock = true;
			Engine.TimeScale = timeScale;
			await ToSignal(GetTree().CreateTimer(duration * timeScale), "timeout");
			Engine.TimeScale = gameSpeed;
			timeLock = false;
			EmitSignal(SignalName.TSChangeSuccess, true);
		}
	}
	public async void TransitionTimeScale(float timeScale, float transTime) {
		GD.Print("Recieved request to change speed to, " + timeScale.ToString());
		if(timeLock) {
			EmitSignal(SignalName.TSChangeSuccess, false);
			GD.Print("Request failed");
		}
		else {
			timeLock = true;
			timeScaleTween = GetTree().CreateTween().BindNode(this);
			timeScaleTween.SetSpeedScale( (float)(1/Engine.TimeScale));
			timeScaleTween.TweenMethod(Callable.From<float>(SetGameSpeed), Engine.TimeScale, timeScale, transTime);
			await ToSignal(timeScaleTween, "finished");
			timeLock = false;
			EmitSignal(SignalName.TSChangeSuccess, true);
			GD.Print("Request succeeded");
		}
	}
	public void SetGameSpeed(float timeScale) {
		Engine.TimeScale = timeScale;
		timeScaleTween.SetSpeedScale(1/timeScale);
	}
}
