using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterUI : SceneUI
    {
        [field: SerializeField] public virtual Button VariablesButton { get; set; }
        [field: SerializeField] public virtual Button SaveAndViewButton { get; set; }
        [field: SerializeField] public virtual Button SaveButton { get; set; }
        [field: SerializeField] public virtual Button MainMenuButton { get; set; }
        [field: SerializeField] public virtual Button ExitButton { get; set; }
        [field: SerializeField] public virtual Button HelpButton { get; set; }

        [field: SerializeField] public virtual SectionsUI Sections { get; set; }
    }
}