using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalPinDataFactory : PinDataFactory
    {
        protected override DialoguePinFactory DialoguePinFactory { get; }
        protected override QuizPinFactory QuizPinFactory { get; }

        public ClinicalPinDataFactory(PanelFactory panelFactory) : base(panelFactory)
        {
            DialoguePinFactory = new ClinicalDialoguePinFactory(panelFactory);
            QuizPinFactory = new ClinicalQuizPinFactory(panelFactory);
        }

        protected NodeInfo LegacyDialogueNode { get; } = new NodeInfo("DialoguePin", TagComparison.NameEquals, new NodeInfo("dialogue"));
        protected override DialoguePin GetDialogue(XmlDeserializer deserializer)
        {
            var dialogue = base.GetDialogue(deserializer);
            if (dialogue == null)
                dialogue = deserializer.GetValue(LegacyDialogueNode, DialoguePinFactory);

            return dialogue;
        }
        protected NodeInfo LegacyQuizNode { get; } = new NodeInfo("QuizPin");
        protected override QuizPin GetQuiz(XmlDeserializer deserializer)
        {
            var quiz = base.GetQuiz(deserializer);
            if (quiz == null)
                quiz = deserializer.GetValue(LegacyQuizNode, QuizPinFactory);

            return quiz;
        }
    }
}
