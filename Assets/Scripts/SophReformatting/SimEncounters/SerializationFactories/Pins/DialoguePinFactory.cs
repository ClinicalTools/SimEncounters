using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class DialoguePinFactory : ISerializationFactory<DialoguePin>
    {
        // pins are created by panels, so that factory can be reused
        // failure to do so can result in an infinite loop
        protected virtual PanelFactory PanelFactory { get; }

        protected virtual CollectionInfo ConversationInfo { get; } = new CollectionInfo("conversation", "panel");

        public DialoguePinFactory(PanelFactory panelFactory)
        {
            PanelFactory = panelFactory;
        }

        public virtual bool ShouldSerialize(DialoguePin value) => value != null;

        public void Serialize(XmlSerializer serializer, DialoguePin value)
        {
            serializer.AddKeyValuePairs(ConversationInfo, value.Conversation, PanelFactory);
        }

        public DialoguePin Deserialize(XmlDeserializer deserializer)
        {
            var dialoguePin = CreateDialoguePin(deserializer);

            AddConversation(deserializer, dialoguePin);

            return dialoguePin;
        }

        protected virtual DialoguePin CreateDialoguePin(XmlDeserializer deserializer) => new DialoguePin();

        protected virtual List<KeyValuePair<string, Panel>> GetConversation(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(ConversationInfo, PanelFactory);
        protected virtual void AddConversation(XmlDeserializer deserializer, DialoguePin dialoguePin)
        {
            var conversationPairs = GetConversation(deserializer);
            if (conversationPairs == null)
                return;

            foreach (var panelPair in conversationPairs)
                dialoguePin.Conversation.Add(panelPair);
        }
    }
}