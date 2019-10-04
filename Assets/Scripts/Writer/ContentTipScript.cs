using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentTipScript : MonoBehaviour
{

    public Sprite defaultCursorImage;
    public bool offScreen;

    private Vector3 defaultPos;
    private Image cursor;
    private RectTransform rt;

    /**
	 * This is the script that gets attached to the cursor object to arrange the tool tip
	 */

    void Awake()
    {
        if (GetComponent<CanvasGroup>())
        {
            GetComponent<CanvasGroup>().alpha = 1f;
        }
        Cursor.visible = false;
    }


    // Use this for initialization
    void Start()
    {
        defaultPos = transform.Find("Cursor/ContentTip").GetComponent<RectTransform>().localPosition;
        cursor = transform.Find("Cursor").GetComponent<Image>();
        rt = transform.Find("Cursor/ContentTip").GetComponent<RectTransform>();
    }

    /**
	 * Calculates the position of tool tip box
	 */
    void Update()
    {
        Vector3 mPos = Input.mousePosition;
        Vector3 tipPos = defaultPos;


        if (mPos.x < 0 || mPos.x > Screen.width || mPos.y < 0 || mPos.y > Screen.height + cursor.rectTransform.rect.height)
        {
            offScreen = true;
            cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 0);
        }
        else if (offScreen)
        { //When coming back on screen
            offScreen = false;
            cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 255);
            //cursor.sprite = defaultCursorImage;
        }

        //I could not figure out why my math was not working, but these hard coded values seemed to get things close enough
        //float X = mPos.x;
        //Debug.Log (tooltipW + ", " + mPos.x + ", " + (X + tooltipW) + ", " + Screen.width);

        if ((rt.rect.width + mPos.x) > Screen.width)
        {
            tipPos.x = -(mPos.x + rt.rect.width - Screen.width);
        }

        if (mPos.x <= 0)
        {
            mPos.x = 0;
        }
        if ((mPos.y - rt.rect.height) < 4)
        {
            //mPos.y - Screen.height
            tipPos.y += rt.rect.height - mPos.y + 4;
            //mPos.y = .8f*rt.rect.height;
        }
        if (mPos.y >= Screen.height + cursor.rectTransform.rect.height)
        {
            mPos.y = Screen.height + cursor.rectTransform.rect.height;
        }

        //Manually readjust the positioning to look better
        mPos.x += 12; mPos.y -= 8;// += new Vector3 (12, -8, 0);
        transform.position = mPos;
        rt.localPosition = tipPos;// * (1/.785f);
    }
}