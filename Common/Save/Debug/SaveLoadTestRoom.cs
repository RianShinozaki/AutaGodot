using Godot;
using System;

public partial class SaveLoadTestRoom : Control
{
    public bool AutaDead;
    public int HP;
    public Vector2 position = Vector2.Zero;

    private int TestsPassed = 0;
    public override void _Ready()
    {
        PopulateSaveData();
    }

    private void PopulateSaveData() {
        //Test 1
        AutaDead = true;
        HP = 69;
        position = Vector2.Down;
        SaveManager.Instance.SaveData(SaveDataLayer.Settings);

        //Test 2
        AutaDead = false;
        HP = 90;
        position = Vector2.Up;
        SaveManager.Instance.SaveData(SaveDataLayer.Global);

        //Test 3
        AutaDead = true;
        HP = 32;
        position = Vector2.Left;
        SaveManager.Instance.SaveData(SaveDataLayer.Local);
    }

    public void TestSaves() {
        TestsPassed = 0;

        AutaDead = false;
        HP = 0;
        position = Vector2.Zero;

        //Test 1
        SaveManager.Instance.LoadData(SaveDataLayer.Settings, 0);
        if (AutaDead && HP == 69 && position == Vector2.Down) { 
            TestsPassed++;
        }
        GD.Print($"Passed {TestsPassed}/3 Tests");

        //Test 2
        SaveManager.Instance.LoadData(SaveDataLayer.Global, 0);
        if (!AutaDead && HP == 90 && position == Vector2.Up)
        {
            TestsPassed++;
        }
        GD.Print($"Passed {TestsPassed}/3 Tests");

        //Test 3
        SaveManager.Instance.LoadData(SaveDataLayer.Local, 0);
        if (AutaDead && HP == 32 && position == Vector2.Left)
        {
            TestsPassed++;
        }
        GD.Print($"Passed {TestsPassed}/3 Tests");
    }
}
