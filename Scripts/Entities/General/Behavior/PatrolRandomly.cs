using Godot;
using System;

[Tool] [GlobalClass]
public partial class PatrolRandomly : StateScript
{
	[Export] public int patrolRange;
	[Export] public Vector2 waitTimeRange;
	[Export] public Vector2 moveTimeRange;
	float t = 0;
	public Vector2 initPos;
	public float currentWaitTime;
	public float currentMoveTime;
	public bool moving;
	RandomNumberGenerator rand;
	AccelAndDecel acdc;

    public override void Init()
    {
        base.Init();
		initPos = GlobalPosition;
		rand = new RandomNumberGenerator();
		currentWaitTime = rand.RandfRange(waitTimeRange.X, waitTimeRange.Y);
		currentMoveTime = rand.RandfRange(moveTimeRange.X, moveTimeRange.Y);
		acdc = GetParent().GetNode<AccelAndDecel>("AccelAndDecel");
    }

    public override void _Process(double delta)
	{
		if(Engine.IsEditorHint()) return;
		if(!active) return;

		t += (float)delta;
		if(t > currentWaitTime && !moving) {
			moving = true;
			currentMoveTime = rand.RandfRange(moveTimeRange.X, moveTimeRange.Y);
			t = 0;

			if(GlobalPosition.X + currentMoveTime*acdc.speed > initPos.X + patrolRange)
				acdc.hor = -1;
			else if(GlobalPosition.X - currentMoveTime*acdc.speed < initPos.X - patrolRange)
				acdc.hor = 1;
			else {
				int newHor = rand.RandiRange(0, 1);
				GD.Print(newHor);
				newHor = (newHor*2)-1;
				acdc.hor = newHor;
			}

		}
		if(t > currentMoveTime && moving) {
			acdc.hor = 0;
			moving = false;
			t = 0;
			currentWaitTime = rand.RandfRange(moveTimeRange.X, moveTimeRange.Y);
		}
	}

	public override void _Draw()
	{
		if(Engine.IsEditorHint()) DrawLine( GlobalPosition - new Vector2(patrolRange, 0f), GlobalPosition + new Vector2(patrolRange, 0f), Colors.Green, 1.0f);
	}
}
