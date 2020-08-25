
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseOrderablePanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderOrderableItemPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
}