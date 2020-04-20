using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using Zenject;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalDialoguePinFactory : DialoguePinFactory
    {
        public ClinicalDialoguePinFactory(LazyInject<ISerializationFactory<Panel>> panelFactory) : base(panelFactory) { }

        protected CollectionInfo LegacyConversationInfo { get; } =
            new CollectionInfo(
                new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData")),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );

        protected override void AddConversation(XmlDeserializer deserializer, DialoguePin dialoguePin)
        {
            base.AddConversation(deserializer, dialoguePin);
            if (dialoguePin.Conversation.Count != 0)
                return;

            var conversation = deserializer.GetList(LegacyConversationInfo, PanelFactory.Value);
            if (conversation == null)
                return;

            foreach (var panel in conversation)
                dialoguePin.Conversation.Add(panel);
        }
    }
}