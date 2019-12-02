using TMPro;
using UnityEngine;

public class IntCondFields : CondFields<IntConditional, int>
{
    [SerializeField] private TMP_InputField valField;
    [SerializeField] private TMP_Dropdown comparatorDropdown;

    public override void Init(CondGroup condGroup, string varSerial)
    {
        base.Init(condGroup, varSerial);
        Cond = new IntConditional(varSerial);
    }

    public override void Init(CondGroup condGroup, IntConditional cond)
    {
        base.Init(condGroup, cond);

        Cond = cond;
        valField.text = Cond.Value.ToString();
        comparatorDropdown.value = (int)Cond.Comparator;
    }

    public override void Apply()
    {
        Cond.Value = int.Parse(valField.text);
        Cond.Comparator = (IntComparator)comparatorDropdown.value;

        if (New)
            CondData.AddIntCond(Cond);

        base.Apply();
    }

    protected override void Delete()
    {
        // TODO: fix
        ///CondGroup.DeleteIntCond(Cond.Serial);
    }
}