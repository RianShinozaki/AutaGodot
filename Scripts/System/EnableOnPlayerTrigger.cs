using System;
using Godot;

public partial class EnableOnPlayerTrigger : Area2D {
    [Export]
    Node2D[] Enables = new Node2D[0];

    [Export]
    bool Invert;

    //TODO: Replace this with static player instance get
    [Export]
    Node2D ReferenceObject;

    ProcessModeEnum[] originals;

    public override void _Ready() {
        base._Ready();

        originals = new ProcessModeEnum[Enables.Length];

        for (int i = 0; i < Enables.Length; i++) {
            originals[i] = Enables[i].ProcessMode;
        }

        AreaEntered += Entered;
        AreaExited += Exited;

        SetDisabled();
    }

    void Entered(Area2D area) {
        if (area == ReferenceObject) {
            SetEnabled();
        }
    }

    void Exited(Area2D area) {
        if (area == ReferenceObject) {
            SetDisabled();
        }
    }


    void SetDisabled() {
        for(int i = 0; i < Enables.Length; i++) {
            Enables[i].ProcessMode = ProcessModeEnum.Disabled;
            Enables[i].Visible = false;
        }
    }

    void SetEnabled() {
        for(int i = 0; i < Enables.Length; i++) {
            Enables[i].ProcessMode = originals[i];
            Enables[i].Visible = true;
        }
    }
}
