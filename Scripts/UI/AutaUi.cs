using Godot;
using System;

public partial class AutaUi : TextureRect
{
	public static AutaUi Instance;
	public TextureProgressBar healthBar;
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		Instance = this;
	}
	public override void _Ready(){
		PlayerController.Instance.GetNode<EntityHealth>("Attributes/EntityHealth").HealthChanged += onHealthChanged;
		healthBar = GetNode<TextureProgressBar>("HealthBar");
	}

	public void onHealthChanged(float delta, float total, int maxhHealth) {
		healthBar.Value = 100*total/maxhHealth;
	}
}
