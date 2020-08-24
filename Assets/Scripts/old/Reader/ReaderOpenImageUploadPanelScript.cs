using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReaderOpenImageUploadPanelScript : MonoBehaviour {

	private Transform parent;
	private Text serial;
	public ReaderSerialScript sserial;
	private ReaderTabManager tm;

    public int previewMinWidth = 0;
    public int previewMinHeight = 0;

    public int previewMaxWidth = 720;
    public int previewMaxHeight = 0;

	private bool isPatientImage;

    // Use this for initialization
    void Start () {
		/*if (tm != null) {
			return;
		}*/
		if (tm == null) {
			tm = GameObject.Find("GaudyBG").GetComponent<ReaderTabManager>();
			if (sserial == null) {
				sserial = new ReaderSerialScript();
			}
			Toggle t;
			if ((t = transform.parent.GetComponentInChildren<Toggle>()) != null) {
				if (t.isOn) {
					isPatientImage = true;
				}
			}
		}

		//LoadData(GetGuid());
	}

	public void ForceStart()
	{
		Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetGuid(string s) {
		if (sserial == null) {
			sserial = new ReaderSerialScript();
		}
		sserial.SetSerial (s);
	}

	public string GetGuid() {
		return sserial.GetSerial ();
	}



	public void LoadData(string guid) {
		if (tm == null) {
			Start();
		}

		if (isPatientImage) {
			guid = GlobalData.patientImageID;
		}

		print ("Loading " + guid);
		Image img = GetComponent<Image> ();
		img.sprite = null;

		if (tm.transform.GetComponent<ReaderDataScript> ().GetImageKeys().Contains (guid)) { //Load image
			img.sprite = tm.transform.GetComponent<ReaderDataScript> ().GetImage(guid).sprite;
		}
		if (img.sprite == null) {
			img.GetComponent<CanvasGroup> ().alpha = 0f;
			img.transform.parent.GetComponent<Image> ().enabled = false;
			transform.parent.gameObject.SetActive(false);
		} else {
			img.GetComponent<CanvasGroup> ().alpha = 1f;
			img.transform.parent.GetComponent<Image> ().enabled = false;
			transform.parent.gameObject.SetActive(true); //Set the image's parent active
			//Start();
			ResizeImage();
		}
	}

    private void ResizeImage()
    {
        Image currentImage = GetComponent<Image>();

        int uploadWidth = currentImage.sprite.texture.width;
        int uploadHeight = currentImage.sprite.texture.height;

        if (previewMaxWidth > 0)
        {
            if (uploadWidth > previewMaxWidth)
            {
                uploadHeight = Convert.ToInt32(uploadHeight * (1.0f * previewMaxWidth / uploadWidth));
                uploadWidth = previewMaxWidth;
            }
        }

        if (previewMaxHeight > 0)
        {
            if (uploadHeight > previewMaxHeight)
            {
                uploadWidth = Convert.ToInt32(uploadWidth * (1.0f * previewMaxHeight / uploadHeight));
                uploadHeight = previewMaxHeight;
            }
        }

        //displayImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uploadWidth, uploadHeight);
        transform.parent.GetComponent<LayoutElement>().preferredWidth = uploadWidth;
        //Debug.Log(uploadWidth);
        transform.parent.GetComponent<LayoutElement>().preferredHeight = uploadHeight;
        //Debug.Log(uploadHeight);

        if (uploadWidth < previewMinWidth)
        {
            uploadWidth = previewMinWidth;
        }

        if (uploadHeight < previewMinHeight)
        {
            uploadHeight = previewMinHeight;
        }

        //Current buffer 40w 110h
        //GameObject.Find("ImagePickerBG/ImagePicker").GetComponent<RectTransform>().sizeDelta = new Vector2(uploadWidth + previewWidthMargin, uploadHeight + previewHeightMargin);
    }

    public void OpenImagePanel() {
		if (sserial.GetSerial () == null || sserial.GetSerial ().Equals ("")) {
			print(sserial.GenerateSerial ());
		}
		string panelType = "ImageViewerBG";
		
		GameObject panel = Instantiate (Resources.Load (GlobalData.resourcePath + "/Prefabs/Panels/" + panelType), GameObject.Find ("GaudyBG").transform) as GameObject;
		panel.name = panelType;
		panel.GetComponent<ReaderImageUploaderScript> ().LoadData (this.GetComponent<Image> ());
	}
}
