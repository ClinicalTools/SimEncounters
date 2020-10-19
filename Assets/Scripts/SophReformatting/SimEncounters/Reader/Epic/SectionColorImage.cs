using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class SectionColorImage : MonoBehaviour
    {
        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected ICompletionHandler CompletionHandler { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<SectionSelectedEventArgs> sectionSelector)
        {
            SectionSelector = sectionSelector;
            SectionSelector.AddSelectedListener(OnSectionSelected);
        }

        protected virtual void OnSectionSelected(object sender, SectionSelectedEventArgs eventArgs)
            => Image.color = eventArgs.SelectedSection.Color;
    }
}