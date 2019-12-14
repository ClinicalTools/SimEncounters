using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class ImagesData
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual Dictionary<string, Icon> Icons { get; } = new Dictionary<string, Icon>();

        public ImagesData() { }
    }
}