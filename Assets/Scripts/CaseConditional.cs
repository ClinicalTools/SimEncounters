public abstract class CaseConditional<T>
{
    protected readonly SerialScript serial = new SerialScript();
    public virtual string Serial {
        get {
            if (string.IsNullOrEmpty(serial.GetSerial()))
                serial.GenerateSerial(null);

            return serial.GetSerial();
        }
        set {
            serial.SetSerial(value);
        }
    }
    public string VarSerial { get; set; }
    public T Value { set; get; }

    protected abstract bool CheckVal(T val);
    protected abstract CaseVariable<T> GetVar();
    public virtual bool ConditionMet()
    {
        var caseVar = GetVar();
        if (caseVar == null)
            return true;
        else
            return (CheckVal(caseVar.Value));
    }
}
