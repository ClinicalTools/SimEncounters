using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseViewEncounterSelector : BaseEncounterSelector
    {
        public abstract string ViewName { get; set; }
        public abstract Sprite ViewSprite { get; set; }
    }
}