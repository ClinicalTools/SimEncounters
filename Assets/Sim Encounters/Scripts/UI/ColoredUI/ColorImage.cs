using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ColorImage : ColorBehaviour
    {
        protected override void SetColor(Color color) => GetComponent<Image>().color = color;
    }
}