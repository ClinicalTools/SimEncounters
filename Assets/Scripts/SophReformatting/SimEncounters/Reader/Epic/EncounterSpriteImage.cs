using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class EncounterSpriteImage : EncounterMetadataBehaviour
    {
        private Image image;
        protected Image Image
        {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected override void OnMetadataSelected(object sender, EncounterMetadata metadata) => Image.sprite = metadata.Sprite;
    }
}