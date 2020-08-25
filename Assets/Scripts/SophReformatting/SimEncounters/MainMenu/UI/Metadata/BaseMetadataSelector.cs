using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseMetadataSelector : MonoBehaviour
    {
        public abstract WaitableResult<KeyValuePair<SaveType, EncounterMetadata>> GetMetadata(MenuEncounter menuEncounter); 
    }
}