using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncountersOld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenImageUploadPanelScript : MonoBehaviour
{
    public SerialScript sserial;
    private TabManager tm;
    public bool canSetPatientImage = false;
    private bool isPatientImage;
    public bool updateThumbnail = true;

    // Use this for initialization
    void Start()
    {
        tm = GameObject.Find("GaudyBG").GetComponent<TabManager>();
        if (sserial == null) {
            sserial = new SerialScript();
        }
        Toggle t;
        if ((t = transform.parent.GetComponentInChildren<Toggle>()) != null) {
            if (t.isOn) {
                isPatientImage = true;
            }
            canSetPatientImage = true;
        }
    }

    public void SetPatientImage(bool b)
    {
        isPatientImage = b;
        if (transform.parent.GetComponentInChildren<Toggle>()) {
            transform.parent.GetComponentInChildren<Toggle>().isOn = isPatientImage;
        }
    }

    public bool GetIsPatientImage()
    {
        return isPatientImage;
    }

    public void SetGuid(string s)
    {
        if (sserial == null) {
            Start();
        }
        sserial.SetSerial(s);
        return;
        /*
        if (isPatientImage) {
            sserial.SetSerial(GlobalData.patientImageID);
        } else {
            sserial.SetSerial(s);
        }*/
    }

    public string GetGuid()
    {
        return sserial.GetSerial();
        /*
        if (isPatientImage) {
            if (!GlobalData.patientImageID.Equals("")) {
                GlobalData.patientImageID = sserial.GetSerial();
            }
            return GlobalData.patientImageID;
        } else {
            return sserial.GetSerial();
        }
        */
    }

    public void LoadData(string guid)
    {
        //Toggle represents isPatientImage. If it's missing, can't set patient image
        Toggle t;
        if ((t = transform.parent.GetComponentInChildren<Toggle>()) != null) {
            if (t.isOn) {
                guid = GlobalData.patientImageID;
                isPatientImage = true;
            }
            canSetPatientImage = true;
        }

        print("Loading " + guid);
        var img = GetComponent<Image>();
        img.sprite = null;

        var images = WriterHandler.WriterInstance.EncounterData.Images;
        if (images.ContainsKey(guid)) { //Load image
            img.sprite = images[guid].sprite;
        }

        if (img.sprite == null) {
            img.color = Color.clear;
            transform.parent.GetComponent<Image>().enabled = true;
        } else {
            img.color = Color.white;
            transform.parent.GetComponent<Image>().enabled = false;
        }
    }

    public void OpenImagePanel()
    {
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
