using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollbarResetScript : MonoBehaviour
    {
        protected Scrollbar Scrollbar { get; set; }

        // Use this for initialization
        void Start()
        {
            Scrollbar = GetComponent<Scrollbar>();
            NextFrame.Function(ResetScroll);
        }

        private void ResetScroll()
        {
            if (this != null)
                Scrollbar.value = 1;
        }

        void Update()
        {
            if (Scrollbar.value > 1 || Scrollbar.value < 0)
                ResetScroll();
        }
    }
}