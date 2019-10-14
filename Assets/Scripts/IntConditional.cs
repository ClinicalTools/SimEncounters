
public enum IntConditionalOperator
{
    Equals, NotEquals, LessThan, GreaterThan
}
public class IntConditional : CaseConditional<int>
{
    public IntConditionalOperator Operator { get; set; }

    protected override bool CheckVal(int val)
    {
        switch (Operator) {
            case IntConditionalOperator.Equals:
                return val == Value;
            case IntConditionalOperator.NotEquals:
                return val != Value;
            case IntConditionalOperator.LessThan:
                return val < Value;
            case IntConditionalOperator.GreaterThan:
                return val > Value;
            default:
                return true;
        }
    }

    protected override CaseVariable<int> GetVar()
    {
        throw new System.NotImplementedException();
    }
}
