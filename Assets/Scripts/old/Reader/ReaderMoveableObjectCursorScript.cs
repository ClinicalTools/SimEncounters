using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ReaderMoveableObjectCursorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public Sprite cursorPicture;                //Image of the standard mouse icon
    public Sprite movePicture;                  //Image of the "You can move this" mouse cursor
    public bool applyFeedbackColor = false;     //Causes the parent object to change color based on distance from starting position
    //public Transform entryParent;               //Column that is parented to all entries
    private Image cursor;                       //Reference to the Cursor object
    private bool hover;                         //Whether or not the mouse is hovering over the clickable area
    private bool drag;                          //Whether or not the entry is being dragged
    private int transformIndex;                 //The starting index of the entry when it was picked up
    private GameObject placeholder;             //Placeholder object to show where the entry will go when dropped
    private List<Transform> entries;            //Local list of sibling entries. Used for calculating positions/placeholder spot
    private ReaderEntryManagerScript hm;       //Pointer to the manager script
    private RectTransform scrollRectTransform;  //The RectTransform of the scroll bar
    private DragOverrideScript scrollScrollRect;//The ScrollRect this entry belongs to
    private RectTransform rt;                   //The RectTransform of this entry
    private float dMPos;                        //Difference between the mouse's height and the center of the entry being clicked
    private Transform container;                //The entry we're moving.

    private Color correctColor;
    private Color partialCorrectColor;
    private Color incorrectColor;

    public int correctPosition;


    /**
	 * Used for objects (mostly entries/panels used by HistoryFieldManagerScript) which need to move
	 * This script should be placed on the object that will be clicked on
	 */

    /**
	 * Initialize most variables
	 */
    void Awake()
    {
        hover = false;
        cursor = GameObject.Find("CursorContainer")?.transform.Find("Cursor").GetComponent<Image>();
        hm = transform.GetComponentInParent<ReaderEntryManagerScript>();
        if (hm.isStatic == true) {
            container = transform.parent.parent;
        } else if (hm.isQuizPanel && !transform.parent.name.StartsWith("QuizQuestionOption")) { //Awake is run before the name change to LabEntry takes effect
            container = transform.parent;
        } else if (transform.name.StartsWith("DiagnosisEntry")) {
            container = transform;
        } else {
            container = transform.parent;
        }
        entries = new List<Transform>();
        foreach (Transform entry in container.parent.GetComponentsInChildren<Transform>()) {
            if (entry.parent == container.parent && entry.name != container.name) {
                entries.Add(entry);
            }
        }

        if (hm.isQuizPanel) {
            scrollRectTransform = hm.parentTab.transform.Find("QuizEditorPanel/Content/ScrollView").GetComponent<RectTransform>();
        } /*else if(hm.isNested){
			scrollRectTransform = hm.
		} */
        else {
            scrollRectTransform = hm.parentTab.transform.Find("Scroll View").GetComponent<RectTransform>();
        }
        scrollScrollRect = scrollRectTransform.transform.GetComponent<DragOverrideScript>();
        rt = container.GetComponent<RectTransform>();
        dMPos = 0;

        correctPosition = container.GetSiblingIndex();
    }


    /**
	 * Use this for initialization of components if they did not exist previously
	 * Also add all surrounding entries to the local list of sibling entries
	 */
    void Start()
    {
        correctColor = GlobalData.GDS.correctColor;
        partialCorrectColor = GlobalData.GDS.partialCorrectColor;
        incorrectColor = GlobalData.GDS.incorrectColor;

        if (container.GetComponent<LayoutElement>() == null) {
            container.gameObject.AddComponent(typeof(LayoutElement));
        }
        if (container.GetComponent<CanvasGroup>() == null) {
            container.gameObject.AddComponent(typeof(CanvasGroup));
        }
        entries = new List<Transform>();
        foreach (Transform entry in container.parent.GetComponentsInChildren<Transform>()) {
            if (entry.parent == container.parent && entry.name != container.name) {
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
        if (entries != null) {
            entries.Clear();
            foreach (Transform entry in container.parent.GetComponentsInChildren<Transform>()) {
                if (entry.parent == container.parent && entry.name != container.name) {
                    entries.Add(entry);
                }
            }
        }
    }

    /**
	 * When the pointer enters the area above the dragable area
	 */
    public void OnPointerEnter(PointerEventData data)
    {
        if (!drag && cursor) {
            cursor.sprite = null;
            cursor.sprite = movePicture;
        }
        hover = true;
    }

    /**
	 * When the pointer exits the area above the dragable area
	 */
    public void OnPointerExit(PointerEventData data)
    {
        if (!drag && cursor) {
            cursor.sprite = null;
            cursor.sprite = cursorPicture;
            container.GetComponent<LayoutElement>().ignoreLayout = false;
        }
        hover = false;
    }

    /**
	 * When the mouse is clicked
	 */
    public void OnPointerDown(PointerEventData data)
    {
        if (!drag && hover && data.button.Equals(PointerEventData.InputButton.Left)) {
            drag = true;
            GetComponentInParent<DragOverrideScript>().enabled = false;
            foreach (Transform t in entries) {
                t.GetComponent<ReaderMoveableObjectCursorScript>().enabled = false;
            }
            transformIndex = container.GetSiblingIndex();
            container.GetComponent<CanvasGroup>().alpha = .5f;

            //Comment this line out to grab the objects by their center.
            dMPos = Input.mousePosition.y - rt.position.y;


            //Spawn placeholder
            placeholder = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Placeholder") as GameObject, container.parent);
            placeholder.transform.SetSiblingIndex(container.GetSiblingIndex());
            placeholder.name = "placeholder";

            container.SetAsLastSibling();
            container.GetComponent<LayoutElement>().ignoreLayout = true;
            //tempY = transform.position.y;
            //container.position.y = Input.mousePosition.y + tempY;
        }
    }

    /**
	 * Run every frame. Determins the world position of the entry being held as well as the placeholder's location
	 */
    void Update()
    {
        if (drag) {
            //container.GetComponent<LayoutElement> ().ignoreLayout = true;
            //Check how far down the list of sibling entrys that this entry physically is
            rt.position = new Vector3(rt.position.x, Input.mousePosition.y - dMPos, 0f);
            int pos = 0;
            foreach (Transform entry in entries) {
                if (entry.transform.localPosition.y > container.localPosition.y) {
                    pos++;
                }
            }
            placeholder.transform.SetSiblingIndex(pos); //Set the placeholder index to match

            //This is used for scrolling when held at the top/bottom of the scroll area.
            //Adjust the .01f to change the speed of the scrolling (.01 means 1% per frame as far as I can tell)
            Vector3[] corners = new Vector3[4];
            scrollRectTransform.GetWorldCorners(corners);
            if (transform.position.y > corners[1].y - 40 && scrollScrollRect.verticalNormalizedPosition < 1.0f) {
                scrollScrollRect.verticalNormalizedPosition += .01f;
            } else if (transform.position.y < corners[0].y + 40 && scrollScrollRect.verticalNormalizedPosition > 0.0f) {
                scrollScrollRect.verticalNormalizedPosition -= .01f;
            }

            //If the user let go
            if (!Input.GetMouseButton(0)) {
                drag = false;
                GetComponentInParent<DragOverrideScript>().enabled = true;
                foreach (Transform t in entries) {
                    t.GetComponent<ReaderMoveableObjectCursorScript>().enabled = true;
                }
                container.GetComponent<CanvasGroup>().alpha = 1.0f;
                container.GetComponent<LayoutElement>().ignoreLayout = false;
                //Destroy (placeholder);
                //placeholder = null;

                //Double check the position
                pos = 0;
                foreach (Transform entry in entries) {
                    if (entry.parent == container.parent) {
                        //Debug.Log ("dropped: " + container.localPosition.y + ", entry: " + entry.transform.localPosition.y);
                        if (container.localPosition.y < entry.transform.localPosition.y) {
                            pos++;
                        }
                    }
                }

                //Destroy the placeholder and set the entry to the correct new location
                Destroy(placeholder);
                placeholder = null;
                hm.MoveTo(container.gameObject, pos, transformIndex);
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    OnPointerExit(null);
                }

                if (GetComponentInParent<DiagnosisCountScript>() != null) {
                    GetComponentInParent<DiagnosisCountScript>().ReorderEntries();
                }

                //Adjust the mouse icon accordingly
                if (!hover && cursor) {
                    cursor.sprite = null;
                    cursor.sprite = cursorPicture;
                }

                //Set the color if color feedback is enabled
                if (applyFeedbackColor) {
                    NextFrame.Function(ApplyFeedbackToEntries);
                }

            }
        }
    }

    public bool SetColor()
    {
        if (!container) {
            return false;
        } else if (container.GetSiblingIndex() == correctPosition) {
            container.GetComponent<Image>().color = correctColor;

            return true;
        } else if (Math.Abs(correctPosition - container.GetSiblingIndex()) > 1) {
            container.GetComponent<Image>().color = incorrectColor;

            return false;
        } else {
            container.GetComponent<Image>().color = partialCorrectColor;

            return false;
        }
    }

    public void ApplyFeedbackToEntries()
    {
        bool allCorrect = true;

        foreach (Transform entry in entries) {
            if (entry == null) {
                return;
            }
            //if (entry.Find("ReorderBar").GetComponent<ReaderMoveableObjectCursorScript>())
            if (entry.GetComponentInChildren<ReaderMoveableObjectCursorScript>()) {
                //if (!entry.Find("ReorderBar").GetComponent<ReaderMoveableObjectCursorScript>().SetColor())
                if (!entry.GetComponentInChildren<ReaderMoveableObjectCursorScript>().SetColor()) {
                    allCorrect = false;
                }
            }
        }

        if (!SetColor()) {
            allCorrect = false;
        }

        if (hm.GetComponent<FeedbackScript>() != null) {
            if (allCorrect) {
                hm.GetComponent<FeedbackScript>().ShowResponse();

            } else {
                hm.GetComponent<FeedbackScript>().shownFeedback.SetActive(false);
            }
        }

    }
}