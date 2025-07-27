using Godot;
using System;

public enum DamageType {
	Slice,
	Bash,
	HeavyBash,
	Pierce,
}

[GlobalClass]
public partial class HitboxData : Resource
{
	[Export] public float damage;
	[Export] public float hitstun; //In seconds
	[Export] public float xKnockback;
	[Export] public float yKnockback;
	[Export] public DamageType damageType;
	[Export] public bool flip;
	[Export] public int weight;
	[Export] public string HitFX;
	[Export] public AudioStream impactSound;

}
