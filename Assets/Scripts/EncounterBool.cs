public class EncounterBool : EncounterVariable<bool>
{
    public override VarType VarType => VarType.Bool;

    public EncounterBool() : base() { }
    public EncounterBool(string name, bool value) : base(name, value) { }
}
