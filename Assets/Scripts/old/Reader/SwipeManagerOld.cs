using UnityEngine;

public class SwipeManagerOld : MonoBehaviour
{
    // Amount angle can be from straight to count as a swipe
    private const float ANGLE_TOLERANCE = 15f;
    private const float ANGLE_MIN_DIST = 100f;

    private Vector2?[] touchPos = new Vector2?[15];
    private float timeSinceLastPress;

    void Start()
    {
        //ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();
        //tm = ds.GetComponent<ReaderTabManager>();
    }

    private void Update()
    {
        if (Input.touches.Length == 1 || Input.GetMouseButton(0)) {
            Vector2 newPos;
            if (Input.touches.Length == 1)
                newPos = Input.touches[0].position;
            else
                newPos = Input.mousePosition;

            TouchPosition(newPos);
        } else if (touchPos[0] != null) {
            ReleaseTouch();
        }
    }

    public void TouchPosition(Vector2 newPos)
    {
        if (touchPos[0] == null) {
            touchPos[0] = newPos;
            return;
        }

        timeSinceLastPress += Time.deltaTime;
        while (timeSinceLastPress > .01f) {
            if (touchPos[touchPos.Length - 1] != null) {
                for (int i = 0; i < touchPos.Length - 1; i++)
                    touchPos[i] = touchPos[i + 1];

                touchPos[touchPos.Length - 1] = newPos;
            } else {
                for (int i = 1; i < touchPos.Length; i++) {
                    if (touchPos[i] == null) {
                        touchPos[i] = newPos;
                        break;
                    }
                }
            }

            timeSinceLastPress -= .01f;
        }
    }

    public void ReleaseTouch()
    {
        Vector2 firstPoint = (Vector2)touchPos[0];
        Vector2 lastPoint = firstPoint;
        for (int i = touchPos.Length - 1; i >= 0; i--) {
            if (touchPos[i] != null) {
                lastPoint = (Vector2)touchPos[i];
                break;
            }
        }

        if (Mathf.Abs(firstPoint.x - lastPoint.x) > ANGLE_MIN_DIST) {
            var angle = Vector2.Angle(Vector2.left, firstPoint - lastPoint);
            if (angle < ANGLE_TOLERANCE) {
                SwipeRight();
            } else if (180 - angle < ANGLE_TOLERANCE) {
                SwipeLeft();
            }
        }

        touchPos = new Vector2?[touchPos.Length];
    }

    public void SwipeRight()
    {
        /*
        if (!OnValidScreen())
            return;

        TabInfoScript currentTab = ds.GetData(tm.getCurrentSection()).GetTabInfo(tm.getCurrentTab());

        if (currentTab.position > 0)
        {
            //This is the name of the next tab.
            string newTabName = ds.GetData(tm.getCurrentSection()).GetTabList()[currentTab.position - 1];
            tm.setTabName(newTabName);
            tm.SwitchTab(newTabName);
        }
        else
        {
            var sectionList = ds.GetSectionsList();
            var nextSectionIndex = sectionList.FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) - 1;
            if (nextSectionIndex >= 0)
            {
                string lastSection = sectionList[nextSectionIndex];
                tm.SwitchSection(lastSection, true);
            }
        }*/
    }

    public void SwipeLeft()
    {
        /*
        if (!OnValidScreen())
            return; 

        TabInfoScript currentTab = ds.GetData(tm.getCurrentSection()).GetTabInfo(tm.getCurrentTab());

        if (ds.GetData(tm.getCurrentSection()).GetTabList().Count > currentTab.position + 1)
        {
            //This is the name of the next tab.
            string newTabName = ds.GetData(tm.getCurrentSection()).GetTabList()[currentTab.position + 1];
            tm.setTabName(newTabName);
            tm.SwitchTab(newTabName);
        }
        else if (!ds.forceInOrder || ds.GetData(tm.getCurrentSection()).AllTabsVisited())
        {
            var nextSectionIndex = ds.GetSectionsList().FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) + 1;
            if (ds.GetSectionsList().Count > nextSectionIndex)
            {
                string nextSection = ds.GetSectionsList()[nextSectionIndex];
                tm.SwitchSection(nextSection);
			}
        }*/
    }

    // Ensure that there's not a popup being displayed by searching to see if there's an ongoing dim effect
    public bool OnValidScreen()
    {
        return (GameObject.Find("DimBG") == null && GameObject.Find("DimPanel") == null);
    }
}