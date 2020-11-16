using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface ICloseSidebar
    {
        event Action CloseSidebar;
    }

    public interface IOpenSidebar
    {
        event Action OpenSidebar;
    }

    public class ReaderMobileSidebar : MonoBehaviour, ICloseSidebar, ICloseHandler
    {
        public event Action CloseSidebar;

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector)
            => EncounterSelector = encounterSelector;
        protected virtual void Start()
        {
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(EncounterSelector, EncounterSelector.CurrentValue);
        }

        public virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs userEncounter)
            => StartCoroutine(CloseAfterSecond());

        protected IEnumerator CloseAfterSecond()
        {
            yield return new WaitForSeconds(2f);
            CloseSidebar?.Invoke();
        }

        public void Close(object sender) => CloseSidebar?.Invoke();
    }
}
