namespace ClinicalTools.SimEncounters
{
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
}