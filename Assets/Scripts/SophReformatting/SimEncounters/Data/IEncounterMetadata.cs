using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public interface IEncounterMetadata
    {
        string Audience { get; set; }
        int AuthorAccountId { get; set; }
        string AuthorName { get; set; }
        List<string> Categories { get; }
        long DateModified { get; set; }
        string Description { get; set; }
        Difficulty Difficulty { get; set; }
        string EditorVersion { get; set; }
        string Filename { get; set; }
        bool IsPublic { get; set; }
        bool IsTemplate { get; set; }
        float Rating { get; set; }
        int RecordNumber { get; set; }
        string Subtitle { get; set; }
        string Title { get; set; }
    }
}