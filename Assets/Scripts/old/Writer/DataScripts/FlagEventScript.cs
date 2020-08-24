using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagEventScript : MonoBehaviour {
    public List<string> events;
    public List<string> flags;
    public Image eventImage;

	// Use this for initialization
	void Start () {
        events = new List<string>();
        flags = new List<string>();
        events.Add("Event1");
        events.Add("Event2");
        flags.Add("Flag1");
        flags.Add("Flag2");

        //Cursor.visible = true;
	}

    public void addEvent(Text input)
    {
        bool isDuplicate = events.Contains(input.text);

        // If it doesn't exist in the list, add it to the list
        if (isDuplicate == false)
        {
            events.Add(input.text);
        }

        if (flags.Contains(input.text))
        {
            eventImage.color = new Color(94f / 255f, 175f / 255f, 172f / 255f);
        }
        else
        {
            eventImage.color = new Color(255f, 0f, 0f);
        }

    }

    public void addFlag(Text input)
    {
        bool isDuplicate = flags.Contains(input.text);

        // If it doesn't exist in the list, add it to the list
        if (isDuplicate == false)
        {
            flags.Add(input.text);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
