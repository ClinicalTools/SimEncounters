using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.UI
{
    [Serializable]
    public class Padding
    {
        [field: SerializeField] public virtual int Spacing { get; set; }

        [field: SerializeField] public virtual int Top { get; set; }
        [field: SerializeField] public virtual int Bottom { get; set; }
        [field: SerializeField] public virtual int Left { get; set; }
        [field: SerializeField] public virtual int Right { get; set; }
    }
}