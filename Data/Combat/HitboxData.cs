using Godot;
using System;

[GlobalClass]
public partial class HitboxData : Resource
{
	[Export] public float damage;
	[Export] public float hitstun; //In seconds
	[Export] public float xKnockback;
	[Export] public float yKnockback;
	[Export] public bool flip;
	[Export] public int weight;

}
