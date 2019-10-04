using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteHolderScript {

	public Sprite sprite;
	public string referenceName;
	public Transform iconHolder;
	public Color color;
	public bool useColor;

	/**
	 * Holds the data for any sprites found in the program
	 * Used by DataScript's imgDict
	 */

	/**
	 * Constructor for a unique sprite
	 */
	public SpriteHolderScript(Sprite s) {
		iconHolder = GameObject.Find ("GaudyBG").transform;
		iconHolder = iconHolder.Find ("SectionCreatorBG/SectionCreatorPanel/Content/ScrollView/Viewport/Content");
		sprite = s;
	}

	/**
	 * Constructor for section icons
	 * imgRefName = the name of the sprite
	 */
	public SpriteHolderScript(string imgRefName) {
		iconHolder = GameObject.Find ("GaudyBG").transform;
		iconHolder = iconHolder.Find ("SectionCreatorBG/SectionCreatorPanel/Content/ScrollView/Viewport/Content");
		referenceName = imgRefName;
	}

	/**
	 * Returns the sprite data in XML format
	 */
	public string GetXMLText() {
		string data = "";
		if (useColor) {
			string colorString = "";
			colorString += color.r + ",";
			colorString += color.g + ",";
			colorString += color.b + ",";
			colorString += color.a;
			data += "<iconColor>" + colorString + "</iconColor>";
		}
		if (referenceName != null) {
			return data + "<reference>" + referenceName + "</reference>";
		} else {
			if(sprite.texture.format == TextureFormat.DXT1 || sprite.texture.format == TextureFormat.DXT5) {
				Texture2D decomp = Decompress(sprite.texture);

				byte[] byteArray = decomp.EncodeToPNG();
				data += "<width>" + sprite.texture.width + "</width><height>" + sprite.texture.height + "</height><data>" + Convert.ToBase64String(byteArray) + "</data>";
				return data;
			}
			


			Texture2D newTexture;
			if (sprite.texture.format == TextureFormat.RGB24) {
				newTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGB24, false);
			} else {
				newTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
			}
			newTexture.SetPixels (0, 0, sprite.texture.width, sprite.texture.height, sprite.texture.GetPixels ());
			newTexture.Apply ();

			byte[] bytes;
			if (newTexture.format == TextureFormat.RGB24) {
				bytes = newTexture.EncodeToJPG();
			} else {
				bytes = newTexture.EncodeToPNG();
			}
			string imageData = Convert.ToBase64String (bytes);
			data += "<width>" + sprite.texture.width + "</width><height>" + sprite.texture.height + "</height><data>" + imageData + "</data>";
			return data;
		}
	}

	/// <summary>
	/// https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	public Texture2D Decompress(Texture2D source)
	{
		RenderTexture renderTex = RenderTexture.GetTemporary(
					source.width,
					source.height,
					0,
					RenderTextureFormat.Default,
					RenderTextureReadWrite.Linear);

		Graphics.Blit(source, renderTex);
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = renderTex;
		Texture2D readableText = new Texture2D(source.width, source.height);
		readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		readableText.Apply();
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(renderTex);
		return readableText;
	}
}
