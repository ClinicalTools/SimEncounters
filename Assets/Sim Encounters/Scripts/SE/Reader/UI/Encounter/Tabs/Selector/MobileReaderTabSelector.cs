namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabSelector : ReaderTabSelector
    {
        // !!!!! FIX
        public void CompletionDraw(ReaderSceneInfo readerSceneInfo)
        {
            if (TabButtons?.ContainsKey(CurrentTab) == true)
                TabButtons[CurrentTab].Select();
        }
    }
}