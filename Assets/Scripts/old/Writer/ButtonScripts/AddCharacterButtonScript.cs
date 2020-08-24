using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCharacterButtonScript : MonoBehaviour {

    public string firstName, lastName;
	public bool isMainCharacter = false;
	private DialogueManagerScript dms;
    //private DataScript ds;

	void Start() {
        //ds = GameObject.Find("GaudyBG").GetComponent<DataScript>();
        //Debug.Log(ds.firstName);
		dms = transform.parent.parent.GetComponentInChildren<DialogueManagerScript> ();
        
		if (isMainCharacter) {
            if(transform.Find("CharacterNameValue"))
            {
            
                if (this.name == "PlayerEntryDefault")
                {
                    transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>().text = "Provider";
                }
                else if (this.name == "InstructorEntryDefault")
                {
                    transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>().text = "Instructor";
                }
				else if (this.name.StartsWith("Character "))
				{
					//we set the name when adding the character, so this shouldn't be necessary, but it's an option
					//transform.Find("CharacterNameValue").GetComponent<Text>().text = this.name;
				}
				
				else if (GlobalData.firstName == "" && GlobalData.lastName == "")
                {
                    transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>().text = "Patient";
                }

                else
                {
					if (GlobalData.firstName == "")
                    {
                        //transform.Find("CharacterNameValue").GetComponent<Text>().text = ds.lastName;
                        transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>().text = "Patient";

                    }

                    else
                    {
                        //transform.Find("CharacterNameValue").GetComponent<Text>().text = ds.firstName + " " + ds.lastName;
                        transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>().text = "Patient";

                    }
                }
            
            }
            //transform.Find("CharacterNameValue").GetComponent<Text>().text = "Patient";
        }
        
	}

	public void AddCharacter() {
		dms.AddNewEntry (transform.Find("CharacterNameValue").GetComponent<TextMeshProUGUI>());
	}
}
