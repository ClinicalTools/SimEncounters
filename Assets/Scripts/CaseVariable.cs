public abstract class CaseVariable<T>
{
    public virtual string Name { get; set; }

    protected readonly SerialScript serial = new SerialScript();
    public virtual string Serial {
        get {
            if (string.IsNullOrEmpty(serial.GetSerial()))
                serial.GenerateSerial(VarData.Keys, VarType.Prefix);

            return serial.GetSerial();
        }
        protected set {
            serial.SetSerial(value);
            VarData.Keys.Add(value);
        }
    }


    public abstract VarType VarType { get; }

    public virtual T Value { get; set; }

    public CaseVariable() { }

    public CaseVariable(string serial, string name, T value)
    {
        Serial = serial;
        Name = name;
        Value = value;
    }
}
