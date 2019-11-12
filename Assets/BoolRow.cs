using TMPro;
using UnityEngine;

public class BoolRow : VarRow<EncounterBool, bool>
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_Dropdown valDropdown;


    public override void NewVal()
    {
        base.NewVal();
        CaseVar = new EncounterBool();
    }

    public override void SetVal(EncounterBool value)
    {
        base.SetVal(value);
        nameField.text = CaseVar.Name;
        valDropdown.value = CaseVar.Value ? 1 : 0;
    }

    public override void Apply()
    {
        CaseVar.Name = nameField.text;
        CaseVar.Value = valDropdown.value == 1;

        if (New)
            VarData.AddCaseBool(CaseVar);

        base.Apply();
    }

    protected override void Delete()
    {
        // TODO: fix
        //BoolGroup.Instance.DeleteVar(CaseVar.Serial);
    } 
}
