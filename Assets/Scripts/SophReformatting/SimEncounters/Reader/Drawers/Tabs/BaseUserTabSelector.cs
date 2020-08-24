﻿
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserTabSelector : MonoBehaviour, IUserTabSelector
    {
        public abstract void Display(UserSection section);
        public abstract event UserTabSelectedHandler TabSelected;
        public abstract void SelectTab(UserTab userTab);
    }
}