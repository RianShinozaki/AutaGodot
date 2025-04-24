using Godot;
using Godot.Collections;
using System;
using System.Data;
using System.Linq;

[GlobalClass]
public partial class DialogueRunner : Control
{	
	[Export] private DialogueData dialogueData;
	[Export] private DialogueManager dialogueBox;
	[Export] private AudioStreamPlayer audioStreamPlayer;
	[Export] private ExternalVariablesDemo externalVariablesDemo;
	[Export] private AnimationPlayer anim;

	[ExportCategory("SFX Options")]
	[Export] private bool enableAudio = true;
	[Export(PropertyHint.Range, "1,5,1.0")] private float sfxFrequency = 3;

	private int charCounter = 0;

	public override void _Ready()
	{	
		dialogueBox.Initialize();
		//dialogueBox.AddExternalVariable(externalVariablesDemo);
		connectSignals();
	}

	public void OnStartDialogue() 
	{
		if (!dialogueBox.IsRunning()) 
		{
			dialogueBox.Start(dialogueData);
		} 
	}


	private void connectSignals() 
	{	
		dialogueBox.Connect("DialogueCharDisplayed", Callable.From(onCharacterDisplayed));
		dialogueBox.Connect("DialogueSignal", Callable.From<string>(x => onDialogueSignal(x)));
		dialogueBox.Connect("DialogueProceeded", Callable.From<string>((x) => charCounter = 0));
	}

	private void onDialogueSignal(string value) 
	{	
		switch (value) 
		{
			case "switchtothinking":
				anim.Play("ThinkingTrans");
				break;
			case "switchtoidle":
				anim.Play("Idle");
				break;
			default:
				break;
		}
	}


	private void onCharacterDisplayed() 
	{	
		if (enableAudio && charCounter % sfxFrequency == 0) 
		{
			audioStreamPlayer.Play();
		}

		charCounter++;
	}
}
