using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SearchStuffUI : MonoBehaviour
    {
        [SerializeField] private FilterGroupsUI filters;
        public virtual FilterGroupsUI Filters { get => filters; set => filters = value; }

        [SerializeField] private SortingOrderUI sortingOrder;
        public virtual SortingOrderUI SortingOrder { get => sortingOrder; set => sortingOrder = value; }
    }
}