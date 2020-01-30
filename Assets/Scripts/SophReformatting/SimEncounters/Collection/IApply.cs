using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IApply<T>
    {
        event Action<T> Apply;
    }
}