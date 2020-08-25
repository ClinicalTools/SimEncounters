using ClinicalTools.SimEncounters.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSpriteSelector : MonoBehaviour
    {
        public abstract WaitableResult<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey);
    }
}