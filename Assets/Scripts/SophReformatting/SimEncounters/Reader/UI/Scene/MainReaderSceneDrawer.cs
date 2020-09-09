using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class MainReaderSceneDrawer : BaseReaderSceneDrawer
    {
        public List<MonoBehaviour> ReaderObjects { get => readerObjects; }
        [SerializeField] private List<MonoBehaviour> readerObjects = new List<MonoBehaviour>();

        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();
        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionDrawer> SectionDrawers { get; } = new List<IUserSectionDrawer>();
        protected List<IUserTabDrawer> TabDrawers { get; } = new List<IUserTabDrawer>();
        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();
        protected List<IUserTabSelector> TabSelectors { get; } = new List<IUserTabSelector>();

        protected virtual void Awake()
        {
            foreach (var readerObject in ReaderObjects) {
                if (readerObject is IReaderSceneDrawer sceneDrawer)
                    SceneDrawers.Add(sceneDrawer);
                if (readerObject is IUserEncounterDrawer encounterDrawer)
                    EncounterDrawers.Add(encounterDrawer);
                if (readerObject is IUserSectionDrawer sectionDrawer)
                    SectionDrawers.Add(sectionDrawer);
                if (readerObject is IUserTabDrawer tabDrawer)
                    TabDrawers.Add(tabDrawer);
                if (readerObject is IUserSectionSelector sectionSelector)
                    SectionSelectors.Add(sectionSelector);
                if (readerObject is IUserTabSelector tabSelector)
                    TabSelectors.Add(tabSelector);
            }
        }

        public override void Display(LoadingReaderSceneInfo sceneInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}