public class CaseBool : CaseVariable<bool>
{
    public override CaseVariableType VarType => CaseVariableType.Bool;

    public override bool SetValue(string value)
    {
        var success = bool.TryParse(value, out var val);
        Value = val;
        return success;
    }
}
