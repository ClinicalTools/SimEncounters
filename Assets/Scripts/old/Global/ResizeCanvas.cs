using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[InitializeOnLoad]
public class ResizeCanvas : MonoBehaviour
{
    static CanvasScaler canvasScaler;

    static float resolutionXMin = 1440f; //1280f;
    static float resolutionXDif = 1920f - resolutionXMin;
    static float resolutionYMin = 900f; //720f;
    static float resolutionYDif = 1080f - resolutionYMin;

    static float menuResolutionXMin = 1440f;
    static float menuResolutionXDif = 1920f - menuResolutionXMin;
    static float menuResolutionYMin = 900f;
    static float menuResolutionYDif = 1080f - menuResolutionYMin;

    [Range(0.0f, 1.0f)]
    public static float resizeValue;

    private void Start()
    {
        print("Resizing to: " + GlobalData.resizeVal);
        if (GlobalData.caseObj == null)
            ScaleMenuResize(GlobalData.resizeVal);
        else
            ScaleResize(GlobalData.resizeVal);
    }

    public static void To720() => ScaleResize(0);
    public static void To1080() => ScaleResize(1);

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
        GlobalData.resizeVal += step * (1f / GlobalData.resizeSteps);
        //If outside range, fix and return
        if (GlobalData.resizeVal < 0 || GlobalData.resizeVal > 1)
            GlobalData.resizeVal = Mathf.Clamp01(GlobalData.resizeVal);

        ScaleResize(GlobalData.resizeVal);
    }

    /// <summary>
    /// Scales the canvas between a predefined min and max scale (from 720p - 1080p)
    /// </summary>
    /// <param name="value">Weight of the scaling from 0 (Small) to 1 (Big)</param>
    public static void ScaleResize(float value)
    {
        if (canvasScaler == null)
            InitObjects();

        value = 1 - value; //Invert so that 0 is small and 1 is big

        //Adjust the canvas scaler resolution
        //Everything not adjusted above is handled naturally by the canvas scaler
        canvasScaler.referenceResolution = new Vector2(resolutionXMin + value * resolutionXDif, resolutionYMin + value * resolutionYDif);
    }

    private static void InitObjects()
        => canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();

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
        if (canvasScaler == null)
            InitMenuObjects();

        value = 1 - value; //Invert so that 0 is small and 1 is big

        canvasScaler.referenceResolution = new Vector2(menuResolutionXMin + value * menuResolutionXDif, menuResolutionYMin + value * menuResolutionYDif);
    }

    private static void InitMenuObjects()
        => canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();


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