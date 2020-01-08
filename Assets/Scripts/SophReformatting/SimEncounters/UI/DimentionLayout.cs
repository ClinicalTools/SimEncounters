using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.UI
{
    [Serializable]
    public class DimentionLayout
    {
        [field: SerializeField] public virtual int Min { get; set; } = -1;
        [field: SerializeField] public virtual int Preferred { get; set; } = -1;
        [field: SerializeField] public virtual int Max { get; set; } = -1;
    }
    [Serializable]
    public class DimensionLayoutGroup
    {
        [field: SerializeField] public virtual bool FitChild { get; set; } = true;
        [field: SerializeField] public virtual bool ControlChild { get; set; } = false;
        [field: SerializeField] public virtual bool ExpandChild { get; set; } = false;

        [field: SerializeField] public virtual DimentionLayout DimentionLayout { get; set; } = new DimentionLayout();
    }
}