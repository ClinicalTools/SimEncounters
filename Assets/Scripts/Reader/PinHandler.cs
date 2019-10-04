using UnityEngine;
using UnityEngine.UI;

public class PinHandler : MonoBehaviour
{
    [SerializeField]
    private bool pinStartHidden = false;

    private Transform pinParent;
    public Transform PinParent {
        get {
            if (!pinParent)
                pinParent = transform;

            return pinParent;
        }
    }

    private GameObject bg;
    private GameObject BG {
        get {
            if (!bg)
                bg = GameObject.Find("GaudyBG");
            return bg;
        }
    }

    private ReaderTabManager tm;
    private ReaderTabManager TM {
        get {
            if (!tm)
                tm = BG.GetComponent<ReaderTabManager>();
            return tm;
        }
    }
    private ReaderDataScript ds;
    private ReaderDataScript DS {
        get {
            if (!ds)
                ds = BG.GetComponent<ReaderDataScript>();
            return ds;
        }
    }
    
    public void AddDialoguePin()
    {
        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/ShowDialogue") as GameObject;
        pinObj = Instantiate(pinObj, PinParent);
        Button b = pinObj.transform.GetChild(0).GetComponent<Button>();
        b.onClick.AddListener(delegate {
            if (Input.GetMouseButton(1)) { //If right clicked
                Debug.Log("OKAY");
                return;
            }
            Transform t = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Panels/DialoguePopUp") as GameObject, GameObject.Find("Canvas").transform).transform;
            t.gameObject.SetActive(true);
            t.GetComponentInChildren<ReaderEntryManagerScript>().SetPin(b.transform);
            t.GetComponentInChildren<ReaderDialogueManagerScript>().PopulateDialogue(b.transform);
            if (t.Find("Image")) {
                t.Find("Image").GetComponent<Image>().color = DS.GetImage(TM.getCurrentSection()).color; //ds.GetImage(tm.getCurrentSection()) != null ? ds.GetImage(tm.getCurrentSection()).color : GlobalData.GDS.defaultGreen;
            }
        });
        pinObj.tag = "Value";
        pinObj.name = "Dialogue" + "Pin";

        pinObj.SetActive(!pinStartHidden);
    }

    public void AddQuizPin()
    {
        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/QuizPinIcon") as GameObject;

        if (PinParent.Find("DialoguePin")) {
            pinObj = Instantiate(pinObj.transform.Find("ShowQuiz").gameObject, PinParent.Find("DialoguePin"));
        } else {
            pinObj = Instantiate(pinObj, PinParent);

            pinObj.SetActive(!pinStartHidden);
        }

        Button b = pinObj.GetComponentInChildren<Button>();
        b.onClick.AddListener(delegate {
            Transform t = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Panels/QuizPopUp") as GameObject, DS.transform).transform;
            t.gameObject.SetActive(true);
            t.GetComponentInChildren<ReaderEntryManagerScript>().SetPin(b.transform);
            t.GetComponentInChildren<ReaderEntryManagerScript>().PopulatePanel(b.transform);
            t.Find("Image").GetComponent<Image>().color = DS.GetImage(TM.getCurrentSection()).color;
        });
        pinObj.tag = "Value";
        pinObj.name = "Quiz" + "Pin";
    }

    public void ShowPins()
    {
        var pin = PinParent.Find("DialoguePin");
        if (!pin)
            pin = PinParent.Find("QuizPin");

        if (pin)
            pin.gameObject.SetActive(true);
    }
}
