using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.EncounterReader;
using System.Threading.Tasks;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterManager : MonoBehaviour
    {
        public static EncounterManager Instance { get; protected set; }
        public Encounter Encounter { get; protected set; }

        protected virtual void Awake()
        {
            Instance = this;

            GlobalData globalData = GlobalData.Instance;
            var encounterGetter = globalData.EncounterGetter;
            if (encounterGetter == null) {
                encounterGetter = new EncounterGetter();
                //globalData.EncounterGetter = encounterGetter;
                //encounterGetter.EncounterXml.GetDemoCase();
            }

            Task.Run(() => GetEncounter(encounterGetter.Encounter));
        }

        protected virtual async Task GetEncounter(Task<Encounter> encounterTask)
        {
            await encounterTask;

            Encounter = encounterTask.Result;
        }
    }
}