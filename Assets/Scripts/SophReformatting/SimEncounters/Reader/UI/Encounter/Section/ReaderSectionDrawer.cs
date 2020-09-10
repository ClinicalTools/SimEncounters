using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionDrawer : BaseUserSectionDrawer
    {
        public virtual List<Image> SectionBorders { get => sectionBorders; set => sectionBorders = value; }
        [SerializeField] private List<Image> sectionBorders;
        public virtual TMP_Text TitleLabel { get => titleLabel; set => titleLabel = value; }
        [SerializeField] private TMP_Text titleLabel;

        public override void Display(UserSection section)
        {
            var color = section.Data.Color;
            foreach (var sectionBorder in SectionBorders)
                sectionBorder.color = color;

            if (TitleLabel != null)
                TitleLabel.text = section.Data.Name;
        }
    }
}