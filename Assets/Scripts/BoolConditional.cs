public class BoolConditional : CaseConditional<bool>
{
    public override VarType VarType => VarType.Bool;

    public BoolConditional(string varSerial) : base(varSerial) { }
    public BoolConditional(string serial, string varSerial, bool value) : base(serial, varSerial, value) { }

    protected override bool CheckVal(bool val)
    {
        return (val == Value);
    }

    protected override CaseVariable<bool> GetVar()
    {
        throw new System.NotImplementedException();
    }
}
