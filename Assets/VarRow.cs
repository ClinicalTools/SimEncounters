using UnityEngine;

public abstract class VarRow<T, U> : MonoBehaviour
    where T : EncounterVariable<U>
{
    protected virtual T CaseVar { get; set; }
    protected virtual bool New { get; set; }
    public virtual void NewVal()
    {
        New = true;
    }

    public virtual void SetVal(T value)
    {
        CaseVar = value;
        New = false;
    }

    public virtual void ConfirmDelete()
    {

        GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"), GameObject.Find("GaudyBG").transform) as GameObject;
        panel.name = "ConfirmActionBG";
        ConfirmationScript cs = panel.GetComponent<ConfirmationScript>();

        cs.obj = gameObject;
        cs.actionText.text = "Are you sure you want to remove this entry?";
        cs.MethodToConfirm = delegate {
            if (!New)
                Delete();

            Destroy(gameObject);
        };
    }

    /// <summary>
    /// Delete the variable. 
    /// Only called if the variable is not new.
    /// </summary>
    protected abstract void Delete();
    public virtual void Apply()
    {
        New = false;
    }
}
