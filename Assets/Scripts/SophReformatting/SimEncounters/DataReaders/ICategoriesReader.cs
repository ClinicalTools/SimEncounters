using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersInfoReader
    {
        WaitableResult<IMenuEncountersInfo> GetMenuEncountersInfo(User user);
    }
    public class MenuEncountersInfoReader : IMenuEncountersInfoReader
    {
        private readonly IMenuEncountersReader menuEncountersReader;
        public MenuEncountersInfoReader(IMenuEncountersReader menuEncountersReader)
        {
            this.menuEncountersReader = menuEncountersReader;
        }

        public virtual WaitableResult<IMenuEncountersInfo> GetMenuEncountersInfo(User user)
        {
            var categories = new WaitableResult<IMenuEncountersInfo>();

            var menuEncounters = menuEncountersReader.GetMenuEncounters(user);
            menuEncounters.AddOnCompletedListener((result) => ProcessResults(user, categories, result));

            return categories;
        }
        protected virtual void ProcessResults(User user, WaitableResult<IMenuEncountersInfo> result, WaitedResult<List<MenuEncounter>> menuEncounters)
        {
            if (menuEncounters.Value == null) {
                result.SetError(null);
                return;
            }

            var menuEncountersInfo = new MenuEncountersInfo(user);
            foreach (var menuEncounter in menuEncounters.Value)
                menuEncountersInfo.AddEncounter(menuEncounter);

            result.SetResult(menuEncountersInfo);
        }
    }

    public interface ICategoriesReader
    {
        WaitableResult<List<Category>> GetCategories(User user);
    }

    public class CategoriesReader : ICategoriesReader
    {
        private readonly IMenuEncountersReader menuEncountersReader;
        public CategoriesReader(IMenuEncountersReader menuEncountersReader)
        {
            this.menuEncountersReader = menuEncountersReader;
        }

        public WaitableResult<List<Category>> GetCategories(User user)
        {
            var categories = new WaitableResult<List<Category>>();

            var menuEncounters = menuEncountersReader.GetMenuEncounters(user);
            menuEncounters.AddOnCompletedListener((result) => ProcessResults(categories, result));

            return categories;
        }

        private void ProcessResults(WaitableResult<List<Category>> result, WaitedResult<List<MenuEncounter>> menuEncounters)
        {
            if (menuEncounters.Value == null)
            {
                result.SetError(null);
                return;
            }

            var categories = new Dictionary<string, Category>();
            foreach (var menuEncounter in menuEncounters.Value)
                AddEncounterToCategories(categories, menuEncounter);
            
            result.SetResult(categories.Values.ToList());
        }

        private void AddEncounterToCategories(Dictionary<string, Category> categories, MenuEncounter menuEncounter)
        {
            if (menuEncounter.GetLatestMetadata().IsTemplate)
                return;

            foreach (var categoryName in menuEncounter.GetLatestMetadata().Categories)
            {
                Category category;
                if (!categories.ContainsKey(categoryName))
                {
                    category = new Category(categoryName);
                    categories.Add(categoryName, category);
                }
                else
                {
                    category = categories[categoryName];
                }
                category.Encounters.Add(menuEncounter);
            }
        }
    }
}