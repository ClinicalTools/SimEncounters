using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalQuizPinFactory : QuizPinFactory
    {
        public ClinicalQuizPinFactory(PanelFactory panelFactory) : base(panelFactory) { }

        protected CollectionInfo LegacyQuestionsInfo { get; } =
            new CollectionInfo(
                new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData")),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );
        protected override void AddQuestions(XmlDeserializer deserializer, QuizPin quizPin)
        {
            base.AddQuestions(deserializer, quizPin);
            if (quizPin.Questions.Count != 0)
                return;

            var questions = deserializer.GetList(LegacyQuestionsInfo, PanelFactory);
            if (questions == null)
                return;

            foreach (var panel in questions)
                quizPin.Questions.Add(panel);
        }
    }
}