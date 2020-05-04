
public class VarType
{
    public static VarType Bool { get; } = new VarType('b');
    public static VarType Int { get; } = new VarType('i');

    public char Prefix { get; }

    private VarType(char prefix)
    {
        Prefix = prefix;
    }

    public override bool Equals(object value)
    {
        if (value is VarType other) {
            return other.Prefix == Prefix;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Prefix.GetHashCode();
    }

    public static bool operator ==(VarType first, VarType second)
        => Equals(first, second);

    public static bool operator !=(VarType first, VarType second)
        => !(first == second);


    public static VarType GetType(char ch)
    {
        if (ch == Bool.Prefix) {
            return Bool;
        } else if (ch == Int.Prefix) {
            return Int;
        } else {
            return null;
        }
    }
    public static VarType GetType(string serial)
    {
        if (string.IsNullOrEmpty(serial))
            return null;
        else
            return GetType(serial[0]);
    }
}
