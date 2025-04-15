using Godot;
using System;

[GlobalClass]
public partial class ProcessDeathEnemyBasic : StateScript
{

	[Signal]
	public delegate void DiedEventHandler();

    public override void Init()
    {
        base.Init();
		entity.GetNode<EntityHealth>("Attributes/EntityHealth").HealthChanged += onHealthChanged;
    }

	public void onHealthChanged(float delta, float total, int maxhHealth) {
		if(total <= 0) {
			Node2D fx = ObjectPool.Instance.Spawn("Burst") as Node2D;
			if(fx != null) {
				fx.GlobalPosition = new Vector2(entity.GlobalPosition.X, entity.GlobalPosition.Y);
			}
			EmitSignal(SignalName.Died);
			entity.QueueFree();
		}
	}
}
