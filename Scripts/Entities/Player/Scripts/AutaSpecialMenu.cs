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
		if (Input.IsActionPressed("SpecialMenu") && !menuActive && !menuLock)
		{
			CallDeferred("AcquireLock");
			GameManager.Instance.TSChangeSuccess += onTSChangeSuccess;
			GameManager.Instance.TransitionTimeScale(0.001f, 0.25f);
		}
		if (!Input.IsActionPressed("SpecialMenu") && menuActive && !menuLock)
		{
			CallDeferred("AcquireLock");
			GameManager.Instance.TSChangeSuccess += onTSChangeSuccess;
			GameManager.Instance.TransitionTimeScale(1, 0.25f);
		}

		if (Input.IsActionPressed("1"))
		{
			Engine.TimeScale = 0.5;
		}
		if(Input.IsActionPressed("2") ) {
			Engine.TimeScale = 0.5;
		}
	}
	
	private void onTSChangeSuccess(bool success) {
		GameManager.Instance.TSChangeSuccess -= onTSChangeSuccess;
		if(success) menuActive = !menuActive;
		CallDeferred("ReleaseLock");
	}
	
	private void AcquireLock() {
		menuLock = true;
	}
	private void ReleaseLock() {
		menuLock = false;
	}
	
	
}
