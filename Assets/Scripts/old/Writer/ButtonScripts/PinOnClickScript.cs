using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinOnClickScript : MonoBehaviour {

	[SerializeField]
	public PinType pinType;

	public enum PinType
	{
		Dialogue,
		Quiz,
        Conditional
	}

	private void Start()
	{
		switch(pinType) {
			case PinType.Dialogue:
				GetComponent<Button>().onClick.AddListener(delegate
				{
					ButtonListenerFunctionsScript.OpenDialogueEditor(GetComponent<Button>());
				});
				break;
			case PinType.Quiz:
				GetComponent<Button>().onClick.AddListener(delegate
				{
					ButtonListenerFunctionsScript.OpenQuizEditor(GetComponent<Button>(), instantiatePanel("QuizEditorBG"));
				});
				break;
		}
	}

	private GameObject instantiatePanel(string panelName)
	{
		GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
		pinPanelPrefab.transform.SetParent(GameObject.Find("GaudyBG").transform, false);
		return pinPanelPrefab;
	}
}
