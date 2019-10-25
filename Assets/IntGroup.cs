public class IntGroup : VarGroup<IntRow, CaseInt, int>
{
    public static IntGroup Instance { get; private set; }
    public void Awake()
    {
        Instance = this;

        foreach (var intVar in VarData.IntVars)
            AddVar(intVar);
    }


    protected override void RemoveVar(string serial)
    {
        VarData.RemoveCaseInt(serial);
    }
}
