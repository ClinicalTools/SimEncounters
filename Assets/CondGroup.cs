using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CondGroup : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    public GameObject RowPrefab => rowPrefab;

    public Transform Transform { get; set; }

    public List<CondRow> CondRows { get; } = new List<CondRow>();

    public List<string> ConditionSerials()
    {
        var serials = new List<string>();

        foreach (var condRow in CondRows)
            serials.Add(condRow.Serial);

        return serials;
    }


    private static List<string> delBoolConds;
    public static void DeleteBoolCond(string serial)
    {
        delBoolConds.Add(serial);
    }

    public IEnumerable<TMP_Dropdown.OptionData> VarNames { get; private set; }
    private Dictionary<string, EncounterBool> boolVars;
    public EncounterBool GetBool(string name)
    {
        return boolVars[name];
    }


    private List<string> delIntConds;
    public void DeleteIntCond(string serial)
    {
        delIntConds.Add(serial);
    }

    private Dictionary<string, EncounterInt> intVars;
    public EncounterInt GetInt(string name)
    {
        return intVars[name];
    }

    private void Awake()
    {
        Setup();
    }

    private bool setup = false;
    public void Setup()
    {
        if (setup)
            return;

        delBoolConds = new List<string>();
        delIntConds = new List<string>();

        var names = new List<TMP_Dropdown.OptionData>();
        names.Add(new TMP_Dropdown.OptionData(" "));

        boolVars = new Dictionary<string, EncounterBool>();
        intVars = new Dictionary<string, EncounterInt>();
        var boolNames = new List<string>();
        foreach (var boolVar in VarData.BoolVars) {
            boolVars.Add(boolVar.Name, boolVar);
            boolNames.Add(boolVar.Name);
        }
        boolNames.Sort();
        foreach (var name in boolNames)
            names.Add(new TMP_Dropdown.OptionData(name));

        var intNames = new List<string>();
        foreach (var intVar in VarData.IntVars) {
            intVars.Add(intVar.Name, intVar);
            intNames.Add(intVar.Name);
        }
        intNames.Sort();
        foreach (var name in intNames)
            names.Add(new TMP_Dropdown.OptionData(name));
        
        VarNames = names;

        setup = true;
    }

    public VarType GetVarType(int varNameIndex)
    {
        if (varNameIndex == 0)
            return null;

        if (varNameIndex <= boolVars.Count)
            return VarType.Bool;
        varNameIndex -= boolVars.Count;
        
        if (varNameIndex <= intVars.Count)
            return VarType.Int;

        return null;
    }

    public void AddCond()
    {
        var condRow = Instantiate(RowPrefab, transform).GetComponent<CondRow>();
        condRow.Init(this);
        CondRows.Add(condRow);
    }

    public void SetConditions(List<string> keys)
    {
        Setup();

        foreach (var key in keys) {
            var type = VarType.GetType(key);

            if (type == null)
                continue;

            var condRow = Instantiate(RowPrefab, transform).GetComponent<CondRow>();
            condRow.Init(this);
            CondRows.Add(condRow);
            if (type == VarType.Bool) {
                condRow.SetBoolCond(CondData.GetBoolCond(key));
            } else if (type == VarType.Int) {
                condRow.SetIntCond(CondData.GetIntCond(key));
            }
        }
    }

    public void Apply()
    {
        foreach (var serial in delBoolConds)
            CondData.RemoveBoolCond(serial);

        foreach (var serial in delIntConds)
            CondData.RemoveIntCond(serial);

        foreach (var condRow in CondRows)
            condRow.Apply();
    }
}
