using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class Icon
    {
        // The only reason icons have colors is to allow compatibility for old Clinical Encounters versions
        public virtual Color Color { get; }
        public virtual string Reference { get; }
        public Icon(Color color, string reference)
        {
            Color = color;
            Reference = GetUpdatedReference(reference);
        }

        protected virtual string GetUpdatedReference(string reference)
        {
            switch (reference) {
                case "IconPanel1":
                    return "person";
                case "IconPanel2":
                    return "clock";
                case "IconPanel3":
                    return "stethoscope";
                case "IconPanel4":
                    return "checklist";
                case "IconPanel5":
                    return "first-aid-kit";
                case "IconPanel6":
                    return "grid";
                default:
                    return "person";
            }
        }
    }
}