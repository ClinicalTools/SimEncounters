using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseOptionUserPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
}