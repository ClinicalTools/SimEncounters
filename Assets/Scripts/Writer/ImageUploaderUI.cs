using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ImageUploaderUI : MonoBehaviour
    {
        public virtual Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public virtual Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public virtual Button UploadImageButton { get => uploadImageButton; set => uploadImageButton = value; }
        [SerializeField] private Button uploadImageButton;
        public virtual Button RemoveImageButton { get => removeImageButton; set => removeImageButton = value; }
        [SerializeField] private Button removeImageButton;
        public virtual GameObject NoImageGameObject { get => noImageGameObject; set => noImageGameObject = value; }
        [SerializeField] private GameObject noImageGameObject;
        public virtual GameObject HasImageGameObject { get => hasImageGameObject; set => hasImageGameObject = value; }
        [SerializeField] private GameObject hasImageGameObject;
        public virtual Image ImageObject { get => imageObject; set => imageObject = value; }
        [SerializeField] private Image imageObject;
    }
}