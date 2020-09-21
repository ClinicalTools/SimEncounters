using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionSelector : ReaderSectionSelector
    {
        public virtual Transform Line { get => line; set => line = value; }
        [SerializeField] private Transform line;
        public override void Display(UserEncounter encounter)
        {
            base.Display(encounter);
            Line.SetAsLastSibling();
        }
    }
}