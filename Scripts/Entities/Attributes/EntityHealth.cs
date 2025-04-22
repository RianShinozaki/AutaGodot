using Godot;
using System;

public partial class EntityHealth : Node
{
	public StateEntity entity;
	
	[Export] public float health;
	[Export] public int maxHealth;
	
	[Signal]
	public delegate void HealthChangedEventHandler(float delta, float total, int maxHealth);
	
	public override void _Ready()
	{
		entity = GetParent<Node2D>().GetParent<StateEntity>();
		health = maxHealth;
		try {
			GetNode<Label>("Label").Text = health.ToString();
		} catch(InvalidCastException) {}
		
	}

	public void ChangeHealth(float amount) {
		health += amount;
		EmitSignal(SignalName.HealthChanged, amount, health, maxHealth);
		try {
			GetNode<Label>("Label").Text = health.ToString();
		} catch(InvalidCastException) {}
	}
}
