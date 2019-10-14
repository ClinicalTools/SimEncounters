public class BoolConditional : CaseConditional<bool>
{
    protected override bool CheckVal(bool val)
    {
        return (val == Value);
    }

    protected override CaseVariable<bool> GetVar()
    {
        throw new System.NotImplementedException();
    }
}
