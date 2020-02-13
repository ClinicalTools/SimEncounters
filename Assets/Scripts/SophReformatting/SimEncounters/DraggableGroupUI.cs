using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DraggableGroupUI : MonoBehaviour
    {
        [SerializeField] private GameObject placeholder;
        public GameObject Placeholder { get => placeholder; set => placeholder = value; }

        [SerializeField] private RectTransform childrenParent;
        public virtual RectTransform ChildrenParent { get => childrenParent; set => childrenParent = value; }

    }
}