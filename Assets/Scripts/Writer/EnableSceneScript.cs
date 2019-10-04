using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Invoke("LoadScene", 2f) ;
		LoadScene();
	}
	
	private void LoadScene()
	{
		List<GameObject> rootObjs = new List<GameObject>(SceneManager.GetActiveScene().GetRootGameObjects());
		rootObjs.Find(obj => obj.name.Equals("AllObjects")).SetActive(true);
		//GameObject.Find("AllObjects").SetActive(true);
	}
}
