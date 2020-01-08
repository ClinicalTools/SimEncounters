using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionsUI : MonoBehaviour
    {
        [field: SerializeField] public virtual Transform SectionButtonsParent { get; set; }
        [field: SerializeField] public virtual Button AddButton { get; set; }

        [field: SerializeField] public virtual TabsUI Tabs { get; set; }
        [field: SerializeField] public virtual SectionButtonUI SectionButtonPrefab { get; set; }
        [field: SerializeField] public virtual SectionCreatorUI AddSectionPrefab { get; set; }
    }
}