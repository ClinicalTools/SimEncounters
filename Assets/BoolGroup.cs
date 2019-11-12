public class BoolGroup : VarGroup<BoolRow, EncounterBool, bool>
{
    public static BoolGroup Instance { get; private set; }
    public void Awake()
    {
        Instance = this;

        foreach (var boolVar in VarData.BoolVars)
            AddVar(boolVar);
    }

    protected override void RemoveVar(string serial)
    {
        VarData.RemoveCaseBool(serial);
    }
}
