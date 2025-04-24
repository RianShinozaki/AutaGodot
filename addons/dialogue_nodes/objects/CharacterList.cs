using Godot;

[GlobalClass]
public partial class CharacterList : Resource
{
    [Export] public Godot.Collections.Array<Character> Characters;
}