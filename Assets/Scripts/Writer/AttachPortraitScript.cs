using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AttachPortraitScript : MonoBehaviour {
    private static string portraitPath;

	// Use this for initialization
	void Start () {
        GameObject test = GameObject.Find("ImageBG");
        portraitPath = Application.dataPath + "/Resources/Writer/Portrait/ScreenShot.png";
        test.GetComponent<Image>().sprite = newSprite();

    }

    Sprite newSprite()
    {
        Texture2D SpriteTexture = LoadTexture(portraitPath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), 100.0f);

        return NewSprite;
    }

    Texture2D LoadTexture(string FilePath)
    {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

 

    
	// Update is called once per frame
	void Update () {
		
	}
}
