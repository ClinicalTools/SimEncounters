using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterUI : MonoBehaviour
    {
        [field: SerializeField] public virtual Button VariablesButton { get; set; }
        [field: SerializeField] public virtual Button SaveAndViewButton { get; set; }
        [field: SerializeField] public virtual Button SaveButton { get; set; }
        [field: SerializeField] public virtual Button MainMenuButton { get; set; }
        [field: SerializeField] public virtual Button ExitButton { get; set; }
        [field: SerializeField] public virtual Toggle HelpToggle { get; set; }

        [field: SerializeField] public virtual SectionsUI Sections { get; set; }
    }
}