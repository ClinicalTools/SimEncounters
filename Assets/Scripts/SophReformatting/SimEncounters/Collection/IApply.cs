using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IApply<T>
    {
        event Action<T> Apply;
    }
}