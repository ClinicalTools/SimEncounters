using UnityEngine;
using System.Collections;

public class WriterEventManager {
	public delegate void OverlayDelegate();
	public static event OverlayDelegate ShowOverlayRequest;

    public static void ShowOverlay(){
		ShowOverlayRequest ();
	}

	public static event OverlayDelegate HideOverlayRequest;


	public static void HideOverlay(){
		HideOverlayRequest ();
	}
}
