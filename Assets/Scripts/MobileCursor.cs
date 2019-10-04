using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileCursor : MonoBehaviour
{
    public Image image;

    private string lastSceneName;
    void Update()
    {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Ended) {
                image.enabled = false;
                print("image false");
            } else {
                image.enabled = true;
            }

            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != lastSceneName) {
                image.enabled = false;
                lastSceneName = sceneName;
            }
        } else if (Application.platform != RuntimePlatform.WebGLPlayer) {
            image.enabled = false;
        }
    }
}
