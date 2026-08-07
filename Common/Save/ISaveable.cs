using Godot;
using System;

public interface ISaveable
{
    public abstract SaveData Save();
    public abstract void Load(SaveData Save);
}
