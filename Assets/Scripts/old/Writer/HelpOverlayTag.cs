using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HelpOverlayTag : MonoBehaviour {

    public string helpName;

    public bool ignorePosition;

    /*
    void Awake()
    {
        if (transform.Find("Help"))
        {
            WriterEventManager.ShowOverlayRequest += DisplayIcon;
            WriterEventManager.HideOverlayRequest += HideIcon;
        }

        maxHeight = maxHeight * Screen.height;
        minHeight = minHeight * Screen.height;        
    }

    // Use this for initialization
    void Start () {
        if (transform.Find("Help"))
        {
            helpOverlay = transform.Find("Help");
            helpPopUp = helpOverlay.transform.Find("HelpPopUp");

            helpOverlay.GetComponent<Toggle>().onValueChanged.AddListener(delegate { TogglePopUp(); });
            helpOverlay.GetComponent<Toggle>().group = GameObject.Find("Canvas/OverlayBG").GetComponent<ToggleGroup>();

            if (!helpName.Equals(""))
            {
                string fullPath = Application.streamingAssetsPath + "/Instructions/Help.csv";
                StreamReader reader = new StreamReader(fullPath);

                string lineText = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] line = lineText.Split(',');
                    if (line[0].Equals(helpName))
                    {
                        string helpText = line[1].Replace("\\n", "\n");
                        helpPopUp.Find("Line/TextBoxContainer/TextBoxBG/Text").GetComponent<TMPro.TextMeshProUGUI>().text = helpText;
                        break;
                    }
                    lineText = reader.ReadLine();
                }
                reader.Close();
            }

            if (helpPopUp.Find("Line/TextBoxContainer/TextBoxBG/Text").GetComponent<TMPro.TextMeshProUGUI>().text.Equals(""))
            {
                enabled = false;
            }
        }
    }

    void OnDestroy()
    {
        if (transform.Find("Help"))
        {
            WriterEventManager.ShowOverlayRequest -= DisplayIcon;
            WriterEventManager.HideOverlayRequest -= HideIcon;
        }
    }

    private void DisplayIcon()
    {
        if (!ignorePosition)
        {
            Vector3[] v = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(v);

            if(v[1].y > maxHeight || v[2].y < minHeight)
            {
                return;
            }
        }
        ToggleElement(helpPopUp, false);
        ToggleElement(helpOverlay, true);
        GetComponent<Canvas>().overrideSorting = true;
        //GetComponent<Canvas>().sortingLayerID = 10;
    }

    private void HideIcon()
    {
        ToggleElement(helpOverlay, false);
        ToggleElement(helpPopUp, false);
        //GetComponent<Canvas>().sortingLayerID = 0;
        GetComponent<Canvas>().overrideSorting = false;
    }

    private void TogglePopUp()
    {
        if (helpOverlay.GetComponent<Toggle>().isOn)
        {
            ToggleElement(helpPopUp, true);
        }
        else
        {
            ToggleElement(helpPopUp, false);
        }
    }


    private void ToggleElement(Transform target, bool show)
    {
        CanvasGroup tcg = target.GetComponent<CanvasGroup>();

        if (show)
        {
            tcg.alpha = 1.0f;
            tcg.blocksRaycasts = true;
            tcg.interactable = true;
        } else
        {
            tcg.alpha = 0.0f;
            tcg.blocksRaycasts = false;
            tcg.interactable = false;
        }
    }
    */
}
