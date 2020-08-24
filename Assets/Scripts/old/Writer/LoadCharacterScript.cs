using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacterScript : MonoBehaviour {
    public Transform characterParent;
    private GameObject prefabCharacter;
    private void Awake()
    {
        
    }

    void Start () {
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		StartCoroutine(loadCharacter("M3DFemale"));
		StartCoroutine(loadCharacter("M3DMale"));
	}

    public IEnumerator loadCharacter(string charName)
    {
        ResourceRequest loadAsync =  Resources.LoadAsync("Writer/Prefabs/Character Creation/" + charName, typeof(GameObject));
        while (!loadAsync.isDone)
        {
            Debug.Log("Load Progress: " + loadAsync.progress);
            yield return null;
        }
        prefabCharacter = Instantiate(loadAsync.asset as GameObject);
        prefabCharacter.transform.SetParent(characterParent, false);
    }

}
