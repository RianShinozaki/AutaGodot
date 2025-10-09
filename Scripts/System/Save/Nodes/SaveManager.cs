using Godot;
using Godot.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        string FilePath = $"user://Saves/{layer.ToString()}.{_saveSlot}.{SaveFileExtension}";

        if (layer == SaveDataLayer.Local)
        {
            FilePath = $"user://Saves/{Levels[_levelId]}.{_saveSlot}.{SaveFileExtension}";
        }

        DirAccess dir = DirAccess.Open("user://");

        if (!dir.DirExists("user://Saves/")) {
            dir.MakeDir("user://Saves/");
        }

        FileAccess file = FileAccess.Open(FilePath + ".tmp", FileAccess.ModeFlags.Write);
        

        List<SaveData> objData = new List<SaveData>();

        file.StoreVar(SaveFileRevision);
        file.StoreVar((short)layer);

        foreach (var saveable in saveables) {
            if (saveable == null) {
                GD.PrintErr("Saveable is null, Skipping...");
                continue;
            }
            if (saveable is SaveLoadComponent comp) {
                if (comp.DataLayer != layer) {
                    continue;
                }

                SaveData sv = comp.Save();

                file.StoreVar(sv.NodePath);
                file.StoreVar(sv.Data);
            }
        }
        file.Flush();
        file.Close();

        dir.Remove(FilePath);
        dir.Rename(FilePath + ".tmp", FilePath);
    }

    public void LoadData(SaveDataLayer layer, int save) {
        string FilePath = $"user://Saves/{layer.ToString()}.{_saveSlot}.{SaveFileExtension}";

        if (layer == SaveDataLayer.Local)
        {
            FilePath = $"user://Saves/{Levels[_levelId]}.{_saveSlot}.{SaveFileExtension}";
        }

        FileAccess file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);

        //var savegame = file.GetVar();
        var filerev = file.GetVar();
        short rv = (short)Convert.ToInt64(filerev.Obj);

        if (rv > SaveFileRevision)
        {
            GD.PrintErr("File is from a newer version!");
            return;
        }

        var dlayer = file.GetVar();
        short dl = (short)Convert.ToInt64(dlayer.Obj);

        while (!file.EofReached()) {
            if (file.GetPosition() + 4 > file.GetLength()) break;

            var nodepath = file.GetVar();

            if (nodepath.Obj.ToString() == null) {
                GD.PrintErr("Nodepath is invalid! Breaking...");
                return;
            }

            Node node = GetNode(nodepath.Obj as string);

            var dict = file.GetVar();

            if (dict.Obj is not Dictionary) {
                GD.PrintErr("Dictionary is corrupt! Breaking...");
                return;
            }

            foreach (string key in (dict.Obj as Dictionary).Keys) {
                node.Set(key, (dict.Obj as Dictionary)[key]);
            }
        }
    }
}
