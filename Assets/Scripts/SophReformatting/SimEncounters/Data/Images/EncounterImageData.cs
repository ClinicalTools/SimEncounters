using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class CEEncounterImageData : EncounterImageData
    {
        public virtual Dictionary<string, Icon> LegacyIconsInfo { get; } = new Dictionary<string, Icon>();
    }

    public class EncounterImageData
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual KeyedCollection<Sprite> Icons { get; } = new KeyedCollection<Sprite>();

        protected virtual string DefaultIconsFolder { get; } = "Section Icons";

        public EncounterImageData()
        {
            var iconSprites = Resources.LoadAll<Sprite>(DefaultIconsFolder);
            foreach (var iconSprite in iconSprites) 
                Icons.Add(iconSprite.name, iconSprite);
        }
    }
}