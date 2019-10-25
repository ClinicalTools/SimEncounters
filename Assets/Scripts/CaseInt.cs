public class CaseInt : CaseVariable<int>
{
    public override VarType VarType => VarType.Int;

    public CaseInt() : base() { }
    public CaseInt(string serial, string name, int value) : base(serial, name, value) { }
}
