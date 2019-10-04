using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using Crosstales.FB;
using UnityEngine.Networking;

public class ReaderImageUploaderScript : MonoBehaviour
{
    //string filePath;
    private Image displayImage;
    private Transform row0;
    //private WWW www;
    private Texture2D texture;
    private ReaderDataScript ds;
    private Image currentImage;
    private string[] extensions = new string[] { "png", "jpg", "jpeg" };
    public ReaderSerialScript sserial;

    public int previewMinWidth = 540;
    public int previewMinHeight = 300;

    public int previewMaxWidth = 720;
    public int previewMaxHeight = 400;

    // Use this for initialization
    void Start()
    {
        row0 = transform.Find("ImageUploadPanel/Content/Row0");
        displayImage = transform.Find("ImageUploadPanel/Content/Row0/WithImage/ImageGoesHere").GetComponent<Image>();
        ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();

        sserial = new ReaderSerialScript();
    }

    private IEnumerator Testing(string filePath)
    {
        Cursor.visible = true;

        //filePath = "file://" + EditorUtility.OpenFilePanel ("Choose your image", "", "png");
        //filePath = "file://C:/Users/Will/Documents/Unity/Clinical Encounters Writer/Test Pictures/Non-Profit-Free-Download-PNG.png";
        //filePath = "http://clipart-library.com/image_gallery2/Non-Profit-Free-Download-PNG.png";

        if (!filePath.Equals("") && !filePath.Equals("file://")) {
            Debug.Log(filePath);
            Cursor.visible = false;
            int width = 320;
            int height = 435;
            texture = new Texture2D(width, height);
			//New
			UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(filePath);
			row0.Find("NoImage").gameObject.SetActive(false);
			row0.Find("UploadingImage").gameObject.SetActive(true);
			yield return webRequest.SendWebRequest();
			if (webRequest.isNetworkError || webRequest.isHttpError) {
				Debug.LogWarning(webRequest.error);
			}
			texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;


			//Old for reference
            /*www = new WWW(filePath);
            row0.Find("NoImage").gameObject.SetActive(false);
            row0.Find("UploadingImage").gameObject.SetActive(true);
            Application.backgroundLoadingPriority = ThreadPriority.High;
            StartCoroutine(LoadInTexture());
            Application.backgroundLoadingPriority = ThreadPriority.Normal;
            yield return www;
            //www.LoadImageIntoTexture (texture);
			*/


            if (texture.width == 8)
                Debug.Log("error loading texture");
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
            displayImage.sprite = newSprite;
            row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = true;
            row0.Find("UploadingImage").gameObject.SetActive(false);
            row0.Find("WithImage").gameObject.SetActive(true);
        }
        yield break;
    }

    public void SetGuid(string s)
    {
        sserial.SetSerial(s);
    }

    public string GetGuid()
    {
        return sserial.GetSerial(); //serial.text;
    }

    public void LoadData(Image targetImage)
    {
        if (displayImage == null) {
            Start();
        }
        currentImage = targetImage;
        displayImage.sprite = targetImage.sprite;
        bool displayImageBool = false;
        if (displayImage.sprite != null) {
            displayImageBool = true;
            row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = true;
            ResizePreview();
            displayImage.preserveAspect = true;
        }
        row0.Find("WithImage").gameObject.SetActive(displayImageBool);
        row0.Find("NoImage").gameObject.SetActive(!displayImageBool);

        SetGuid(targetImage.GetComponent<ReaderOpenImageUploadPanelScript>().GetGuid());
    }

    public void OpenFilePanel()
    {
        Cursor.visible = true;
        if (sserial.GetSerial() == null || sserial.GetSerial().Equals("")) { //|| serial.text == null || serial.text.Equals ("")) {
            sserial.SetSerial(currentImage.GetComponent<ReaderOpenImageUploadPanelScript>().GetGuid());
        }
        var fileName = FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(fileName)) {
            Debug.Log("[Open File] Canceled");
            Cursor.visible = false;
            return;
        }
        fileName = "file:///" + fileName;
        Debug.Log("[Open File] Selected file: " + fileName);
        StartCoroutine(Testing(fileName));
    }

    /*private IEnumerator LoadInTexture()
    {
        yield return www;
        Debug.Log("loaded, www texture width = " + www.texture.width);
        texture = www.texture;
    }*/

    public void ApplyImage()
    {
        currentImage.sprite = displayImage.sprite;
        if (currentImage.sprite == null) {
            currentImage.GetComponent<CanvasGroup>().alpha = 0f;
            currentImage.transform.parent.GetComponent<Image>().enabled = true;
        } else {
            currentImage.GetComponent<CanvasGroup>().alpha = 1f;
            currentImage.transform.parent.GetComponent<Image>().enabled = false;
        }
        row0.parent.Find("Row1/RemoveImageButton").GetComponent<Button>().interactable = false;
        string guid = currentImage.GetComponent<ReaderOpenImageUploadPanelScript>().GetGuid(); //serial.text;

        Debug.Log("Saved image: " + guid);
        if (displayImage.sprite == null) {
            Debug.Log("Removing Image: " + guid);
            ds.RemoveImage(guid);
        } else {
            ds.AddImg(guid, displayImage.sprite);
        }
    }

    private void ResizePreview()
    {
        int uploadWidth = currentImage.sprite.texture.width;
        int uploadHeight = currentImage.sprite.texture.height;

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

        /*
        if (uploadWidth < previewMinWidth) {
            uploadWidth = previewMinWidth;
        }

        if (uploadHeight < previewMinHeight) {
            uploadHeight = previewMinHeight;
        }

        //Current buffer 40w 110h
        GameObject.Find("ImagePickerBG/ImagePicker").GetComponent<RectTransform>().sizeDelta = new Vector2(uploadWidth + previewWidthMargin, uploadHeight + previewHeightMargin);
        */
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
        Destroy(this.gameObject);
    }
}
