using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [Serializable]
    public struct NullableFloat
    {
        // not the optimal name for a bool, but I wanted a struct with a null initial value
        [field: SerializeField] private bool notNull;
        [field: SerializeField] private float value;

        public NullableFloat(float? value)
        {
            notNull = value != null;
            if (notNull)
                this.value = (float)value;
            else
                this.value = 0;
        }

        public float? Value {
            get {
                if (notNull)
                    return value;

                return null;
            }
            set {
                notNull = value != null;
                if (notNull)
                    this.value = (float)value;
            }
        }
    }
}