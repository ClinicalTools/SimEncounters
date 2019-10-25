public class CaseBool : CaseVariable<bool>
{
    public override VarType VarType => VarType.Bool;


    public CaseBool() : base() { }
    public CaseBool(string serial, string name, bool value) : base(serial, name, value) { }
}
