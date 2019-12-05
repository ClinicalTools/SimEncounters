using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class Icon
    {
        public virtual Color Color { get; }
        public virtual string Reference { get; }
        public Icon(Color color, string reference)
        {
            Color = color;
            Reference = reference;
        }
    }
}