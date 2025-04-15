using Godot;
using System;

[GlobalClass]
public partial class AutaSpecialMenu : StateScript
{
	public bool menuActive = false;
	bool menuLock = false;
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("SpecialMenu") && !menuActive && !menuLock) {
			GD.Print("Slowing, current timescale is ", Engine.TimeScale);
			CallDeferred("AcquireLock");
			GameManager.Instance.TSChangeSuccess += onTSChangeSuccess;
			GameManager.Instance.TransitionTimeScale(0.001f, 0.25f);
		}
		if(!Input.IsActionPressed("SpecialMenu") && menuActive && !menuLock) {
			GD.Print("SpeedingUp, current timescale is ", Engine.TimeScale);
			CallDeferred("AcquireLock");
			GameManager.Instance.TSChangeSuccess += onTSChangeSuccess;
			GameManager.Instance.TransitionTimeScale(1, 0.25f);
		}
	}
	
	private void onTSChangeSuccess(bool success) {
		GD.Print("Got Success Start");
		GameManager.Instance.TSChangeSuccess -= onTSChangeSuccess;
		if(success) menuActive = !menuActive;
		CallDeferred("ReleaseLock");
		GD.Print("Got Success End");
	}
	
	private void AcquireLock() {
		menuLock = true;
	}
	private void ReleaseLock() {
		menuLock = false;
	}
	
	
}
