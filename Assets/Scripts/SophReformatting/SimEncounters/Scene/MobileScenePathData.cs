namespace ClinicalTools.SimEncounters
{
    public class MobileScenePathData : IScenePathData
    {
        public string MainMenuPath => "MobileMainMenu";
        public string ReaderPath => "MobileCassReader";
        public string WriterPath => null;
    }
    public class DesktopScenePathData : IScenePathData
    {
        public string MainMenuPath => "DesktopMainMenu";
        public string ReaderPath => "MobileCassReader";
        public string WriterPath => "Writer";
    }
}