using System;
using UnityEngine;

public class WidthSizeGetter
{

}

public class SetDimensionsBasedOnResolution : MonoBehaviour
{
    public RectTransform Canvas { get => canvas; set => canvas = value; }
    [SerializeField] private RectTransform canvas;

    public Vector2 LandscapeDimensions { get => landscapeDimensions; set => landscapeDimensions = value; }
    [SerializeField] private Vector2 landscapeDimensions;
    public Vector2 PortraitDimensions { get => portraitDimensions; set => portraitDimensions = value; }
    [SerializeField] private Vector2 portraitDimensions;

    private readonly Vector2 LandscapeScreenDimensions = new Vector2(1920, 1080);
    private readonly Vector2 PortraitScreenDimensions = new Vector2(1080, 1920);
    protected virtual float GetSlope(Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
    {
        var y1 = GetPercentage(PortraitDimensions, PortraitScreenDimensions, primaryValueGetter);
        var y2 = GetPercentage(LandscapeDimensions, LandscapeScreenDimensions, primaryValueGetter);
        var x1 = GetProportion(PortraitScreenDimensions, primaryValueGetter, secondaryValueGetter);
        var x2 = GetProportion(LandscapeScreenDimensions, primaryValueGetter, secondaryValueGetter);
        return (y1 - y2) / (x1 - x2);
    }
    protected virtual float GetProportion(Vector2 dimensions, Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
        => secondaryValueGetter(dimensions) / primaryValueGetter(dimensions);
    protected virtual float GetPercentage(Vector2 dimensions, Vector2 screenDimension, Func<Vector2, float> primaryValueGetter)
        => primaryValueGetter(dimensions) / primaryValueGetter(screenDimension);

    protected virtual float GetSize(Vector2 dimensions, Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
    {
        var slope = GetSlope(primaryValueGetter, secondaryValueGetter);
        var y1 = GetPercentage(PortraitDimensions, PortraitScreenDimensions, primaryValueGetter);
        var x1 = GetProportion(PortraitScreenDimensions, primaryValueGetter, secondaryValueGetter);
        var x = GetProportion(PortraitScreenDimensions, primaryValueGetter, secondaryValueGetter);

        var proportion = slope * (x - x1) + y1;

        return proportion * primaryValueGetter(dimensions);
    }
    protected virtual float GetX(Vector2 vector) => vector.x;
    protected virtual float GetY(Vector2 vector) => vector.y;


    private Vector2 canvasSize = new Vector2();
    protected virtual void Awake() => UpdateSize();

    protected virtual void Update() => UpdateSize();

    protected virtual void UpdateSize()
    {
        var canvasRect = Canvas.rect;
        var currentCanvasSize = new Vector2(canvasRect.width, canvasRect.height);
        if (currentCanvasSize == canvasSize)
            return;

        canvasSize = currentCanvasSize;

        var rectTrans = (RectTransform)transform;
        var sizeDelta = rectTrans.sizeDelta;
        sizeDelta.x = GetSize(canvasSize, GetX, GetY);
        sizeDelta.y = GetSize(canvasSize, GetY, GetX);
        rectTrans.sizeDelta = sizeDelta;
    }
}
