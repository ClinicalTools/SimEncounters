using ClinicalTools.SimEncounters.Collections;

using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderFooter : MonoBehaviour, IUserSectionSelector, IUserTabSelector
    {
        public abstract void Display(UserEncounter userEncounter);
        public abstract event UserSectionSelectedHandler SectionSelected;
        public abstract void SelectSection(UserSection userSection);
        public abstract event UserTabSelectedHandler TabSelected;
        public abstract void SelectTab(UserTab userTab);

        public abstract event Action Finished;
    }
}