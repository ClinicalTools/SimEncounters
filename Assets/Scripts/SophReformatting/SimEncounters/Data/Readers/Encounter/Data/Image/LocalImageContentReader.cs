namespace ClinicalTools.SimEncounters
{
    public class LocalImageContentReader : IImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterImageContent> parser;
        public LocalImageContentReader(IFileManager fileManager, IStringDeserializer<EncounterImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            UnityEngine.Debug.LogWarning("a1");
            var imageData = new WaitableTask<EncounterImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableTask<EncounterImageContent> result, TaskResult<string> fileText)
        {
            UnityEngine.Debug.LogWarning("c0");
            if (fileText.IsError()) {
                UnityEngine.Debug.LogWarning("c1");
                UnityEngine.Debug.LogError(fileText.Exception.Message);
                result.SetError(fileText.Exception);
            } else {
                UnityEngine.Debug.LogWarning("c2");
                UnityEngine.Debug.LogWarning(fileText.Value);
                result.SetResult(parser.Deserialize(fileText.Value));
            }
            UnityEngine.Debug.LogWarning("c3");
        }
    }
}