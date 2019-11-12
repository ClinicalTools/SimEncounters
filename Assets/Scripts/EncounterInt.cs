public class EncounterInt : EncounterVariable<int>
{
    public override VarType VarType => VarType.Int;

    public EncounterInt() : base() { }
    public EncounterInt(string name, int value) : base(name, value) { }
}
