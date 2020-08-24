using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is used on the Create button of the EntrySelectorBG
public class AddEnrtyButtonScript : MonoBehaviour {

	HistoryFieldManagerScript hm;

	// Use this for initialization
	void Start () {
		
	}
	
	public void CreateEntry() {
		hm = GameObject.Find ("GaudyBG").GetComponent<TabManager> ().TabContentPar.transform.GetComponentInChildren<HistoryFieldManagerScript>();
		hm.AddEntryFromPanel ();
	}

	public void DestroyMe(){
		Destroy (this.gameObject);
	}
}
