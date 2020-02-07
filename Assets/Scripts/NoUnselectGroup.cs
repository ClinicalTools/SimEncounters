﻿using UnityEngine.UI;

public class NoUnselectGroup : ToggleGroup
{
    protected override void Awake()
    {
        allowSwitchOff = true;

        base.Awake();
    }

    protected virtual void Update()
    {
        if (allowSwitchOff && AnyTogglesOn()) {
            allowSwitchOff = false;
        }
    }

}
