using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.SimEncounters.UI
{
    public class BetterHorizontalLayoutGroup : UIBehaviour
    {
        [field: SerializeField] public virtual DimensionLayoutGroup Width { get; set; } = new DimensionLayoutGroup();
        [field: SerializeField] public virtual DimensionLayoutGroup Height { get; set; } = new DimensionLayoutGroup();

        [field: SerializeField] public virtual Padding Padding { get; set; } = new Padding();

        protected 
        public void UpdateDimensions()
        {
            if (Width.)
        }
    }
}