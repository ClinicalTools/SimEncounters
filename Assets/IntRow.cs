using TMPro;
using UnityEngine;

public class IntRow : VarRow<EncounterInt, int>
{
    [SerializeField] private TMP_InputField nameField = null;
    [SerializeField] private TMP_InputField valField = null;

    public override void NewVal()
    {
        base.NewVal();
        CaseVar = new EncounterInt();
    }

    public override void SetVal(EncounterInt value)
    {
        base.SetVal(value);

        nameField.text = value.Name;
        valField.text = value.Value.ToString();
    }

    public override void Apply()
    {
        CaseVar.Name = nameField.text;
        if (int.TryParse(valField.text, out var val))
            CaseVar.Value = val;

        if (New)
            VarData.AddCaseInt(CaseVar);

        base.Apply();
    }

    protected override void Delete()
    {
        // TODO: Fix
        //IntGroup.Instance.DeleteVar(CaseVar.Serial);
    }

}
