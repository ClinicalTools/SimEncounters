using ClinicalTools.SimEncounters.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class Abcdef : MonoBehaviour
    {
        [Inject]
        public virtual void Inject(IEncounterDataReaderSelector encounterDataReaderSelector)
        {
            Debug.LogError("hmmm");
        }

    }
}