using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class InfoNeededForMainMenuToHappen
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }

        public Dictionary<string, Category> Categories { get; } = new Dictionary<string, Category>();

        public event Action<Dictionary<string, Category>> CategoriesLoaded;
        public bool IsDone { get; protected set; }

        public InfoNeededForMainMenuToHappen(User user, ILoadingScreen loadingScreen, IEncountersInfoReader encountersInfoReader)
        {
            User = user;
            LoadingScreen = loadingScreen;
            encountersInfoReader.Completed += EncountersInfoReader_Completed;
            encountersInfoReader.GetEncounterInfos(user);
        }

        private void EncountersInfoReader_Completed(List<EncounterInfo> encounterDetails)
        {
            foreach (var encounterDetail in encounterDetails)
                AddEncounterDetail(encounterDetail);
            IsDone = true;
            CategoriesLoaded?.Invoke(Categories);
        }

        public void AddEncounterDetail(EncounterInfo encounterDetail)
        {
            var latestInfo = encounterDetail.MetaGroup.GetLatestInfo();
            if (latestInfo.IsTemplate)
                return;

            var categories = latestInfo.Categories;
            foreach (var category in categories) {
                if (string.IsNullOrWhiteSpace(category))
                    continue;

                if (!Categories.ContainsKey(category))
                    Categories.Add(category, new Category());

                Categories[category].Encounters.Add(encounterDetail);
            }
        }
    }

    public class InfoNeededForReaderToHappen
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public Encounter Encounter { get; private set; }

        public event Action<Encounter> EncounterLoaded;
        public bool IsDone { get; protected set; }

        public List<EncounterInfo> SuggestedEncounters { get; } = new List<EncounterInfo>();

        public InfoNeededForReaderToHappen(User user, ILoadingScreen loadingScreen, Encounter encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            IsDone = true;
        }

        public InfoNeededForReaderToHappen(User user, ILoadingScreen loadingScreen, IEncounterReader encounterReader)
        {
            User = user;
            LoadingScreen = loadingScreen;
            if (encounterReader.IsDone) {
                Encounter = encounterReader.Encounter;
                IsDone = true;
            } else {
                encounterReader.Completed += EncounterRetrieved;
            }
        }

        private void EncounterRetrieved(Encounter encounter)
        {
            Encounter = encounter;
            IsDone = true;
            EncounterLoaded?.Invoke(Encounter);
        }
    }
}