
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelsCreator : MonoBehaviour
    {
        public abstract List<BaseReaderPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
}