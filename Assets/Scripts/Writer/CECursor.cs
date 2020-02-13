using UnityEngine;
using UnityEngine.UI;

public class CECursor : MonoBehaviour {
	public bool offScreen;

	private Image cursor;

	/**
	 * This is the script that gets attached to the cursor object to arrange the tool tip
	 */
	void Awake () {
		if (GetComponent<CanvasGroup>()) {
			GetComponent<CanvasGroup>().alpha = 1f;
		}
		//Cursor.visible = false;
	}


	// Use this for initialization
	void Start () {
		cursor = transform.Find ("Cursor").GetComponent<Image>();
		cursor.transform.localPosition = new Vector2(0, 0);
		/*
		if (!GlobalData.GDS.isMobile) {
            transform.Find("Cursor").gameObject.SetActive(true);
            transform.Find("CursorMove")?.gameObject.SetActive(false);
            transform.Find("MobileCursor")?.gameObject.SetActive(false);
			GameObject.Find("MobileCursorOff")?.SetActive(false);
			GameObject.Find("MobileCursorOn")?.SetActive(false);
		} else if (GlobalData.showMobileCursor) {
			transform.Find("MobileCursor")?.gameObject.SetActive(true);
			transform.Find("Cursor").gameObject.SetActive(false);
            transform.Find("CursorMove")?.gameObject.SetActive(false);
            GameObject.Find("MobileCursorOff")?.SetActive(true);
		} else {
			transform.Find("MobileCursor")?.gameObject.SetActive(false);
			transform.Find("Cursor").gameObject.SetActive(false);
            transform.Find("CursorMove")?.gameObject.SetActive(false);
            GameObject.Find("MobileCursorOn")?.SetActive(true);
		}*/
	}
	
	/**
	 * Calculates the position of tool tip box
	 */
	void OnGUI () {
		Vector3 mPos = Input.mousePosition;


		if (mPos.x < 0 || mPos.x > Screen.width || mPos.y < 0 || mPos.y > Screen.height+cursor.rectTransform.rect.height) {
			offScreen = true;
			cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 0);
		} else if (offScreen) { //When coming back on screen
			offScreen = false;
			cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 255);
		}
                
		if (mPos.x <= 0) {
			mPos.x = 0;
		}
		if (mPos.y >= Screen.height+cursor.rectTransform.rect.height) {
			mPos.y = Screen.height+cursor.rectTransform.rect.height;
		}

		transform.position = mPos;
	}

	public void SwitchMobileCursor()
	{
		GlobalData.showMobileCursor = !GlobalData.showMobileCursor;
		transform.Find("MobileCursor").gameObject.SetActive(GlobalData.showMobileCursor);
	}

    public void StartMoveCursor()
    {
        transform.Find("Cursor").gameObject.SetActive(false);
        transform.Find("CursorMove")?.gameObject.SetActive(true);
    }

    public void StopMoveCursor()
    {
        transform.Find("Cursor").gameObject.SetActive(true);
        transform.Find("CursorMove")?.gameObject.SetActive(false);
    }
}