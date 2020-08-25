using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPinButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        public virtual Button Button { get => button; set => button = value; }
    }
}