using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCompletionPopup : ReaderBehaviour, ICompletionDrawer
    {
        public abstract event Action ExitScene;
        public abstract void CompletionDraw(ReaderSceneInfo readerSceneInfo);
    }
}