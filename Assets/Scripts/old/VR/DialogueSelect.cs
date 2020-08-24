using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSelect : MonoBehaviour {

    public bool isSelected = false;
    public DialogueSpawn dSpawn;

    // Use this for initialization
    void Start () {

    }

    // On button click, fade all other buttons
    public void select()
    {
        isSelected = true;
        dSpawn.optionsFade(this);


    }

}
