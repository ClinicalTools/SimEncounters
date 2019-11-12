using System.Collections.Generic;
using UnityEngine;

public abstract class VarGroup<T, U, V> : MonoBehaviour
    where T : VarRow<U, V>
    where U : EncounterVariable<V>
{
    [SerializeField] private GameObject rowPrefab;
    public virtual GameObject RowPrefab => rowPrefab;
    public virtual Transform Group => transform;

    public virtual void AddVar()
    {
        var row = Instantiate(RowPrefab, Group);
        var component = row.GetComponent<T>();
        component.NewVal();
    }

    public virtual void AddVar(U caseVar)
    {
        var row = Instantiate(RowPrefab, Group);
        var component = row.GetComponent<T>();
        component.SetVal(caseVar);
    }

    public virtual void Apply()
    {
        foreach (var serial in delVars)
            RemoveVar(serial);

        foreach (var row in GetComponentsInChildren<T>())
            row.Apply();
    }

    private readonly List<string> delVars = new List<string>();
    public void DeleteVar(string serial)
    {
        delVars.Add(serial);
    }

    protected abstract void RemoveVar(string serial);
}
