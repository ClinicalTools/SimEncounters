namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabSelector : ReaderTabSelector, ICompletionDrawer
    {
        public void CompletionDraw(ReaderSceneInfo readerSceneInfo)
        {
            if (TabButtons[CurrentTab] is ICompletionDrawer completionDrawer)
                completionDrawer.CompletionDraw(readerSceneInfo);
        }
    }
}