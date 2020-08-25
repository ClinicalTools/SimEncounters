
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseDialogueOptionsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderDialogueOption> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
}