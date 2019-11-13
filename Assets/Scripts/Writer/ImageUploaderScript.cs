using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Crosstales.FB;
using SimEncounters;

public class ImageUploaderScript : MonoBehaviour
{
    //string filePath;
    private Image displayImage;
    private Transform row0;
    private Texture2D texture;
    private WriterHandler ds;
    private SpriteHolderScript PatientImg => ds.EncounterData.Images[GlobalData.patientImageID];
    private Image currentImage;
    private bool openingFileExpolorer;
    private string[] extensions = new string[] { "png", "jpg", "jpeg" };
    private bool isPatientImage;
    public SerialScript sserial;

    public int previewMinWidth = 540;
    public int previewMinHeight = 300;

    public int previewMaxWidth = 990; //720, 990
    public int previewMaxHeight = 550; //400, 550

    private Camera patientCamera;
    public bool updateThumbnail = true;

    // Use this for initialization
    void Start()
    {
        patientCamera = GameObject.Find("CharacterCamera").GetComponent<Camera>();
        row0 = transform.Find("ImageUploadPanel/Content/Row0");
        displayImage = transform.Find("ImageUploadPanel/Content/Row0/WithImage/ImageGoesHere").GetComponent<Image>();
        ds = WriterHandler.WriterInstance;
        openingFileExpolorer = false;

        sserial = new SerialScript();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Destroy(gameObject);
    }

    public void ToggleIsPatientImage()
    {
        ToggleIsPatientImage(!isPatientImage);
    }

    public void ToggleIsPatientImage(bool b)
    {
        print("Setting is patient for " + GetGuid() + " to " + b);
        isPatientImage = b;
        if (isPatientImage) {
            print(GlobalData.patientImageID);
            //if (GlobalData.patientImageID == null || GlobalData.patientImageID.Equals("")) {
            if (PatientImg == null) {
                //No patient image set. Wait until Apply to set one
            } else if (displayImage?.sprite != null &&
                !currentImage.GetComponent<OpenImageUploadPanelScript>().GetIsPatientImage()) {
                //Ask to replace current image with patient image or replace patient image with current
                AskAboutReplace();
            } else {
                //Displayed image is null. Replace it with patient image
                if (updateThumbnail)
                    currentImage.sprite = PatientImg.sprite;
                LoadImage(PatientImg.sprite);
            }
        } else {
            LoadImage(ds.EncounterData.Images[GetGuid()]?.sprite);
        }

        //transform.Find("ImageUploadPanel/Content/Row1/Toggle").GetComponent<Toggle>().isOn = isPatientImage;
    }

    public void AskAboutReplace()
    {
        if (PatientImg == null) {
            ReplacePatientImage();
            return;
        }

        transform.GetChild(1).gameObject.SetActive(true);
        Image keep, replace;
        int max = (int)transform.Find("ChooseImage/Content/Row0").GetComponent<LayoutElement>().preferredHeight;

        keep = transform.Find("ChooseImage/Content/Row0/KeepImage/Image").GetComponent<Image>();
        keep.sprite = PatientImg.sprite;
        keep.GetComponent<LayoutElement>().preferredHeight = Math.Min(max, keep.sprite.texture.height);
        keep.GetComponent<LayoutElement>().preferredWidth = Math.Min(max, keep.sprite.texture.width);

        (replace = transform.Find("ChooseImage/Content/Row0/ReplaceImage/Image").GetComponent<Image>()).sprite = displayImage.sprite;
        replace.GetComponent<LayoutElement>().preferredHeight = Math.Min(max, replace.sprite.texture.height);
        replace.GetComponent<LayoutElement>().preferredWidth = Math.Min(max, replace.sprite.texture.width);
    }

    /// <summary>
    /// Replace the patient's image with the currently uploaded image
    /// </summary>
    public void ReplacePatientImage()
    {
        ds.EncounterData.Images[GlobalData.patientImageID].sprite = displayImage.sprite;

        patientCamera.transform.GetChild(0).gameObject.SetActive(true);
        Image i = patientCamera.transform.Find("Canvas/Image").GetComponent<Image>();
        i.sprite = displayImage.sprite;
    }

    public void RemovePatientImage()
    {
        ds.EncounterData.Images.Remove(GlobalData.patientImageID);
        patientCamera.transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary>
    /// Replace the currently uploaded image with the patient's image
    /// </summary>
    public void KeepPatientImage()
    {
        displayImage.sprite = PatientImg.sprite;
    }

    public bool useMipmaps = false;
    public TextureFormat tFormat = TextureFormat.RGBA32;
    private IEnumerator Testing2(string filePath)
    {
        Cursor.visible = true;

        //filePath = "file://" + EditorUtility.OpenFilePanel ("Choose your image", "", "png");
        //filePath = "file://C:/Users/Will/Documents/Unity/Clinical Encounters Writer/Test Pictures/Non-Profit-Free-Download-PNG.png";
        //filePath = "http://clipart-library.com/image_gallery2/Non-Profit-Free-Download-PNG.png";

        if (!filePath.Equals("") && !filePath.Equals("file://")) {
            if (filePath.StartsWith("file:///")) {
                filePath = filePath.Remove(0, 8);
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            print("Bytes: " + bytes.Length);
            Texture2D newTexture;
            if (useMipmaps) {
                newTexture = new Texture2D(2, 2);
            } else {
                newTexture = new Texture2D(2, 2, tFormat, false);
            }
            newTexture.LoadImage(bytes);
            print("Mip map count: " + newTexture.mipmapCount);
            print("Raw texture data: " + newTexture.GetRawTextureData().Length);
            //Compress the image
            //newTexture.LoadImage(newTexture.EncodeToJPG());
            //print(newTexture.GetRawTextureData().Length);

            //Scale to max file size
            if (newTexture.height > previewMaxHeight) {
                float heightRatio = (float)previewMaxHeight / newTexture.height;
                var width = (int)Math.Floor(newTexture.width * heightRatio);
                if (width > 0) {
                    TextureScale.Bilinear(newTexture, width, previewMaxHeight);
                } else {
                    ds.ShowMessage("Invalid image width", true);
                    yield break;
                }
            }
            if (newTexture.width > previewMaxWidth) {
                float widthRatio = (float)previewMaxWidth / newTexture.width;
                var height = (int)Math.Floor(newTexture.height * widthRatio);
                if (height > 0) {
                    TextureScale.Bilinear(newTexture, previewMaxWidth, height);
                } else {
                    ds.ShowMessage("Invalid image height", true);
                    yield break;
                }
            }
            print("Raw texture data: " + newTexture.GetRawTextureData().Length);
            if (newTexture.format != TextureFormat.DXT1 && newTexture.format != TextureFormat.DXT5) {
                print("Base64: " + Convert.ToBase64String(newTexture.EncodeToPNG()).Length);
            }

            texture = newTexture;
            row0.Find("NoImage").gameObject.SetActive(false);
            row0.Find("UploadingImage").gameObject.SetActive(true);

            /*
			StringBuilder hex = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
				hex.AppendFormat("{0:x2}", b);
			*/

            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
            displayImage.sprite = newSprite;
            row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = true;
            row0.Find("UploadingImage").gameObject.SetActive(false);
            row0.Find("WithImage").gameObject.SetActive(true);

            if (isPatientImage && PatientImg != null) {
                AskAboutReplace();
            }
        }
        yield break;
    }

    public void Compress()
    {
        byte[] bytes;
        if (texture.format != TextureFormat.DXT1 && texture.format != TextureFormat.DXT5) {
            if (texture.format == TextureFormat.RGB24) {
                bytes = texture.EncodeToJPG();
            } else {
                bytes = texture.EncodeToPNG();
            }
            string imageData = Convert.ToBase64String(bytes);
            /*print("Bytes: " + bytes.Length);
			print("Raw Texture Data: " + texture.GetRawTextureData().Length);
			print("Base64: " + imageData.Length);*/
        }

        texture.Compress(true);

        /*Texture2D decomp = Decompress(texture);
		print("Raw Texture Data (Decompress): " + decomp.GetRawTextureData().Length);

		bytes = decomp.EncodeToPNG();
		print("Base64 (Decompress): " + Convert.ToBase64String(bytes).Length);
		*/
        //bytes = texture.GetRawTextureData();

        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
        displayImage.sprite = newSprite;
    }

    /// <summary>
    /// https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Texture2D Decompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    public void CheckImage()
    {
        Texture2D t = Resources.Load("Image") as Texture2D;
        print(t.GetRawTextureData().Length);
    }

    public void SetGuid(string s)
    {
        sserial.SetSerial(s);
    }

    public string GetGuid()
    {
        return currentImage.GetComponent<OpenImageUploadPanelScript>().GetGuid();
        //return sserial.GetSerial (); //serial.text;
    }

    public void LoadData(Image targetImage)
    {
        if (displayImage == null) {
            Start();
        }

        SetGuid(targetImage.GetComponent<OpenImageUploadPanelScript>().GetGuid());
        currentImage = targetImage;
        LoadImage(targetImage.sprite);
        /*
		if (GetGuid().Equals(GlobalData.patientImageID)) {
			ToggleIsPatientImage(true);
		}
		*/
        //if (isPatientImage && !GlobalData.patientImageID.Equals("")) {
        /* //Load the primary image before maybe toggling (we let the opener handle that)
		if (isPatientImage && ds.GetImage(GlobalData.patientImageID) != null) {
			LoadImage(ds.GetImage(GlobalData.patientImageID).sprite);
		} else {
			LoadImage(targetImage.sprite);
		}
		*/
    }

    private void LoadImage(Sprite s)
    {
        displayImage.sprite = null;
        displayImage.sprite = s;
        bool displayImageBool = false;
        if (displayImage.sprite != null) {
            displayImageBool = true;
            row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = true;
            ResizePreview();
            displayImage.preserveAspect = true;
        }
        row0.Find("WithImage").gameObject.SetActive(displayImageBool);
        row0.Find("NoImage").gameObject.SetActive(!displayImageBool);
    }

    public void OpenFilePanel()
    {
        if (openingFileExpolorer) {
            return;
        }
        openingFileExpolorer = true;
        Cursor.visible = true;
        if (sserial.GetSerial() == null || sserial.GetSerial().Equals("")) { //|| serial.text == null || serial.text.Equals ("")) {
            sserial.SetSerial(currentImage.GetComponent<OpenImageUploadPanelScript>().GetGuid());
        }
        var fileName = FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(fileName)) {
            Debug.Log("[Open File] Canceled");
            Cursor.visible = false;
            openingFileExpolorer = false;
            return;
        }

        fileName = "file:///" + fileName;
        Debug.Log("[Open File] Selected file: " + fileName);
        openingFileExpolorer = false;
        StartCoroutine(Testing2(fileName));
        Cursor.visible = false;

        /*
        FileBrowser.OpenFilePanel("Open case file", Application.persistentDataPath, extensions, null, (bool canceled, string fileName) => {
            if (canceled) {
                Debug.Log("[Open File] Canceled");
                Cursor.visible = false;
                openingFileExpolorer = false;
                return;
            }
            fileName = "file:///" + fileName;
            Debug.Log("[Open File] Selected file: " + fileName);
            openingFileExpolorer = false;
            StartCoroutine(Testing2(fileName));
        });*/
    }

    public void ApplyImage()
    {
        if (updateThumbnail) {
            // Unity 2019.1 requires setting the sprite to null before changing it unless you import it in a very particular way 🙃
            // Otherwise, the image's proportions will be messed up
            currentImage.sprite = null;
            currentImage.sprite = displayImage.sprite;
            if (currentImage.sprite == null) {
                currentImage.color = Color.clear;

                currentImage.transform.parent.GetComponent<Image>().enabled = true;
            } else {
                currentImage.color = Color.white;

                currentImage.transform.parent.GetComponent<Image>().enabled = false;
            }
            row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = false;
        }
        string guid = "";
        guid = currentImage.GetComponent<OpenImageUploadPanelScript>().GetGuid(); //serial.text;

        /*
		if (isPatientImage) {
			if (GlobalData.patientImageID == null || GlobalData.patientImageID.Equals("")) {
				//No GUID set, so we simply have to set it. No need to worry about swapping images
				//ds.AddImg(GlobalData.patientImageID, displayImage.sprite);
				GlobalData.patientImageID = currentImage.GetComponent<OpenImageUploadPanelScript>().GetGuid();
			}
		} else {
			//If the original patient image is no longer the patient image, remove the reference to patient image
			if (guid.Equals(GlobalData.patientImageID)) {
				GlobalData.patientImageID = "";
			}

			//If patientImageID is a unique ID
			//ds.RemoveImage(GlobalData.patientImageID);
		}
		*/

        currentImage.GetComponent<OpenImageUploadPanelScript>().SetPatientImage(isPatientImage);

        Debug.Log("Saved image: " + guid);
        if (displayImage.sprite == null) {
            if (isPatientImage) {
                RemovePatientImage();
                //GlobalData.patientImageID = "";
            } else {
                Debug.Log("Removing Image: " + guid);
                ds.EncounterData.Images.Remove(guid);
            }
        } else {
            if (isPatientImage) {
                ReplacePatientImage();
            } else {
                var newSpriteScript = new SpriteHolderScript(displayImage.sprite);
                ds.EncounterData.Images.Add(newSpriteScript);
            }
        }
    }

    private void ResizePreview()
    {
        int uploadWidth = displayImage.sprite.texture.width;
        int uploadHeight = displayImage.sprite.texture.height;

        if (uploadWidth > previewMaxWidth) {
            uploadHeight = Convert.ToInt32(uploadHeight * (1.0f * previewMaxWidth / uploadWidth));
            uploadWidth = previewMaxWidth;
        }

        if (uploadHeight > previewMaxHeight) {
            uploadWidth = Convert.ToInt32(uploadWidth * (1.0f * previewMaxHeight / uploadHeight));
            uploadHeight = previewMaxHeight;
        }

        //displayImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uploadWidth, uploadHeight);
        displayImage.GetComponent<LayoutElement>().preferredWidth = uploadWidth;
        displayImage.GetComponent<LayoutElement>().preferredHeight = uploadHeight;

        if (uploadWidth < previewMinWidth) {
            uploadWidth = previewMinWidth;
        }

        if (uploadHeight < previewMinHeight) {
            uploadHeight = previewMinHeight;
        }

        //Current buffer 40w 110h
        //GameObject.Find("ImagePickerBG/ImagePicker").GetComponent<RectTransform>().sizeDelta = new Vector2(uploadWidth + previewWidthMargin, uploadHeight + previewHeightMargin);
    }

    public void RemoveImage()
    {
        //StartingUID calculation used to happen here
        //ds.GetImages ().Remove (startingUID); //save this for apply
        displayImage.sprite = null;
        row0.Find("WithImage").gameObject.SetActive(false);
        row0.Find("NoImage").gameObject.SetActive(true);
        row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = false;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
