namespace ClinicalTools.SimEncounters
{
    public class LoadingWriterSceneInfoSelector : Selector<LoadingWriterSceneInfoSelectedEventArgs>
    {
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; }
        public LoadingWriterSceneInfoSelector(ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector)
            => SceneInfoSelector = sceneInfoSelector;

        public override void Select(object sender, LoadingWriterSceneInfoSelectedEventArgs value)
        {
            base.Select(sender, value);
            value.SceneInfo.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<WriterSceneInfo> sceneInfoResult)
        {
            CurrentValue.SceneInfo.LoadingScreen?.Stop();
            if (sceneInfoResult.HasValue())
                SceneInfoSelector.Select(this, new WriterSceneInfoSelectedEventArgs(sceneInfoResult.Value));
        }
    }
    public class LoadingReaderSceneInfoSelector : Selector<LoadingReaderSceneInfoSelectedEventArgs>
    {
        protected ISelector<ReaderSceneInfoSelectedEventArgs> ReaderSceneInfoSelector { get; }
        public LoadingReaderSceneInfoSelector(ISelector<ReaderSceneInfoSelectedEventArgs> readerSceneInfoSelector)
            => ReaderSceneInfoSelector = readerSceneInfoSelector;

        public override void Select(object sender, LoadingReaderSceneInfoSelectedEventArgs value)
        {
            base.Select(sender, value);
            value.SceneInfo.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<ReaderSceneInfo> sceneInfoResult)
        {
            CurrentValue.SceneInfo.LoadingScreen?.Stop();
            if (sceneInfoResult.HasValue())
                ReaderSceneInfoSelector.Select(this, new ReaderSceneInfoSelectedEventArgs(sceneInfoResult.Value));
        }
    }
}