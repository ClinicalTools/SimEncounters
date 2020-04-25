using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class CESection : Section
    {
        private readonly string legacyIconKey;

        public CESection(string name, string legacyIconKey) : base(name, null, Color.clear)
        {
            this.legacyIconKey = legacyIconKey;
        }

        public void InitializeLegacyData(Dictionary<string, Icon> pairs)
        {
            if (legacyIconKey == null || !pairs.ContainsKey(legacyIconKey))
                return;

            var icon = pairs[legacyIconKey];
            Color = icon.Color;
            IconKey = icon.Reference;
        }
    }
}