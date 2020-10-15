using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface ISelector<T>
    {
        void Select(object sender, T eventArgs);
        void AddSelectedListener(SelectedHandler<T> handler);
        void RemoveSelectedListener(SelectedHandler<T> handler);
    }

    public delegate void SelectedHandler<T>(object sender, T e);
    public class Selector<T> : ISelector<T>
    {
        public event SelectedHandler<T> Selected;

        protected virtual object CurrentSender { get; set; }
        protected virtual T CurrentValue { get; set; }

        public virtual void Select(object sender, T value)
        {
            CurrentSender = sender;
            CurrentValue = value;
            Selected?.Invoke(sender, value);
        }

        public virtual void AddSelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            if (CurrentValue != null)
                handler(CurrentSender, CurrentValue);
        }

        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) => Selected -= handler;
    }

    public class LoadingReaderSceneInfoSelector : Selector<LoadingReaderSceneInfo>
    {
        protected ISelector<ReaderSceneInfo> ReaderSceneInfoSelector { get; }
        public LoadingReaderSceneInfoSelector(ISelector<ReaderSceneInfo> readerSceneInfoSelector)
            => ReaderSceneInfoSelector = readerSceneInfoSelector;

        public override void Select(object sender, LoadingReaderSceneInfo value)
        {
            base.Select(sender, value);
            value.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<ReaderSceneInfo> sceneInfoResult)
        {
            if (sceneInfoResult.HasValue())
                ReaderSceneInfoSelector.Select(this, sceneInfoResult.Value);
        }
    }

    public class ReaderSceneInfoSelector : Selector<ReaderSceneInfo>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; }
        public ReaderSceneInfoSelector(ISelector<UserEncounterSelectedEventArgs> userEncounterSelector)
            => UserEncounterSelector = userEncounterSelector;

        public override void Select(object sender, ReaderSceneInfo value)
        {
            base.Select(sender, value);
            UserEncounterSelector.Select(this, new UserEncounterSelectedEventArgs(value.Encounter));
        }
    }

    public class UserEncounterSelector : Selector<UserEncounterSelectedEventArgs>
    {
        protected ISelector<Encounter> EncounterSelector { get; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; }
        public UserEncounterSelector(
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<Encounter> encounterSelector)
        {
            UserSectionSelector = userSectionSelector;
            EncounterSelector = encounterSelector;
        }
        public override void Select(object sender, UserEncounterSelectedEventArgs value)
        {
            base.Select(sender, value);
            var sectionArgs = new UserSectionSelectedEventArgs(value.Encounter.GetCurrentSection(), ChangeType.JumpTo);
            UserSectionSelector.Select(this, sectionArgs);
            EncounterSelector.Select(this, value.Encounter.Data);
        }
    }
    public class UserSectionSelector : Selector<UserSectionSelectedEventArgs>
    {
        protected ISelector<Section> SectionSelector { get; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; }
        public UserSectionSelector(
            ISelector<UserTabSelectedEventArgs> userSectionSelector,
            ISelector<Section> sectionSelector)
        {
            UserTabSelector = userSectionSelector;
            SectionSelector = sectionSelector;
        }
        public override void Select(object sender, UserSectionSelectedEventArgs value)
        {
            base.Select(sender, value);
            var tabArgs = new UserTabSelectedEventArgs(value.SelectedSection.GetCurrentTab(), value.ChangeType);
            UserTabSelector.Select(this, tabArgs);
            SectionSelector.Select(this, value.SelectedSection.Data);
        }
    }
    public class UserTabSelector : Selector<UserTabSelectedEventArgs>
    {
        protected ISelector<Tab> TabSelector { get; }
        public UserTabSelector(ISelector<Tab> tabSelector)
            => TabSelector = tabSelector;

        public override void Select(object sender, UserTabSelectedEventArgs value)
        {
            base.Select(sender, value);
            TabSelector.Select(this, value.SelectedTab.Data);
        }
    }
    public class EncounterSelector : Selector<Encounter>
    {
        protected ISelector<EncounterMetadata> MetadataSelector { get; }
        public EncounterSelector(ISelector<EncounterMetadata> metadataSelector)
            => MetadataSelector = metadataSelector;

        public override void Select(object sender, Encounter value)
        {
            base.Select(sender, value);
            MetadataSelector.Select(this, value.Metadata);
        }
    }


    public class ReaderSelectorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelector<LoadingReaderSceneInfo>>().To<LoadingReaderSceneInfoSelector>().AsTransient();
            Container.Bind<ISelector<ReaderSceneInfo>>().To<ReaderSceneInfoSelector>().AsTransient();

            Container.Bind<ISelector<UserEncounterSelectedEventArgs>>().To<UserEncounterSelector>().AsTransient();
            Container.Bind<ISelector<UserSectionSelectedEventArgs>>().To<UserSectionSelector>().AsTransient();
            Container.Bind<ISelector<UserTabSelectedEventArgs>>().To<UserTabSelector>().AsTransient();

            Container.Bind<ISelector<Encounter>>().To<EncounterSelector>().AsTransient();

            Container.Bind<ISelector<EncounterMetadata>>().To<Selector<EncounterMetadata>>().AsTransient();
            Container.Bind<ISelector<Section>>().To<Selector<Section>>().AsTransient();
            Container.Bind<ISelector<Tab>>().To<Selector<Tab>>().AsTransient();
        }
    }

    public class UserEncounterSelectedEventArgs : EventArgs
    {
        public UserEncounter Encounter { get; }
        public UserEncounterSelectedEventArgs(UserEncounter encounter) => Encounter = encounter;
    }
}