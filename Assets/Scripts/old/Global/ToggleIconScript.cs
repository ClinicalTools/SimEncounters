using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIconScript : MonoBehaviour
{
    public List<GameObject> icons;

    public void CycleIcon()
    {
        bool nextActive = false;

        for(int i = 0; i < icons.Count; i++)
        {
            if (nextActive) {
                icons[i].SetActive(true);
                nextActive = false;
            } else if (icons[i].activeInHierarchy)
            {
                icons[i].SetActive(false);
                if (i + 1 >= icons.Count)
                {
                    icons[0].SetActive(true);
                }
                else
                {
                    nextActive = true;
                }
            }
        }
    }
}