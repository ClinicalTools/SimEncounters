using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("PauseAnimation", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PauseAnimation()
    {
        GetComponent<Animator>().enabled = false;
    }

	public void Reset()
	{
		Start();
	}
}
