using Godot;
using System;
using System.Collections.Generic;

public partial class SaveData : Resource
{
    public string NodePath { get; set; }
    public Dictionary<string, Variant> Data { get; set; }
}
