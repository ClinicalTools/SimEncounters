public abstract class CaseConditional<T>
{
    public abstract VarType VarType { get; }

    public string VarKey { get; set; }
    public T Value { set; get; }

    public CaseConditional(string varSerial)
    {
        VarKey = varSerial;
    }
    public CaseConditional(string varSerial, T value)
    {
        VarKey = varSerial;
        Value = value;
    }

    protected abstract bool CheckVal(T val);
    protected abstract EncounterVariable<T> GetVar();
    public virtual bool ConditionMet()
    {
        var caseVar = GetVar();
        if (caseVar == null)
            return true;
        else
            return CheckVal(caseVar.Value);
    }
}
