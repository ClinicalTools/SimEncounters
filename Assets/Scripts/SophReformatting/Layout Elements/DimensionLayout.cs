using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [Serializable]
    public class DimensionLayout : IDimensionLayout
    {
        public event Action ValueChanged;

        [SerializeField] private NullableFloat min;
        public float? Min {
            get { return min.Value; }
            set {
                if (value > Max)
                    min.Value = (float)Max;
              if (value > Max)
                    min.Value = (float)Max;
                else if (value < 0)
                    min.Value = 0;
                else
                    min.Value = value;
                ValueChanged?.Invoke();
            }
        }

        [SerializeField] private NullableFloat preferred;
        public float? Preferred {
            get { return preferred.Value; }
            set {
                if (value > Max)
                    preferred.Value = (float)Max;
                else if (value < Min)
                    preferred.Value = (float)Min;
                else if (value < 0)
                    preferred.Value = 0;
                else 
                    preferred.Value = value;
                ValueChanged?.Invoke();
            }
        }

        [SerializeField] private NullableFloat max;
        public float? Max {
            get { return max.Value; }
            set {
                if (value < Min)
                    max.Value = (float)Min;
                else if (value < 0)
                    max.Value = 0;
                else
                    max.Value = value;
                ValueChanged?.Invoke();
            }
        }
    }
}