using UnityEngine.Events;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Extensions
{
    public static class Extensions
    {
        public static void AddOnSelectListener(this Toggle toggle, UnityAction action)
        {
            toggle.onValueChanged.AddListener(
                (selected) => { 
                    if (selected) action(); 
                }
            );
        }
    }
}