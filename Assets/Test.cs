﻿using ClinicalTools.SimEncounters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject]
    public void TestInject(IUserEncounterReaderSelector encounterReaderSelector, IMenuEncountersReader menuEncountersReader)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
