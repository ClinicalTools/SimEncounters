using TMPro;
using UnityEngine;

public class IntRow : VarRow<CaseInt, int>
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField valField;

    public override void NewVal()
    {
        base.NewVal();
        CaseVar = new CaseInt();
    }

    public override void SetVal(CaseInt value)
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
        IntGroup.Instance.DeleteVar(CaseVar.Serial);
    }

}
