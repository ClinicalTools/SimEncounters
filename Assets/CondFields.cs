using UnityEngine;

public abstract class CondFields<T, U> : MonoBehaviour
    where T : CaseConditional<U>
{
    public virtual T Cond { get; protected set; }
    protected virtual bool New { get; set; }
    protected virtual CondGroup CondGroup { get; set; }

    public virtual void Init(CondGroup condGroup, string varSerial)
    {
        CondGroup = condGroup;
        New = true;
    }

    public virtual void Init(CondGroup condGroup, T cond)
    {
        CondGroup = condGroup;
        Cond = cond;
        New = false;
    }


    // Confirmation dialogue not needed for conditionals
    public virtual void ConfirmDelete()
    {
        if (!New)
            Delete();

        Destroy(gameObject);
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
