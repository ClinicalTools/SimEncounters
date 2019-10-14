public class CaseInt : CaseVariable<int>
{
    public override CaseVariableType VarType => CaseVariableType.Int;

    public override bool SetValue(string value)
    {
        var success = int.TryParse(value, out var val);
        Value = val;
        return success;
    }
}
