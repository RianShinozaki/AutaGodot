using Godot;
using System;

[GlobalClass, Tool]
public partial class ItemSelectionEvent : StoryEvent
{
    [Export]
    public string Key;
    [Export]
    public string[] Selections;

    private float _timer = 0f;
    private bool _visible = false;

    public override void Start()
    {
        Show();
        ResponseWindow.OnResponded += Selected;
    }

    public void Show()
    {
        ResponseWindow.Instance.SetChoices(Selections);
    }

    public void Selected(int index, EventArgs e)
    {
        ResponseWindow.OnResponded -= Selected;

        Racoonery.SetInt(Key, index);
        Racoonery.SetString(Key + "Name", Selections[index]);

        InvokeComplete();
    }
}
