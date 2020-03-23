using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class StringParser : IParser<string>
    {
        protected IStringSplitter StringSplitter { get; }
        
        public string Parse(string text) => text;
    }
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
            foreach (var textElement in splitText) {
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
            foreach (var textElement in splitText) {
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
        void SetEncounterInfo(EncounterMetaGroup group, EncounterMetadata info);
    }
    public class EncounterAutosaveInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterMetaGroup group, EncounterMetadata info) => group.AutosaveInfo = info;
    }
    public class EncounterLocalInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterMetaGroup group, EncounterMetadata info) => group.LocalInfo = info;
    }
    public class EncounterDemoInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterMetaGroup group, EncounterMetadata info) => group.DemoInfo = info;
    }
    public class EncounterServerInfoSetter : IEncounterInfoSetter
    {
        public void SetEncounterInfo(EncounterMetaGroup group, EncounterMetadata info) => group.ServerInfo = info;
    }
}