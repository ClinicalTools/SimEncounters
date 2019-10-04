using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public enum LevelChange
	{
		reader = 0,
		writer = 1,
		menu = 2
	}

	/// <summary>
	/// Change scenes depending on if mobile or not
	/// </summary>
	/// <param name="level">0=reader, 1=writer, 2=menu</param>
	public void GenericSceneChange(int level)
	{
		switch(level) {
			case (int) LevelChange.reader:
				if (GlobalData.GDS.isMobile) {
					SceneChange("MobileCassReader");
				} else {
					SceneChange("CassReader");
				}
				break;
			case (int) LevelChange.writer:
				SceneChange("Writer");
				break;
			case (int) LevelChange.menu:
				if (GlobalData.GDS.isMobile) {
					SceneChange("MobileMainMenu");
				} else {
					SceneChange("Cass1WriterMainMenu");
				}
				break;
		}
	}

	public void SceneChange(string sceneName){
		GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"),GameObject.Find("GaudyBG").transform) as GameObject;
		panel.name = "ConfirmActionBG";
		ConfirmationScript cs = panel.GetComponent<ConfirmationScript> ();

		cs.actionText.text = "Are you sure you want to exit?";
		if (GlobalData.resourcePath.Equals("Writer")) {
			cs.actionText.text = cs.actionText.text + "\nAny unsaved changes will be lost";
		}

        if (sceneName == "Cass1WriterMainMenu")
            GlobalData.loadLocal = true;

        cs.args = new object[] {sceneName};
		cs.AnyParamMethodToConfirm = ApprovedSceneChange;
	}

	public void ApprovedSceneChange(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void ApprovedSceneChange(object[] sceneName) {
		SceneManager.LoadScene (sceneName[0].ToString());
	}
}
