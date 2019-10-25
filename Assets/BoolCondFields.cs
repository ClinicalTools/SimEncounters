using TMPro;
using UnityEngine;

public class BoolCondFields : CondFields<BoolConditional, bool>
{
    [SerializeField] private TMP_Dropdown valDropdown;
    
    public override void Init(CondGroup condGroup, string varSerial)
    {
        base.Init(condGroup, varSerial);

        Cond = new BoolConditional(varSerial);
    }

    public override void Init(CondGroup condGroup, BoolConditional cond)
    {
        base.Init(condGroup, cond);

        Cond = cond;
        valDropdown.value = Cond.Value ? 1 : 0;
    }

    public override void Apply()
    {
        Cond.Value = valDropdown.value == 1;

        if (New)
            CondData.AddBoolCond(Cond);

        base.Apply();
    }

    protected override void Delete()
    {
        CondGroup.DeleteBoolCond(Cond.Serial);
    }
}