using Crosstales.FB;
using System;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ImageUploader : IApply<Sprite>
    {
        public event Action<Sprite> Apply;

        protected virtual Sprite Image { get; set; }

        public ImageUploader(ImageUploaderUI imageUploaderUI)
        {
            LinkUI(imageUploaderUI);
        }

        public ImageUploader(ImageUploaderUI imageUploaderUI, Sprite image)
        {
            Image = image;
            LinkUI(imageUploaderUI);
        }

        protected void LinkUI(ImageUploaderUI imageUploaderUI)
        {
            imageUploaderUI.ApplyButton.onClick.AddListener(() => ApplyClicked(imageUploaderUI.gameObject));
            imageUploaderUI.CancelButton.onClick.AddListener(() => Close(imageUploaderUI.gameObject));

            imageUploaderUI.RemoveImageButton.onClick.AddListener(() => SetImage(imageUploaderUI, null));
            imageUploaderUI.UploadImageButton.onClick.AddListener(() => UploadImage(imageUploaderUI));
        }

        protected void ApplyClicked(GameObject gameObject)
        {
            Apply?.Invoke(Image);
            Close(gameObject);
        }

        protected virtual void Close(GameObject gameObject)
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        protected virtual void SetImage(ImageUploaderUI imageUploaderUI, Sprite image)
        {
            Image = image;

            var hasImage = Image == null;
            imageUploaderUI.NoImageGameObject.SetActive(!hasImage);
            imageUploaderUI.HasImageGameObject.SetActive(hasImage);
            if (hasImage)
                imageUploaderUI.ImageObject.sprite = Image;
        }

        protected virtual void UploadImage(ImageUploaderUI imageUploaderUI)
        {
            var image = GetImage();
            if (image != null)
                SetImage(imageUploaderUI, image);
        }

        public virtual TextureFormat TextureFormat { get; } = TextureFormat.RGBA32;
        public virtual int MaxWidth { get; } = 990; //720, 990
        public virtual int MaxHeight { get; } = 550; //400, 550
        protected virtual Sprite GetImage()
        {
            var filePath = GetImagePath();
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2, TextureFormat, false);
            texture.LoadImage(bytes);
            ScaleTexture(texture, MaxHeight, MaxWidth);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
        }

        protected void ScaleTexture(Texture2D texture, int maxHeight, int maxWidth)
        {
            //Scale to max file size
            var maxRatio = maxHeight / maxWidth;
            var ratio = texture.height / texture.width;

            if (ratio >= maxRatio) {
                if (texture.height > maxHeight) {
                    var width = GetFittedSecondDimention(texture.height, maxHeight, texture.width);
                    TextureScale.Bilinear(texture, width, maxHeight);
                }
            } else {
                if (texture.width > maxWidth) {
                    var height = GetFittedSecondDimention(texture.width, maxWidth, texture.height);
                    TextureScale.Bilinear(texture, maxWidth, height);
                }
            }
        }

        protected int GetFittedSecondDimention(int firstDimention, int firstDimentionMax, int secondDimention)
        {
            float ratio = (float)firstDimentionMax / firstDimention;
            return (int)(secondDimention * ratio);
        }

        protected virtual string[] ImageExtensions { get; } = new string[] { "png", "jpg", "jpeg" };
        protected virtual string GetImagePath() => FileBrowser.OpenSingleFile("Open Image", "", ImageExtensions);
    }
}