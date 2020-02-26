using TMPro;
using UnityEngine;

public class CondRow : MonoBehaviour
{

    [SerializeField] private GameObject boolFieldsPrefab = null, intFieldsPrefab = null;
    [SerializeField] private Transform typeFieldsLoc = null;
    private BoolCondFields boolFields;
    private IntCondFields intFields;
    [SerializeField] private TMP_Dropdown varDropdown = null;
    private VarType type;
    private CondGroup condGroup;

    public string Serial {
        get {
            return null;
        }
    }

    public void Init(CondGroup condGroup)
    {
        this.condGroup = condGroup;
        varDropdown.options.Clear();
        varDropdown.options.AddRange(condGroup.VarNames);
        varDropdown.onValueChanged.AddListener(VarChanged);
    }

    public void VarChanged(int index)
    {
        var varName = varDropdown.options[index].text;
        var lastType = type;
        type = condGroup.GetVarType(index);
        if (lastType != type) {
            if (lastType == VarType.Bool)
                boolFields.gameObject.SetActive(false);
            else if (lastType == VarType.Int)
                intFields.gameObject.SetActive(false);
        }

        // TODO: Fix 
        /*
        if (type == VarType.Bool) {
            var caseBool = condGroup.GetBool(varName);

            if (boolFields == null || boolFields.Cond.VarKey != caseBool.Serial) {
                if (boolFields != null)
                    boolFields.ConfirmDelete();

                var fields = Instantiate(boolFieldsPrefab, typeFieldsLoc);
                boolFields = fields.GetComponent<BoolCondFields>();
                boolFields.Init(condGroup, caseBool.Serial);
            } else {
                boolFields.gameObject.SetActive(true);
            }
        } else if (type == VarType.Int) {
            var caseInt = condGroup.GetInt(varName);

            if (intFields == null || intFields.Cond.VarKey != caseInt.Serial) {
                if (intFields != null)
                    intFields.ConfirmDelete();

                var fields = Instantiate(intFieldsPrefab, typeFieldsLoc);
                intFields = fields.GetComponent<IntCondFields>();
                intFields.Init(condGroup, caseInt.Serial);
            } else {
                intFields.gameObject.SetActive(true);
            }
        }*/
    }


    public void SetBoolCond(BoolConditional cond)
    {
        var caseVar = VarData.GetCaseBool(cond.VarKey);

        if (caseVar == null) {
            // TODO: Fix
            //CondGroup.DeleteBoolCond(cond.Serial);
            Destroy(gameObject);
            return;
        }

        type = cond.VarType;
        varDropdown.interactable = false;
        varDropdown.options[0] = new TMP_Dropdown.OptionData(caseVar.Name);

        var fields = Instantiate(boolFieldsPrefab, typeFieldsLoc);
        boolFields = fields.GetComponent<BoolCondFields>();
        boolFields.Init(condGroup, cond);
    }
    public void SetIntCond(IntConditional cond)
    {
        var caseVar = VarData.GetCaseInt(cond.VarKey);

        if (caseVar == null) {
            // TODO: Fix
            //condGroup.DeleteIntCond(cond.Serial);
            Destroy(gameObject);
            return;
        }

        type = cond.VarType;
        varDropdown.interactable = false;
        varDropdown.options[0] = new TMP_Dropdown.OptionData(caseVar.Name);

        var fields = Instantiate(intFieldsPrefab, typeFieldsLoc);
        intFields = fields.GetComponent<IntCondFields>();
        intFields.Init(condGroup, cond);
    }

    public void Apply()
    {
        if (type == VarType.Bool)
            boolFields.Apply();
        else if (type == VarType.Int)
            intFields.Apply();
    }

    public void Delete()
    {
        GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"), GameObject.Find("GaudyBG").transform) as GameObject;
        panel.name = "ConfirmActionBG";
        ConfirmationScript cs = panel.GetComponent<ConfirmationScript>();

        cs.obj = gameObject;
        cs.actionText.text = "Are you sure you want to remove this entry?";
        cs.MethodToConfirm = delegate {
            if (type == VarType.Bool) {
                boolFields.ConfirmDelete();
            } else if (type == VarType.Int) {
                intFields.ConfirmDelete();
            }

            condGroup.CondRows.Remove(this);
            Destroy(gameObject);
        };
    }
}
