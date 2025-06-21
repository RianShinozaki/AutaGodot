using Godot;
using System;

[GlobalClass]
public partial class PlayerInteract : StateScript
{
	public Godot.Collections.Array<BasicInteractable> interactables = new Godot.Collections.Array<BasicInteractable>();
	public override void Init()
	{
		base.Init();

	}

	public override void _Process(double delta)
	{
		if (!active) return;

		if (!(Input.IsActionJustPressed("ui_select") && interactables.Count != 0)) return;
		if (GameManager.gameState != GameState.Play) return;
		if (interactables[0].conditionGrounded && !entity.grounded) return;

			GD.Print("StartDialogue");
			interactables[0].Trigger();
	}

	private void _on_interaction_trigger_area_entered(Area2D area)
	{
		if (area is BasicInteractable interactable)
		{
			interactables.Insert(0, interactable);
		}
	}
	private void _on_interaction_trigger_area_exited(Area2D area)
	{
		if (area is BasicInteractable interactable)
		{
			interactables.Remove(interactable);
		}
	}
	


}
