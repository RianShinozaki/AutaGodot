using Godot;
using Godot.Collections;

using System;

public partial class SaveData : Resource
{
    public string NodePath { get; set; }
    public Dictionary<string, Variant> Data { get; set; }
}
