using Godot;
using System;

[GlobalClass]
public partial class AnimPlayerSignalHelper : Node
{
	[Export] public bool active = true;
	[Export] public AnimationPlayer anim;
	[Export] public string animName;
	private void _on_signal()
	{
		if (active)
		{
			anim.Play(animName);
		}
	}
}
