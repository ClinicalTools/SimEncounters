﻿using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSpriteSelector : MonoBehaviour
    {
        public abstract WaitableTask<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey);
    }
}