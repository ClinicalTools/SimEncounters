using UnityEngine;

public class GameResolution : MonoBehaviour {
    private struct Res {
        public readonly int Width, Height;

        public Res(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    private void Start()
    {
        windowedRes = new Res(PlayerPrefs.GetInt("WindowWidth"), PlayerPrefs.GetInt("WindowHeight"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
            ToggleFullscreen();
    }

    private static Res fullscreenRes;
    private static Res windowedRes;
    public void ToggleFullscreen()
    {
        if (fullscreenRes.Width == 0 || fullscreenRes.Height == 0) {
            var res = Screen.resolutions[Screen.resolutions.Length - 1];
            fullscreenRes = new Res(res.width, res.height);
        }
        if (windowedRes.Width == 0 || windowedRes.Height == 0)
            windowedRes = fullscreenRes;
        if (Screen.fullScreen) {
            fullscreenRes = new Res(Screen.width, Screen.height);
            Screen.SetResolution(windowedRes.Width, windowedRes.Height, false);
        } else {
            windowedRes = new Res(Screen.width, Screen.height);
            Screen.SetResolution(fullscreenRes.Width, fullscreenRes.Height, true);
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("WindowWidth", windowedRes.Width);
        PlayerPrefs.SetInt("WindowHeight", windowedRes.Height);
        PlayerPrefs.Save();
    }
}
