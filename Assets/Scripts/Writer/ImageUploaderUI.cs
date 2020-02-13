using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ImageUploaderUI : MonoBehaviour
    {
        [field: SerializeField] public virtual Button CancelButton { get; set; }
        [field: SerializeField] public virtual Button ApplyButton { get; set; }
        [field: SerializeField] public virtual Button UploadImageButton { get; set; }
        [field: SerializeField] public virtual Button RemoveImageButton { get; set; }

        [field: SerializeField] public virtual GameObject NoImageGameObject { get; set; }
        [field: SerializeField] public virtual GameObject HasImageGameObject { get; set; }
        [field: SerializeField] public virtual Image ImageObject { get; set; }
    }
}