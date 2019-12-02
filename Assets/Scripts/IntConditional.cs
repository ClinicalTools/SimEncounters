
using System;

public enum IntComparator
{
    Equals, NotEquals, LessThan, GreaterThan
}
public class IntConditional : CaseConditional<int>
{
    public override VarType VarType => VarType.Int;

    public IntComparator Comparator { get; set; }

    public IntConditional(string varSerial) : base(varSerial) { }
    public IntConditional(string varSerial, int value, IntComparator op) : base(varSerial, value) {
        Comparator = op;
    }

    protected override bool CheckVal(int val)
    {
        switch (Comparator) {
            case IntComparator.Equals:
                return val == Value;
            case IntComparator.NotEquals:
                return val != Value;
            case IntComparator.LessThan:
                return val < Value;
            case IntComparator.GreaterThan:
                return val > Value;
            default:
                return true;
        }
    }

    protected override EncounterVariable<int> GetVar()
    {
        throw new NotImplementedException();
    }

    public static bool TryParseOperator(string val, out IntComparator result)
    {
        bool success = Enum.TryParse(val, out result);
        if (success)
            return true;

        // If unsuccessful, perform any conversions for possible old names of enum members to keep old data compatible
        return false;
    }
}
