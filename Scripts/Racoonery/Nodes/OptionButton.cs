using Godot;
using System;
[GlobalClass]
public partial class OptionButton : Button
{
    public OptionButton(int optionId) {
        _optionId = optionId;
    }
    public delegate void Selected(int id, EventArgs args);
    public event Selected OnSelected;

    private int _optionId;

    public override void _Pressed()
    {
        OnSelected.Invoke(_optionId, null);
    }
}
