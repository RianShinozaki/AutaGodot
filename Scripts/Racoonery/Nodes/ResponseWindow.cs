using Godot;
using System;
using System.Collections.Generic;
[GlobalClass]
public partial class ResponseWindow : FlowContainer
{
    public static ResponseWindow Instance;

    public delegate void Responded(int choice, EventArgs args);
    public static event Responded OnResponded;

    private List<OptionButton> OptionButtons = new List<OptionButton>();

    public override void _Ready()
    {
        if (Instance != null) {
            QueueFree();
            return;
        }
        Instance = this;
    }
    public void SetChoices(string[] choices) 
    {
        if (GetChildren().Count > 0) {
            foreach (Node child in GetChildren()) {
                child.QueueFree();
            }
        }

        OptionButton.Selected _evntHandler = null;

        _evntHandler = (s, e) => {
            OnResponded.Invoke((int)s, e);

            foreach (OptionButton oBtn in OptionButtons)
            {
                oBtn.OnSelected -= _evntHandler;
                oBtn.QueueFree();
            }
        };

        for (int i = 0; i < choices.Length; i++) {
            OptionButton btn = new OptionButton(i);
            btn.Text = choices[i];

            btn.OnSelected += _evntHandler;

            AddChild(btn);
        }
    }
}
