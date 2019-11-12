public abstract class EncounterVariable<T>
{
    public virtual string Name { get; set; }

    public abstract VarType VarType { get; }

    public virtual T Value { get; set; }

    public EncounterVariable() { }

    public EncounterVariable(string name, T value)
    {
        Name = name;
        Value = value;
    }
}
