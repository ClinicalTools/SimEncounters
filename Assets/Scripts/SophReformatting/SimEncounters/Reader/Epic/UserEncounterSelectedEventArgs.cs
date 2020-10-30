using System;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterSelectedEventArgs : EventArgs
    {
        public UserEncounter Encounter { get; }
        public UserEncounterSelectedEventArgs(UserEncounter encounter) => Encounter = encounter;
    }
    public class LoadingMenuSceneInfoSelectedEventArgs : EventArgs
    {
        public LoadingMenuSceneInfo SceneInfo { get; }
        public LoadingMenuSceneInfoSelectedEventArgs(LoadingMenuSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
    public class MenuSceneInfoSelectedEventArgs : EventArgs
    {
        public MenuSceneInfo SceneInfo { get; }
        public MenuSceneInfoSelectedEventArgs(MenuSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
    public class LoadingReaderSceneInfoSelectedEventArgs : EventArgs
    {
        public LoadingReaderSceneInfo SceneInfo { get; }
        public LoadingReaderSceneInfoSelectedEventArgs(LoadingReaderSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
    public class ReaderSceneInfoSelectedEventArgs : EventArgs
    {
        public ReaderSceneInfo SceneInfo { get; }
        public ReaderSceneInfoSelectedEventArgs(ReaderSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
    public class UserDialoguePinSelectedEventArgs : EventArgs
    {
        public UserDialoguePin Pin { get; }
        public UserDialoguePinSelectedEventArgs(UserDialoguePin pin) => Pin = pin;
    }
    public class UserQuizPinSelectedEventArgs : EventArgs
    {
        public UserQuizPin Pin { get; }
        public UserQuizPinSelectedEventArgs(UserQuizPin pin) => Pin = pin;
    }
    public class EncounterMetadataSelectedEventArgs : EventArgs
    {
        public EncounterMetadata Metadata { get; }
        public EncounterMetadataSelectedEventArgs(EncounterMetadata metadata) => Metadata = metadata;
    }
    public class EncounterSelectedEventArgs : EventArgs
    {
        public Encounter Encounter { get; }
        public EncounterSelectedEventArgs(Encounter encounter) => Encounter = encounter;
    }
    public class PanelSelectedEventArgs : EventArgs
    {
        public Panel Panel { get; }
        public PanelSelectedEventArgs(Panel panel) => Panel = panel;
    }
}