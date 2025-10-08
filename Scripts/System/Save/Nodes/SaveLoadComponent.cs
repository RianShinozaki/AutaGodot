using Godot;
using System;
using System.Collections.Generic;
[GlobalClass]
public partial class SaveLoadComponent : Node, ISaveable
{
    [Export]
    public SaveDataLayer DataLayer { get; set; }

    [Export]
    public string[] Objects { get; set; }

    public SaveData Save() {
        if (Objects == null || Objects.Length == 0) {
            GD.PrintErr("Error when creating \"SaveData\":\nObjects either not present or null!");
        }

        SaveData data = new SaveData() { 
            NodePath = GetParent().GetPath().ToString() 
        };

        Node parent = GetParent();

        foreach (var obj in Objects) {
            var variable = parent.Get(obj);

            if (variable.AsGodotObject() == null) {
                GD.PrintErr("Error when creating \"SaveData\":Variable is not GodotObject! Skipping...\n");
                continue;
            }

            data.Data[obj] = variable;
        }

        return data;
    }
    public void Load(SaveData Save) {
        if (Save == null || Save.Data.Count == 0)
        {
            GD.PrintErr("Error when loading:\nSave file is null or corrupt!");
        }

        SaveData data = new SaveData()
        {
            NodePath = GetParent().GetPath().ToString()
        };

        Node parent = GetParent();

        foreach (var key in Save.Data.Keys)
        {
            if (Save.Data[key].Obj == null) {
                GD.PrintErr("Error when loading:\nVariable is null or corrupt! Skipping...");
                return;
            }
            parent.Set(key, Save.Data[key]);
        }

        GD.Print("Save data loaded for " + parent.Name.ToString());
    }
}
