using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterMetadataSerializer : IStringSerializer<EncounterMetadata>
    {
        public virtual string Serialize(EncounterMetadata metadata)
        {
            Name name;
            string url, completionCode;
            if (metadata is CEEncounterMetadata ceMetadata) {
                name = ceMetadata.Name;
                url = ceMetadata.Url;
                completionCode = ceMetadata.CompletionCode;
            } else {
                throw new Exception("This should only be called on CEEncounterMetadata");
            }

            var str = metadata.RecordNumber.ToString(); 
            str += AppendValue(metadata.AuthorAccountId.ToString());
            str += AppendValue(metadata.AuthorName);
            str += AppendValue(name);
            str += AppendValue(metadata.Difficulty.ToString());
            str += AppendValue(metadata.Description);
            str += AppendValue(metadata.Subtitle);
            str += AppendValue(metadata.Categories);
            str += AppendValue(metadata.DateModified.ToString());
            str += AppendValue(metadata.Audience);
            str += AppendValue(metadata.EditorVersion);
            str += AppendValue(metadata.IsPublic);
            str += AppendValue(metadata.IsTemplate);
            str += AppendValue(metadata.Rating.ToString());
            str += AppendValue(url);
            str += AppendValue(completionCode);

            return str;
        }

        private const string CaseInfoDivider = "|";
        private const string CategoryDivider = ";";
        protected virtual string AppendValue(bool value) => CaseInfoDivider + (value ? "1" : "0");
        protected virtual string AppendValue(string value) => CaseInfoDivider + value;
        protected virtual string AppendValue(IEnumerable<string> values) => CaseInfoDivider + string.Join(CategoryDivider, values);
        protected virtual string AppendValue(Name name) => $"{CaseInfoDivider}{name.Honorific}{CategoryDivider}{name.FirstName}{CategoryDivider}{name.LastName}";
    }
}