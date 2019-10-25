
using System;

public enum IntConditionalOperator
{
    Equals, NotEquals, LessThan, GreaterThan
}
public class IntConditional : CaseConditional<int>
{
    public override VarType VarType => VarType.Int;

    public IntConditionalOperator Comparator { get; set; }

    public IntConditional(string varSerial) : base(varSerial) { }
    public IntConditional(string serial, string varSerial, int value, IntConditionalOperator op) : base(serial, varSerial, value) {
        Comparator = op;
    }

    protected override bool CheckVal(int val)
    {
        switch (Comparator) {
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
        throw new NotImplementedException();
    }

    public static bool TryParseOperator(string val, out IntConditionalOperator result)
    {
        bool success = Enum.TryParse(val, out result);
        if (success)
            return true;

        // If unsuccessful, perform any conversions for possible old names of enum members to keep old data compatible
        return false;
    }
}
