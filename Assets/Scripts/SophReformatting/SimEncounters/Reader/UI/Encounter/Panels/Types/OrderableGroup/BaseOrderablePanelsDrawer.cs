
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseOrderablePanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderOrderableItemPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
}