using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabsUI : MonoBehaviour
    {
        [field: SerializeField] public virtual Transform TabButtonsParent { get; set; }
        [field: SerializeField] public virtual Button AddButton { get; set; }
        [field: SerializeField] public virtual Transform TabContent { get; set; }
    }
}