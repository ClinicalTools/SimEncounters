using ClinicalTools.SimEncounters.SerializationFactories;
using System;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalSpriteFactory : SpriteFactory
    {
        protected override Sprite GetSprite(Rect imageRect, string imageData)
        {
            return base.GetSprite(imageRect, imageData.Replace(' ', '+'));
        }
    }
}