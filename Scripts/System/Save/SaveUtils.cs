using Godot;

using System;
using System.Collections.Generic;

public class SaveUtils
{
    public static List<ISaveable> FindAllSaveables(Node root)
    {
        List<ISaveable> saveables = new List<ISaveable>();
        Recurse(root, saveables);
        return saveables;
    }

    private static void Recurse(Node node, List<ISaveable> list)
    {
        if (node is ISaveable saveable) {
            list.Add(saveable);
        }

        foreach (Node child in node.GetChildren()) {
            Recurse(child, list);
        }
    }
}

public enum SaveDataLayer {
    Settings,
    Global,
    Local,
}
