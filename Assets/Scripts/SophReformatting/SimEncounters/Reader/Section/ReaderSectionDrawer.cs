using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionDrawer : UserSectionDrawer
    {
        [SerializeField] private List<Image> sectionBorders;
        public virtual List<Image> SectionBorders { get => sectionBorders; set => sectionBorders = value; }

        public override void Display(UserSection userSection)
        {
            var color = userSection.Data.Color;
            foreach (var sectionBorder in SectionBorders)
                sectionBorder.color = color;
        }
    }
}