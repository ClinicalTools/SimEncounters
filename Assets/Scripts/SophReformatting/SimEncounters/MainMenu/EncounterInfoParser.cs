using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using static MenuCase;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DictionaryParser<TKey, TValue> : IParser<Dictionary<TKey, TValue>>
    {
        protected IStringSplitter StringSplitter { get; }
        protected IParser<KeyValuePair<TKey, TValue>> PairParser { get; }
        public DictionaryParser(IParser<KeyValuePair<TKey, TValue>> elementParser, IStringSplitter stringSplitter)
        {
            PairParser = elementParser;
            StringSplitter = stringSplitter;
        }

        public Dictionary<TKey, TValue> Parse(string text)
        {
            if (text == null)
                return null;
            var splitText = StringSplitter.Split(text);
            if (splitText == null)
                return null;

            var dict = new Dictionary<TKey, TValue>();
            foreach (var textElement in splitText)
            {
                var pair = PairParser.Parse(textElement);
                if (pair.Value != null && !dict.ContainsKey(pair.Key))
                    dict.Add(pair.Key, pair.Value);
            }

            return dict;
        }
    }
    public class ListParser<T> : IParser<List<T>>
    {
        protected IStringSplitter StringSplitter { get; }
        protected IParser<T> ElementParser { get; }
        public ListParser(IParser<T> elementParser, IStringSplitter stringSplitter)
        {
            ElementParser = elementParser;
            StringSplitter = stringSplitter;
        }

        public List<T> Parse(string text)
        {
            if (text == null)
                return null;
            var splitText = StringSplitter.Split(text);
            if (splitText == null)
                return null;

            var list = new List<T>();
            foreach (var textElement in splitText)
            {
                var element = ElementParser.Parse(textElement);
                if (element != null)
                    list.Add(element);
            }

            return list;
        }
    }

    public interface IStringSplitter
    {
        string[] Split(string str);
    }

    public interface IEncounterInfoSetter
    {
        void SetEncounterInfo(EncounterInfoGroup group, EncounterInfo info);
    }
    public class EncounterAutosaveInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterInfoGroup group, EncounterInfo info) => group.AutosaveInfo = info;
    }
    public class EncounterLocalInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterInfoGroup group, EncounterInfo info) => group.LocalInfo = info;
    }
    public class EncounterServerInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterInfoGroup group, EncounterInfo info) => group.ServerInfo = info;
    }

    public class EncounterDetailParser : IParser<EncounterDetail>
    {
        protected IEncounterInfoSetter EncounterInfoSetter { get; }

        public EncounterDetailParser(IEncounterInfoSetter encounterInfoSetter)
        {
            EncounterInfoSetter = encounterInfoSetter;
        }

        public virtual EncounterDetail Parse(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            var infoGroup = GetInfoGroup(parsedItem);
            if (infoGroup != null)
                EncounterInfoSetter.SetEncounterInfo(infoGroup, GetInfo(parsedItem));

            var recordNumber = GetRecordNumber(parsedItem);
            return new EncounterDetail(recordNumber, infoGroup);
        }

        private const int recordNumberIndex = 4;
        private int GetRecordNumber(string[] parsedItem)
        {
            if (parsedItem.Length <= recordNumberIndex)
                if (int.TryParse(parsedItem[recordNumberIndex], out var recordNumber))
                    return recordNumber;
            
            return 0;
        }

        private const string caseInfoDivider = "--";
        protected virtual string[] GetParsedEncounterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            //Split each data string of the current MenuCase, each string divided by "--"
            return text.Split(new string[] { caseInfoDivider }, StringSplitOptions.None);
        }
        private const int encounterParts = 14;

        private const int authorAccountIdIndex = 0;
        private const int filenameIndex = 1;
        private const int authorNameIndex = 2;
        private const int titleIndex = 3;
        private const int difficultyIndex = 5;
        private const int descriptionIndex = 7;
        private const int subtitleIndex = 6;
        private const int tagsIndex = 8;
        private const int dateModifiedIndex = 9;
        private const int audienceIndex = 10;
        private const int editorVersionIndex = 11;
        private const int ratingIndex = 12;
        private const int caseTypeIndex = 13;
        private const string filenameExtension = ".ced";
        public EncounterInfoGroup GetInfoGroup(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var encounterInfoGroup = new EncounterInfoGroup
            {
                Filename = GetFilename(parsedItem[filenameIndex])
            };

            if (float.TryParse(parsedItem[ratingIndex], out var rating))
                encounterInfoGroup.Rating = rating;

            return encounterInfoGroup;
        }

        protected virtual string GetFilename(string filename)
        {
            if (filename.EndsWith(filenameExtension, StringComparison.OrdinalIgnoreCase))
                filename = filename.Substring(0, filename.Length - filenameExtension.Length);
            return filename;
        }

        private const string categoryDivider = ", ";
        public EncounterInfo GetInfo(string[] parsedItem)
        {
            if (parsedItem.Length < encounterParts)
                return null;

            var encounterInfo = new EncounterInfo()
            {
                AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                Title = parsedItem[titleIndex],
                Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
                AuthorName = parsedItem[authorNameIndex],
                Description = parsedItem[descriptionIndex],
                Subtitle = parsedItem[subtitleIndex],
                DateModified = long.Parse(parsedItem[dateModifiedIndex]),
                Audience = parsedItem[audienceIndex],
                EditorVersion = parsedItem[editorVersionIndex]
            };

            encounterInfo.Categories.AddRange(parsedItem[tagsIndex]
                .Split(new string[] { categoryDivider }, StringSplitOptions.None));

            var caseType = (CaseType)int.Parse(parsedItem[caseTypeIndex]);
            encounterInfo.IsPublic = caseType == CaseType.publicCase || caseType == CaseType.publicTemplate;
            encounterInfo.IsTemplate = caseType == CaseType.publicTemplate || caseType == CaseType.privateTemplate;

            return encounterInfo;
        }

        protected Difficulty GetDifficulty(string difficulty)
        {
            if (difficulty == "Intermediate")
                return Difficulty.Intermediate;
            else if (difficulty == "Beginner")
                return Difficulty.Beginner;
            else if (difficulty == "Advanced")
                return Difficulty.Advanced;
            return Difficulty.None;
        }
    }
}