using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.UI
{
    [ExecuteAlways]
    public class ColorImage : ColorBehaviour
    {
        protected override void SetColor(Color color) => GetComponent<Image>().color = color;
    }
}