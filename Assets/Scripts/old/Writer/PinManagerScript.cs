using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinManagerScript : MonoBehaviour {

    public List<Sprite> pinImages; // List of editors that will be included
    private Transform menu; // Pin Display Area
    private Transform parent; // Pin Area
    private GameObject BG;

    void Start () {
        BG = GameObject.Find("GaudyBG");
        menu = this.transform.GetChild(0);       
    }

    // When you click the pin icon, shows all available pins
    public void pinList()
    {
        // If it's aleady active, destory the pins and turn off
        if (menu.gameObject.activeSelf == true)
        {
            menu.gameObject.SetActive(false);
            wipePins();
            return;
        }

        // Instantiate the pins, compare to pin area to prevent duplicate pins on both sides
        else
        {
            bool match;
            menu.gameObject.SetActive(true);
            parent = transform.parent.Find("PinArea");
			foreach (Sprite pinImage in pinImages) {
				match = false;
				if (parent.childCount > 0) {
					for (int i = 0; i < parent.childCount; i++) {
						bool test = parent.GetChild (i).Find ("ItemIcon");
						Debug.Log (test);

						if (test) {
							if (((parent.GetChild (i).Find ("ItemIcon").GetComponent<Image> ().sprite.name) == (pinImage.name))) { //|| ((parent.GetChild(i).Find("ItemIcon").GetComponent<Image>().sprite.name) == (pinImage.name)))
								match = true;
								continue;
							}

						} else {
							if (((parent.GetChild (i).Find ("Item Background/ItemIcon").GetComponent<Image> ().sprite.name) == (pinImage.name))) { //|| ((parent.GetChild(i).Find("ItemIcon").GetComponent<Image>().sprite.name) == (pinImage.name)))
								match = true;
								continue;
							}
						}

						//if (match)
						//{
						//    continue;
						//}
					}

					//Debug.Log(i);
					//Debug.Log(parent.GetChild(i).Find("Item Background/ItemIcon").GetComponent<Image>().sprite.name);                    }
				}

				if (match) {
					continue;
				}

				GameObject gObj = Instantiate (menu.transform.Find ("Pin").gameObject, menu);
				gObj.SetActive (true);
				GameObject imgObj = gObj.transform.Find ("Item Background/ItemIcon").gameObject;
				imgObj.GetComponent<Image> ().sprite = pinImage;
				imgObj.AddComponent<ToolTipInfoScript> ();
				Button b = gObj.GetComponent<Button> ();
				gObj.tag = "Value";
				editorSelection (b, gObj, pinImage);
			}
        }
    }
 
    // Adds listeners to the editor buttons
    // Opens the editor panel on click
    // Adds pin to Pin Area
    public void editorSelection(Button b, GameObject go, Sprite s)
    {
        Debug.Log(s.name);
        b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.assign(parent, go); });
        b.onClick.AddListener(delegate { menu.gameObject.SetActive(false); });
        b.onClick.AddListener(delegate { wipePins(); });

		switch (s.name) {
			case "pin-dialogue":
				b.onClick.AddListener (delegate {
					ButtonListenerFunctionsScript.OpenDialogueEditor (b);
				});
	                //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenDialogueEditor(b, instantiatePanel("DialogueEditorBG")); });
				go.transform.name = "DialoguePin";
				go.transform.Find ("Item Background/ItemIcon").GetComponent<ToolTipInfoScript> ().tooltipName = "DialoguePin";
				break;
			case "pin-quiz":
				b.onClick.AddListener (delegate {
					ButtonListenerFunctionsScript.OpenQuizEditor (b, instantiatePanel ("QuizEditorBG"));
				});
	                //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenQuizEditor(b); });
				go.transform.name = "QuizPin";
				go.transform.Find ("Item Background/ItemIcon").GetComponent<ToolTipInfoScript> ().tooltipName = "QuizPin";
				break;
			case "pin-mood":
	                // Mood listener goes here
				break;
			case "pin-vitals":
	                // Vital listener goes here
				break;
			case "pin-flag":
				b.onClick.AddListener (delegate {
					ButtonListenerFunctionsScript.OpenFlagEditor (b, instantiatePanel ("FlagEventEditorBG"));
				});
				go.transform.name = "FlagPin";
				go.transform.Find ("Item Background/ItemIcon").GetComponent<ToolTipInfoScript> ().tooltipName = "FlagPin";
				break;
			case "pin-event":
				Image eventImg = go.transform.Find ("Item Background").GetComponent<Image> ();
				b.onClick.AddListener (delegate {
					ButtonListenerFunctionsScript.OpenEventEditor (b, instantiatePanel ("EventEditorBG"));
				});
				b.onClick.AddListener (delegate {
					eventImg.color = new Color (255f, 0f, 0f);
				});
				GameObject.Find ("GaudyBG").GetComponent<FlagEventScript> ().eventImage = eventImg;
				go.transform.name = "EventPin";
				go.transform.Find ("Item Background/ItemIcon").GetComponent<ToolTipInfoScript> ().tooltipName = "EventPin";
				break;
			default:
				Debug.Log ("Pins have been renamed or need to be re-configured");
				break;
		}
    }

    public void wipePins()
    {
        // This wipes all pin clones. Does not wipe the pointer or the pin template. 
        for (int i = 2; i < menu.childCount; i++)
        {
            Destroy(menu.GetChild(i).gameObject);
        }
    }

    private GameObject instantiatePanel(string panelName)
    {
        GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
        pinPanelPrefab.transform.SetParent(BG.transform, false);
        return pinPanelPrefab;
    }

}
