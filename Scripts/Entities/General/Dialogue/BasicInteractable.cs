using Godot;
using System;

public partial class BasicInteractable : Area2D
{
	DialogueRunner runner;

	[Export] public bool conditionGrounded = true;

	public override void _Ready()
	{
		runner = GetNode<DialogueRunner>("DialogueRunner");

	}
	public void Trigger()
	{
		GameManager.SwitchGameState(GameState.Cutscene);
		DialogueManager.Instance.DialogueEnded += onDialogueEnded;
		PlayerController.Instance.SwitchState("CutsceneState");
		runner.OnStartDialogue();
	}
	private void onDialogueEnded()
	{
		GameManager.SwitchGameState(GameState.Play);
		PlayerController.Instance.SwitchState("NormalState");

	}
	
}
