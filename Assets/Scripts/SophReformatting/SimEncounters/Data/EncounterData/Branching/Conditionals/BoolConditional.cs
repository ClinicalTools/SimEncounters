public class BoolConditional : CaseConditional<bool>


{
    public override VarType VarType => VarType.Bool;

    public BoolConditional(string varSerial) : base(varSerial) { }
    public BoolConditional(string varSerial, bool value) : base(varSerial, value) { }
 
    protected override bool CheckVal(bool val)
    {
        return (val == Value);
    }

    protected override EncounterVariable<bool> GetVar()
    {
        throw new System.NotImplementedException();
    }
}
