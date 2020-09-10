using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderProgressBar : ReaderBehaviour, IUserTabDrawer
    {
        public Image FillImage { get => fillImage; set => fillImage = value; }
        [SerializeField] private Image fillImage;

        private float tabCount;

        public virtual void Display(UserTab tab)
        {
            var nonImageContent = tab.Encounter.Data.Content.NonImageContent;
            if (tabCount == 0)
                tabCount = nonImageContent.GetTabCount();
            var tabNumber = nonImageContent.GetCurrentTabNumber();

            FillImage.fillAmount = tabNumber / tabCount;
        }
    }
}
