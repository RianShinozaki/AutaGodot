using Godot;
using System;

public partial class HitboxTrigger : Node
{
	[Export] public float triggerLifeTime = -1f;
	public bool active = true;
	[Export] Godot.Collections.Array<Area2D> ignoreAreas;
	public Area2D hurtbox;
	float triggerTime = 0;

	[Signal]
	public delegate void TriggerEventHandler();
	[Signal]
	public delegate void UnTriggerEventHandler();
    public override void _Ready()
	{
		hurtbox = GetNode<Area2D>("Area2D");
		hurtbox.AreaEntered += _on_hurtbox_area_entered;
		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (triggerTime > 0)
		{
			triggerTime = Mathf.MoveToward(triggerTime, 0, (float)delta);
			if (triggerTime == 0)
			{
				EmitSignal(SignalName.UnTrigger);
			}
		}
    }

	public virtual void _on_hurtbox_area_entered(Area2D area)
	{
		if (!active) return;
		if (ignoreAreas != null && ignoreAreas.Contains(area))
		{
			return;
		}
		if (area is Hitbox hitB)
		{
			HitboxData dat = hitB.hitboxData;
			EmitSignal(SignalName.Trigger);
			triggerTime = triggerLifeTime;
		}
	}
}
