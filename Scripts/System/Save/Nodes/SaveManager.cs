using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
[GlobalClass]
public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }

    /// <summary>
    /// This is the identifier for the version of the save file format.
    /// Please change this after every revision to maintain compatability!
    /// </summary>
    [Export]
    public short SaveFileRevision;

    /// <summary>
    /// Override this if you want a custom file extension
    /// </summary>
    [Export]
    public string SaveFileExtension = "bin";

    /// <summary>
    /// This is a string name associated with a level in the game.
    /// </summary>
    [Export]
    public string[] Levels;

    /// <summary>
    /// This is the index we use to track the current level.
    /// Update this by calling LevelChange(int levelId) or LevelChange(string level)
    /// </summary>
    private int _levelId = 0;

    private int _saveSlot = 0;

    public override void _Ready()
    {
        if (Instance != null) {
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void LevelChange(int levelId) => _levelId = levelId;

    public void LevelChange(string level) {
        for (int i = 0; i < Levels.Length; i++) {
            if (Levels[i].ToLower().Trim() == level.ToLower().Trim()) { 
                _levelId = i;
                return;
            }
        }
        GD.PrintErr($"Unable to find level \"{level}\"");
    }

    public void SaveData(SaveDataLayer layer) {
        ISaveable[] saveables = SaveUtils.FindAllSaveables(GetTree().Root).ToArray();

        string FilePath = $"user://Saves/{nameof(layer)}.{_saveSlot}.{SaveFileExtension}";

        if (layer == SaveDataLayer.Local)
        {
            FilePath = $"user://Saves/{Levels[_levelId]}.{_saveSlot}.{SaveFileExtension}";
        }

        FileAccess file = FileAccess.Open(FilePath + ".tmp", FileAccess.ModeFlags.Write);
        

        SaveFile saveFile = new SaveFile()
        {
            SaveFileRevision = SaveFileRevision,
            DataLayer = (short)layer,
        };

        List<SaveData> objData = new List<SaveData>();

        foreach (var saveable in saveables) {
            if (saveable == null) {
                GD.PrintErr("Saveable is null, Skipping...");
                continue;
            }
            if (saveable is SaveLoadComponent comp) {
                if (comp.DataLayer != layer) {
                    continue;
                }
                objData.Add(comp.Save());
            }
        }

        DirAccess dir = DirAccess.Open("user://");

        if (!file.StoreVar(saveFile)) {
            GD.PrintErr("Error when writing to file!");
            file.Close();
            dir.Remove(FilePath + ".tmp");
            
        }
        file.Close();

        dir.Remove(FilePath);
        dir.Rename(FilePath + ".tmp", FilePath);
    }

    public void LoadData(SaveDataLayer layer, int save) {
        string FilePath = $"user://Saves/{nameof(layer)}.{_saveSlot}.{SaveFileExtension}";

        if (layer == SaveDataLayer.Local)
        {
            FilePath = $"user://Saves/{Levels[_levelId]}.{_saveSlot}.{SaveFileExtension}";
        }

        FileAccess file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);

        var savegame = file.GetVar();

        if (savegame.Obj is not SaveFile || (savegame.Obj as SaveFile).SaveFileRevision > SaveFileRevision) {
            GD.PrintErr("File is not valid save game!");
            return;
        }

        foreach (SaveData svdata in (savegame.Obj as SaveFile).SavedData) { 
            string nodepath = svdata.NodePath;

            Node node = GetNode(nodepath);

            for (int i = 0; i < svdata.Data.Count; i++) {
                node.Set(svdata.Data.Keys.ToArray()[i], svdata.Data.Values.ToArray()[i]);
            }
        }
    }
}
