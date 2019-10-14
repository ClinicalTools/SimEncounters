public enum CaseVariableType
{
    Bool, Int
}
public abstract class CaseVariable<T>
{
    public virtual string Name { get; set; }

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

    public abstract CaseVariableType VarType { get; }

    public virtual T Value { get; set; }

    public abstract bool SetValue(string value);
    public virtual string GetValue()
    {
        return Value.ToString();
    }


}
