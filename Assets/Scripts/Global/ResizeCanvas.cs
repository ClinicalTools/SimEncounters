using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//[InitializeOnLoad]
public class ResizeCanvas : MonoBehaviour
{
	#region variables

	#region gameObjects
	//Reader
	static Transform gaudyBG;
	static CanvasScaler canvasScaler;

	//Menu

	#endregion

	#region values
	/*
	* (variableName) : (Min (720p) value) - (Max (1080p) value)
	* contentPanel : 267.86 - 326.74
	* section : -59.4 - 0
	* tabButtons : -25.805 - 0
	* scrollView.min : 0 - -3.6545
	* scrollView.max : 53.5995 - 57.25
	* leftScrollArrow : 26.52 - 11.785
	* rightScrollArrow : 26.52 - 11.785
	* tabButtonContent : 60.778 - 79.234
	* tabContent : 25.8051 - 0
	* sidePanel : 22.7 - 109.1
	* resolution : 1280x720 - 1920x1080
	*/

	static float contentPanelMin = 267.86f;
	static float contentPanelDif = 326.74f - contentPanelMin;
	static float sectionMin = 0f;
	static float sectionDif = 0f - sectionMin;
	static float tabButtonsMin = -25.805f;
	static float tabButtonsDif = 0f - tabButtonsMin;
	static float scrollViewMinMin = 0f;
	static float scrollViewMinDif = -3.6545f - scrollViewMinMin;
	static float scrollViewMaxMin = 53.5995f;
	static float scrollViewMaxDif = 57.25f - scrollViewMaxMin;
	static float scrollArrowMin = 26.52f;
	static float scrollArrowDif = 11.785f - scrollArrowMin;
	static float tabButtonContentMin = 60.778f;
	static float tabButtonContentDif = 79.234f - tabButtonContentMin;
	static float tabContentMin = 25.8051f;
	static float tabContentDif = 0f - tabContentMin;
	static float sidePanelMin = 22.7f;
	static float sidePanelDif = 109.1f - sidePanelMin;
	static float resolutionXMin = 1440f; //1280f;
	static float resolutionXDif = 1920f - resolutionXMin;
	static float resolutionYMin = 900f; //720f;
	static float resolutionYDif = 1080f - resolutionYMin;

	static float menuResolutionXMin = 1440f;
	static float menuResolutionXDif = 1920f - menuResolutionXMin;
	static float menuResolutionYMin = 900f;
	static float menuResolutionYDif = 1080f - menuResolutionYMin;
	#endregion

	#endregion

	[Range(0.0f, 1.0f)]
	public static float resizeValue;

	//[MenuItem("Canvas Resize/1280x720")]
	public static void To720()
	{
		ScaleResize(0);
		
		#region oldcode
		/*
		Transform gaudyBG = GameObject.Find("GaudyBG").transform;
		CanvasScaler canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
		RectTransform contentPanel = gaudyBG.Find("ContentPanel ").GetComponent<RectTransform>();
		RectTransform section = gaudyBG.Find("ContentPanel /Section").GetComponent<RectTransform>();
		RectTransform tabButtons = section.Find("TabButtons").GetComponent<RectTransform>();
		RectTransform scrollView = tabButtons.Find("Scroll View").GetComponent<RectTransform>();
		RectTransform leftScrollArrow = scrollView.Find("Scrollbar/TabScrollLeft/Text").GetComponent<RectTransform>();
		RectTransform rightScrollArrow = scrollView.Find("Scrollbar/TabScrollRight/Text").GetComponent<RectTransform>();
		RectTransform tabButtonContent = scrollView.Find("Viewport/TabButtonContent").GetComponent<RectTransform>();
		RectTransform tabContent = section.Find("TabContent").GetComponent<RectTransform>();
		RectTransform sidePanel = gaudyBG.Find("SidePanel").GetComponent<RectTransform>();

		contentPanel.offsetMin = new Vector2(267.86f, contentPanel.offsetMin.y);
		//section.offsetMin = new Vector2(-59.4f, section.offsetMin.y);
		section.offsetMin = new Vector2(0f, section.offsetMin.y);
		tabButtons.offsetMin = new Vector2(tabButtons.offsetMin.x, -25.805f);
		scrollView.offsetMin = new Vector2(scrollView.offsetMin.x, -0f); //Bottom
		scrollView.offsetMax = new Vector2(scrollView.offsetMax.x, -(53.5995f)); //Top
		leftScrollArrow.offsetMin = new Vector2(leftScrollArrow.offsetMin.x, 26.52f);
		rightScrollArrow.offsetMin = new Vector2(rightScrollArrow.offsetMin.x, 26.52f);
		tabButtonContent.sizeDelta = new Vector2(tabButtonContent.rect.width, 60.778f);
		tabContent.offsetMax = new Vector2(tabContent.offsetMax.x, -(25.8051f));
		//sidePanel.GetComponent<LayoutElement>().preferredWidth = 267.86f;
		sidePanel.offsetMax = new Vector2(-(22.7f), sidePanel.offsetMax.y);

		//Debug.Log(sidePanel.offsetMin + "::" + sidePanel.offsetMax);

		canvasScaler.referenceResolution = new Vector2(1280f, 720f);
		*/
		#endregion
	}

	private void Start()
	{
		print("Resizing to: " + GlobalData.resizeVal);
		if (GlobalData.caseObj == null) {
			//Main Menu
			ScaleMenuResize(GlobalData.resizeVal);
		} else {
			//Reader
			ScaleResize(GlobalData.resizeVal);
		}
	}

	//[MenuItem("Canvas Resize/1920x1080")]
	public static void To1080()
	{
		ScaleResize(1);

		#region oldcode
		/*
		Transform gaudyBG = GameObject.Find("GaudyBG").transform;
		CanvasScaler canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
		RectTransform contentPanel = gaudyBG.Find("ContentPanel ").GetComponent<RectTransform>();
		RectTransform section = gaudyBG.Find("ContentPanel /Section").GetComponent<RectTransform>();
		RectTransform tabButtons = section.Find("TabButtons").GetComponent<RectTransform>();
		RectTransform scrollView = tabButtons.Find("Scroll View").GetComponent<RectTransform>();
		RectTransform leftScrollArrow = scrollView.Find("Scrollbar/TabScrollLeft/Text").GetComponent<RectTransform>();
		RectTransform rightScrollArrow = scrollView.Find("Scrollbar/TabScrollRight/Text").GetComponent<RectTransform>();
		RectTransform tabButtonContent = scrollView.Find("Viewport/TabButtonContent").GetComponent<RectTransform>();
		RectTransform tabContent = section.Find("TabContent").GetComponent<RectTransform>();
		RectTransform sidePanel = gaudyBG.Find("SidePanel").GetComponent<RectTransform>();

		contentPanel.offsetMin = new Vector2(326.74f, contentPanel.offsetMin.y);
		section.offsetMin = new Vector2(0f, section.offsetMin.y);
		tabButtons.offsetMin = new Vector2(tabButtons.offsetMin.x, 0f);
		scrollView.offsetMin = new Vector2(scrollView.offsetMin.x, -3.6545f); //Bottom
		scrollView.offsetMax = new Vector2(scrollView.offsetMax.x, -(57.25f)); //Top
		leftScrollArrow.offsetMin = new Vector2(leftScrollArrow.offsetMin.x, 11.785f);
		rightScrollArrow.offsetMin = new Vector2(rightScrollArrow.offsetMin.x, 11.785f);
		tabButtonContent.sizeDelta = new Vector2(tabButtonContent.rect.width, 79.234f);
		tabContent.offsetMax = new Vector2(tabContent.offsetMax.x, -(0f));
		//sidePanel.GetComponent<LayoutElement>().preferredWidth = 326.74f;
		sidePanel.offsetMax = new Vector2(-(109.1f), sidePanel.offsetMax.y);

		canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
		*/
		#endregion
	}

    public void CallResize(float value)
    {
        ScaleResize(value);
		GlobalData.resizeVal = value;
	}

	public void InGameSliderResize(Slider slider)
	{
		GlobalData.resizeVal = slider.value / slider.maxValue;
		ScaleResize(GlobalData.resizeVal);
	}

	public void IncrementSizing(int step)
	{
		GlobalData.resizeVal += step * (1f/GlobalData.resizeSteps);
		//If outside range, fix and return
		if (GlobalData.resizeVal < 0 || GlobalData.resizeVal > 1) {
			GlobalData.resizeVal = Mathf.Clamp01(GlobalData.resizeVal);
			//return;
		}

		ScaleResize(GlobalData.resizeVal);
	}

	/// <summary>
	/// Scales the canvas between a predefined min and max scale (from 720p - 1080p)
	/// </summary>
	/// <param name="value">Weight of the scaling from 0 (Small) to 1 (Big)</param>
	public static void ScaleResize(float value)
	{
		if (canvasScaler == null) {
			InitObjects();
		}
		value = 1 - value; //Invert so that 0 is small and 1 is big

		//Adjust the canvas scaler resolution
		//Everything not adjusted above is handled naturally by the canvas scaler
		canvasScaler.referenceResolution = new Vector2(resolutionXMin + value * resolutionXDif, resolutionYMin + value * resolutionYDif);
	}

	private static void InitObjects()
	{
		canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
	}

	public void IncrementMenuSizing(int step)
	{
		GlobalData.resizeVal += step * (1f / GlobalData.resizeSteps);

		//If outside range, fix and return
		if (GlobalData.resizeVal < 0 || GlobalData.resizeVal > 1) {
			GlobalData.resizeVal = Mathf.Clamp01(GlobalData.resizeVal);
			return;
		}

		ScaleMenuResize(GlobalData.resizeVal);
	}

	/// <summary>
	/// This is for scaling the menu up and down
	/// </summary>
	/// <param name="value">Weight of the scaling from 0 (Small) to 1 (Big)</param>
	public static void ScaleMenuResize(float value)
	{
		if (canvasScaler == null) {
			InitMenuObjects();
		}
		value = 1 - value; //Invert so that 0 is small and 1 is big

		canvasScaler.referenceResolution = new Vector2(menuResolutionXMin + value * menuResolutionXDif, menuResolutionYMin + value * menuResolutionYDif);
	}

	private static void InitMenuObjects()
	{
		canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
	}


	Vector2[] touches;
	float startDistance;
	float prevDistance = 0;
	float ZOOM_DEADZONE = 50;
	float DISTANCE_TO_MAX = 300;
	bool zooming = true;
	private void Update()
	{
		int tapCount = Input.touchCount;
		if (tapCount > 1) {
			touches = new Vector2[tapCount];
			for (int i = 0; i < tapCount; i++) {
				touches[i] = Input.GetTouch(i).position;
			}
			switch (Input.GetTouch(1).phase) {
				case TouchPhase.Began:
					startDistance = Vector2.Distance(touches[0], touches[1]);
					break;
				case TouchPhase.Moved:
					//Check if the zoom threshold has been passed
					CalcZoomScale();
					break;
			}
		} else {
			if (!simulate) {
				zooming = false;
			}
		}

		if (simulate) {
			if (simulation == null) {
				InitSimulation();
			}
			if (frame >= simulation.Count) {
				zooming = false;
				return;
			}
			touches = new Vector2[2];
			touches[0] = new Vector2(50, 50);
			touches[1] = simulation[frame];
			CalcZoomScale();
			frame++;
		}
	}

	int frame = 0;
	private void CalcZoomScale()
	{
		if (zooming) {
			float deltaTouch = Mathf.Abs(Vector2.Distance(touches[0], touches[1]));
			float deltaDistance = deltaTouch - prevDistance;

			GlobalData.resizeVal += deltaDistance / DISTANCE_TO_MAX;
			GlobalData.resizeVal = Mathf.Clamp01(GlobalData.resizeVal);

			//If reader
			ScaleResize(GlobalData.resizeVal);

			//If menu
			//ScaleMenuResize(GlobalData.resizeVal);

			prevDistance = deltaTouch;
		} else if (Mathf.Abs(Vector2.Distance(touches[0], touches[1]) - startDistance) >= ZOOM_DEADZONE) {
			float deltaTouch = Vector2.Distance(touches[0], touches[1]) - startDistance;
			if (deltaTouch > startDistance + ZOOM_DEADZONE) {
				prevDistance = startDistance + ZOOM_DEADZONE;
			} else {
				prevDistance = startDistance - ZOOM_DEADZONE;
			}
			zooming = true;
		};
	}

	bool simulate = false;
	private List<Vector2> simulation;

	private void InitSimulation()
	{
		simulation = new List<Vector2>();
		//GrowSimulation();
		ShrinkSimulation();
	}

	private void ShrinkSimulation()
	{
		GlobalData.resizeVal = 1;
		ScaleResize(GlobalData.resizeVal);

		startDistance = 250;
		simulation.Add(new Vector2(50, 50 + startDistance));
		for (int i = 20; i >= 0; i--) {
			simulation.Add(new Vector2(50, 100 + 10 * i));
		}
		for (int i = 0; i <= 10; i++) {
			simulation.Add(new Vector2(50, 100));
		}
		for (int i = 0; i <= 20; i++) {
			simulation.Add(new Vector2(50, 100 + 10 * i));
		}
	}

	private void GrowSimulation()
	{
		GlobalData.resizeVal = 0;
		ScaleResize(GlobalData.resizeVal);

		startDistance = 50;
		simulation.Add(new Vector2(50, 50 + startDistance));
		for (int i = 0; i <= 20; i++) {
			simulation.Add(new Vector2(50, 100 + 10 * i));
		}
		for (int i = 0; i <= 10; i++) {
			simulation.Add(new Vector2(50, 300));
		}
		for (int i = 20; i >= 0; i--) {
			simulation.Add(new Vector2(50, 100 + 10 * i));
		}
	}
}