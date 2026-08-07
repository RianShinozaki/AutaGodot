using Godot;
using System;

public partial class SaveFile : Resource
{
    public short SaveFileRevision { get; set; }
    public short DataLayer { get; set; }
    public SaveData[] SavedData { get; set; }
}
