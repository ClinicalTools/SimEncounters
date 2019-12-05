using ClinicalTools.SimEncounters;
using UnityEngine;
using UnityEngine.UI;

public class OpenConditionalPanel : MonoBehaviour
{
    public SerialScript sserial;
    private TabManager tm;
    public bool canSetPatientImage = false;
    private bool isPatientImage;
    public bool updateThumbnail = true;

    // TODO: change how serial is handled
    // Use this for initialization
    void Start()
    {
        tm = GameObject.Find("GaudyBG").GetComponent<TabManager>();
        if (sserial == null) {
            sserial = new SerialScript();
        }
    }


    public void SetGuid(string s)
    {
        if (sserial == null) {
            Start();
        }
        sserial.SetSerial(s);
        return;
    }

    public string GetGuid()
    {
        return sserial.GetSerial();
    }

    public void OpenImagePanel()
    {
        if (sserial.GetSerial() == null || sserial.GetSerial().Equals("")) {
            //print(sserial.GenerateSerial(WriterHandler.WriterInstance.));
        }
        GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ImageUploadBG"), GameObject.Find("GaudyBG").transform) as GameObject;
        panel.name = "ImageUploadBG";
        panel.transform.Find("ImageUploadPanel/Content/Row1/Toggle").gameObject.SetActive(canSetPatientImage);

        var imgUploader = panel.GetComponent<ImageUploaderScript>();
        imgUploader.updateThumbnail = updateThumbnail;
        imgUploader.LoadData(GetComponent<Image>());

        var tgl = panel.transform.Find("ImageUploadPanel/Content/Row1/Toggle").GetComponent<Toggle>();
        tgl.isOn = isPatientImage;
        //Whether or not we want to permanantly set an image as a patient picture
        if (transform.parent.GetComponentInChildren<Toggle>()) {
            tgl.interactable = transform.parent.GetComponentInChildren<Toggle>().interactable;
        }
    }
}
