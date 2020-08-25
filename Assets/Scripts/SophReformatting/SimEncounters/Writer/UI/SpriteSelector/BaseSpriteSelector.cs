using ClinicalTools.SimEncounters.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseSpriteSelector : MonoBehaviour
    {
        public abstract WaitableResult<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey);
    }
}