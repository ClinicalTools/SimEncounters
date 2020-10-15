namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabSelector : ReaderTabSelector, ICompletionDrawer
    {
        public void CompletionDraw(ReaderSceneInfo readerSceneInfo)
        {
            if (TabButtons?.ContainsKey(CurrentTab) == true)
                TabButtons[CurrentTab].Select();
        }
    }
}