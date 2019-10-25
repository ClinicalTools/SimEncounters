public abstract class CaseConditional<T>
{
    public abstract VarType VarType { get; }

    private readonly SerialScript serial = new SerialScript();
    public virtual string Serial {
        get {
            if (string.IsNullOrEmpty(serial.GetSerial()))
                serial.GenerateSerial(CondData.Keys, VarType.Prefix);

            return serial.GetSerial();
        }
        protected set {
            serial.SetSerial(value);
            CondData.Keys.Add(value);
        }
    }

    public string VarSerial { get; set; }
    public T Value { set; get; }

    public CaseConditional(string varSerial)
    {
        VarSerial = varSerial;
    }
    public CaseConditional(string serial, string varSerial, T value)
    {
        Serial = serial;
        VarSerial = varSerial;
        Value = value;
    }

    protected abstract bool CheckVal(T val);
    protected abstract CaseVariable<T> GetVar();
    public virtual bool ConditionMet()
    {
        var caseVar = GetVar();
        if (caseVar == null)
            return true;
        else
            return CheckVal(caseVar.Value);
    }
}
