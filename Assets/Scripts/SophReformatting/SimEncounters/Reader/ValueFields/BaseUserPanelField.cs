﻿namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserPanelField : BaseField
    {
        public abstract void Initialize(UserPanel userPanel);
        public abstract void Initialize(UserPanel userPanel, string value);
    }
}