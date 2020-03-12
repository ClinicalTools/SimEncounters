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

        private void EncountersInfoReader_Completed(List<EncounterDetail> encounterDetails)
        {
            foreach (var encounterDetail in encounterDetails)
                AddEncounterDetail(encounterDetail);
            IsDone = true;
            CategoriesLoaded?.Invoke(Categories);
        }

        public void AddEncounterDetail(EncounterDetail encounterDetail)
        {
            var latestInfo = encounterDetail.InfoGroup.GetLatestInfo();
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

        public EncounterDetail EncounterDetail { get; }

        public Encounter Encounter { get; private set; }

        public event Action<Encounter> EncounterLoaded;
        public bool IsDone { get; protected set; }


        public List<EncounterDetail> SuggestedEncounters { get; } = new List<EncounterDetail>();

        public InfoNeededForReaderToHappen(User user, ILoadingScreen loadingScreen, EncounterDetail encounterDetail, Encounter encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            EncounterDetail = encounterDetail;
            Encounter = encounter;
            IsDone = true;
        }

        public InfoNeededForReaderToHappen(User user, ILoadingScreen loadingScreen, EncounterDetail encounterDetail, IEncounterGetter encounterGetter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            EncounterDetail = encounterDetail;
            if (encounterGetter.IsDone) {
                Encounter = encounterGetter.Encounter;
                IsDone = true;
            } else {
                encounterGetter.Completed += EncounterGetter_Completed;
            }
        }

        private void EncounterGetter_Completed(Encounter encounter)
        {
            Encounter = encounter;
            IsDone = true;
            EncounterLoaded?.Invoke(Encounter);
        }
    }
}