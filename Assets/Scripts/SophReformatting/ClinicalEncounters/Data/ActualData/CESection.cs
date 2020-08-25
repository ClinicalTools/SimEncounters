using ClinicalTools.SimEncounters;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters
{
    public class CESection : Section
    {
        private static readonly Color defaultLegacyColor = new Color(.078f, .698f, .639f);

        private readonly string legacyIconKey;

        public CESection(string name, string legacyIconKey) : base(name, null, defaultLegacyColor)
        {
            this.legacyIconKey = legacyIconKey;
        }
        public void InitializeLegacyData(Dictionary<string, LegacyIcon> pairs)
        {
            if (legacyIconKey == null || !pairs.ContainsKey(legacyIconKey))
                return;

            var icon = pairs[legacyIconKey];
            if (icon.Color.a > .5f)
                Color = icon.Color;
            IconKey = icon.Reference;
        }
    }
}