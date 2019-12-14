using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using ClinicalTools.SimEncountersOld;

public class TabAndSectionDragScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public Sprite cursorPicture;                //Image of the standard mouse icon
    public Sprite movePicture;                  //Image of the "You can move this" mouse cursor
    private Image cursor;                       //Reference to the Cursor object
    private bool hover;                         //Whether or not the mouse is hovering over the clickable area
    private bool drag;                          //Whether or not the entry is being dragged
    private GameObject placeholder;             //Placeholder object to show where the entry will go when dropped
    private List<Transform> entries;            //Local list of sibling entries. Used for calculating positions/placeholder spot
    private RectTransform scrollRectTransform;  //The RectTransform of the scroll bar
    private ScrollRect scrollScrollRect;        //The ScrollRect this entry belongs to
    private RectTransform rt;                   //The RectTransform of this entry
    private float dMPos;                        //Difference between the mouse's height and the center of the entry being clicked
    private bool clicked;
    private TabManager tm;
    private WriterHandler ds;
    private float clickPoint;
    private string linkToText;
    private bool draggable;
    private Color regularNormalColor;
    /**
	 * Used for tabs and sections
	 * This script should be placed on the object that will be clicked on
	 */

    /**
	* Initialize most variables
	*/
    void Awake()
    {
        hover = false;
        cursor = GameObject.Find("CursorContainer").transform.Find("Cursor").GetComponent<Image>();
        entries = new List<Transform>();
        foreach (Transform entry in transform.parent.GetComponentsInChildren<Transform>()) {
            if (entry.parent == transform.parent && entry.name != transform.name && !entry.name.Equals("Filler") && !entry.name.Equals("AddTabButton") && !entry.name.Equals("AddSectionButton")) {
                entries.Add(entry);
            }
        }
        scrollRectTransform = transform.parent.parent.parent.GetComponent<RectTransform>(); //Scroll view
        scrollScrollRect = scrollRectTransform.transform.GetComponent<ScrollRect>();
        rt = transform.GetComponent<RectTransform>();
        dMPos = 0;
        clickPoint = 0;
        clicked = false;
        tm = GameObject.Find("GaudyBG").GetComponent<TabManager>();
        ds = WriterHandler.WriterInstance;
        string tabType = "";
        if (transform.Find("TabButtonLinkToText") != null) {
            linkToText = transform.Find("TabButtonLinkToText").GetComponent<TextMeshProUGUI>().text;
            tabType = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(linkToText).type;
        } else {
            linkToText = transform.Find("SectionLinkToText").GetComponent<TextMeshProUGUI>().text;

        }
        var tabInfo = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(linkToText);

        draggable = !(!linkToText.EndsWith("Section") && (tabInfo != null && tabInfo.persistant));
        if (tabType.StartsWith("Personal Info") || tabType.StartsWith("Office Visit")) {
            draggable = true;
        }

        //draggable = (draggable && !linkToText.StartsWith("Background_InfoTab")); 
        //draggable = (draggable && (Test Condition)); Use this format to add other conditionals to draggable, such as for background info
    }


    /**
	 * Use this for initialization of components if they did not exist previously
	 * Also add all surrounding entries to the local list of sibling entries
	 */
    void Start()
    {
        if (transform.GetComponent<LayoutElement>() == null) {
            transform.gameObject.AddComponent(typeof(LayoutElement));
        }
        if (transform.GetComponent<CanvasGroup>() == null) {
            transform.gameObject.AddComponent(typeof(CanvasGroup));
        }
        entries = new List<Transform>();
        foreach (Transform entry in transform.parent.GetComponentsInChildren<Transform>()) {
            if (entry.parent == transform.parent && entry.name != transform.name && !entry.name.Equals("Filler") && !entry.name.Equals("AddTabButton") && !entry.name.Equals("AddSectionButton")) {
                entries.Add(entry);
            }
        }
    }

    /**
	 * Removes an entry from the local list of entries
	 */
    public void RemoveFromEntryList(Transform t)
    {
        entries.Remove(t);
    }

    /**
	 * Update/Refresh this entry's local list of entries
	 */
    public void UpdateEntryList()
    {
        entries.Clear();
        foreach (Transform entry in transform.parent.GetComponentsInChildren<Transform>()) {
            if (entry.parent == transform.parent && entry.name != transform.parent.name && !entry.name.Equals("Filler") && !entry.name.Equals("AddTabButton") && !entry.name.Equals("AddSectionButton")) {
                entries.Add(entry);
            }
        }
    }

    /**
	 * When the pointer enters the area above the dragable area
	 */
    public void OnPointerEnter(PointerEventData data)
    {
        if (!drag) {

            //cursor.sprite = movePicture;
        }
        hover = true;
    }

    /**
	 * When the pointer exits the area above the dragable area
	 */
    public void OnPointerExit(PointerEventData data)
    {
        if (!drag) {
            cursor.sprite = cursorPicture;
            transform.GetComponent<LayoutElement>().ignoreLayout = false;
            /*if (false && clicked) { //This handled selecting a tag when clicked and dragged off (before dragging started)
				PointerEventData pointer = new PointerEventData (EventSystem.current);
				ExecuteEvents.Execute (transform.gameObject, pointer, ExecuteEvents.submitHandler);
				clicked = false;
			}*/
        }
        hover = false;
    }

    /**
	 * When the mouse is clicked
	 */
    public void OnPointerDown(PointerEventData data)
    {
        if (hover) {
            dMPos = Input.mousePosition.x - rt.position.x;
            clickPoint = Input.mousePosition.x;
            clicked = true;
            return;
        }
    }

    /**
	 * Run every frame. Determins the world position of the entry being held as well as the placeholder's location
	 */
    void Update()
    {
        if (clicked) {
            if (Math.Abs(Input.mousePosition.x - clickPoint) > 30 && Input.GetMouseButton(0) && draggable) {
                UpdateEntryList();
                drag = true;
                clicked = false;

                Color highlight = GetComponent<Button>().colors.highlightedColor;
                ColorBlock buttonColors = GetComponent<Button>().colors;
                buttonColors.highlightedColor = new Color(highlight.r, highlight.g, highlight.b, 1f);
                regularNormalColor = buttonColors.normalColor;
                buttonColors.normalColor = new Color(1f, 1f, 1f);
                GetComponent<Button>().colors = buttonColors;

                transform.GetComponent<CanvasGroup>().alpha = .5f;

                placeholder = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Placeholder") as GameObject, transform.parent);
                transform.GetComponent<LayoutElement>().ignoreLayout = true;

                placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
                placeholder.name = "placeholder";
                placeholder.GetComponent<LayoutElement>().preferredWidth = rt.rect.width;// - transform.parent.GetComponent<HorizontalLayoutGroup> ().spacing * 2;
                placeholder.GetComponent<LayoutElement>().flexibleWidth = 0f;
                transform.GetComponent<ScriptButtonFixScript>().FixPlaceholder();
                LayoutRebuilder.ForceRebuildLayoutImmediate(placeholder.transform.parent.parent.GetComponent<RectTransform>());

                transform.SetAsLastSibling();
            } else if (!Input.GetMouseButton(0)) {
                //tm.SwitchTab (linkToText);
                clicked = false;
            }
        } else if (drag) {
            //transform.parent.GetComponent<LayoutElement> ().ignoreLayout = true;
            //Check how far down the list of sibling entrys that this entry physically is
            rt.position = new Vector3(Input.mousePosition.x - dMPos, rt.position.y, 0f);
            int pos = 0;
            foreach (Transform entry in entries) {
                if (entry.transform.position.x < transform.position.x) {
                    pos++;
                } else if (draggable) {

                } else if (!linkToText.EndsWith("Section") && ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(entry.Find("TabButtonLinkToText").GetComponent<TextMeshProUGUI>().text).persistant) {                    //If you're to the left of a persistant tab
                    pos++;
                } else if (!linkToText.EndsWith("Section") && entry.name.Equals("BackgroundData")) {
                    pos++;
                }
            }
            placeholder.transform.SetSiblingIndex(pos); //Set the placeholder index to match
            Rect r = placeholder.GetComponent<RectTransform>().rect;
            r.Set(r.x, r.y, rt.rect.width, r.height);

            //This is used for scrolling when held at the top/bottom of the scroll area.
            //Adjust the .01f to change the speed of the scrolling (.01 means 1% per frame as far as I can tell)
            Vector3[] corners = new Vector3[4];
            scrollRectTransform.GetWorldCorners(corners);
            if (transform.position.x > corners[2].x - 40 && scrollScrollRect.verticalNormalizedPosition < 1.0f) {
                scrollScrollRect.verticalNormalizedPosition += .01f;
            } else if (transform.position.x < corners[0].x + 40 && scrollScrollRect.verticalNormalizedPosition > 0.0f) {
                scrollScrollRect.verticalNormalizedPosition -= .01f;
            }

            //If the user let go
            if (!Input.GetMouseButton(0)) {
                drag = false;

                ColorBlock buttonColors = GetComponent<Button>().colors;
                Color highlight = buttonColors.highlightedColor;
                buttonColors.highlightedColor = new Color(highlight.r, highlight.g, highlight.b, (200 / 255f));
                buttonColors.normalColor = regularNormalColor;
                GetComponent<Button>().colors = buttonColors;

                transform.GetComponent<CanvasGroup>().alpha = 1.0f;
                transform.GetComponent<LayoutElement>().ignoreLayout = false;

                //Double check the position
                pos = 0;
                foreach (Transform entry in entries) {
                    //Debug.Log ("dropped: " + transform.parent.localPosition.y + ", entry: " + entry.transform.localPosition.y);
                    if (transform.position.x > entry.transform.position.x) {
                        pos++;
                    } /*else if (!linkToText.EndsWith("Section") && ds.GetData (tm.getCurrentSection ()).GetTabInfo (entry.Find ("TabButtonLinkToText").GetComponent<Text> ().text).persistant) {
						//If you're to the left of a persistant tab
						pos++;
					}*/
                }

                //Destroy the placeholder and set the entry to the correct new location
                transform.SetSiblingIndex(pos);
                placeholder.transform.SetAsLastSibling();
                Destroy(placeholder);
                placeholder = null;

                //Adjust the mouse icon accordingly
                if (!hover) {
                    cursor.sprite = cursorPicture;
                }


                List<Transform> buttons = new List<Transform>();
                for (int i = 0; i < transform.parent.childCount; i++) {
                    if (!transform.parent.GetChild(i).name.Equals("placeholder") 
                        && !transform.parent.GetChild(i).name.Equals("Filler") 
                        && !transform.parent.GetChild(i).name.Equals("AddTabButton") 
                        && !transform.parent.GetChild(i).name.Equals("AddSectionButton")) {
                        buttons.Add(transform.parent.GetChild(i));
                    }
                }

                foreach (Transform t in buttons) {
                    if (!linkToText.EndsWith("Section")) {
                        //Debug.Log (t.name + ", SETTING POSITION: " + t.GetSiblingIndex() + ", FROM" + ds.GetData(tm.getCurrentSection ()).GetTabInfo (t.Find("TabButtonLinkToText").GetComponent<Text>().text).position);
                        var label = t.Find("TabButtonDisplayText");
                        if (label != null) {
                            ds.EncounterData.OldSections[tm.GetCurrentSectionKey()]
                                .GetTabInfo(label.GetComponent<TextMeshProUGUI>().text)
                                .SetPosition(t.GetSiblingIndex());
                        } else if (!t.name.Equals("AddSectionButton")) {
                            ds.EncounterData.OldSections[t.Find("SectionLinkToText")
                                .GetComponent<TextMeshProUGUI>().text]
                                .SetPosition(t.GetSiblingIndex());
                        }
                    } else if (!t.name.Equals("AddSectionButton")) {
                            ds.EncounterData.OldSections[t.Find("SectionLinkToText")
                                .GetComponent<TextMeshProUGUI>().text]
                                .SetPosition(t.GetSiblingIndex());
                    }
                }
                transform.GetComponent<ScriptButtonFixScript>().FixPlaceholder();
                transform.GetComponent<Button>().OnDeselect(null);
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(transform.gameObject, pointer, ExecuteEvents.pointerEnterHandler);

            }
        }
    }
}
