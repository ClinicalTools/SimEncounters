using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class QuizPinFactory : ISerializationFactory<QuizPin>
    {
        // pins are created by panels, so that factory can be reused
        // failure to do so can result in an infinite loop
        protected virtual PanelFactory PanelFactory { get; }

        protected virtual CollectionInfo QuestionsInfo { get; } = new CollectionInfo("questions", "panel");


        public QuizPinFactory(PanelFactory panelFactory)
        {
            PanelFactory = panelFactory;
        }

        public virtual bool ShouldSerialize(QuizPin value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, QuizPin value)
        {
            serializer.AddKeyValuePairs(QuestionsInfo, value.Questions, PanelFactory);
        }

        public virtual QuizPin Deserialize(XmlDeserializer deserializer)
        {
            var quizPin = CreateQuizPin(deserializer);

            AddQuestions(deserializer, quizPin);

            return quizPin;
        }

        protected virtual QuizPin CreateQuizPin(XmlDeserializer deserializer) => new QuizPin();

        protected virtual List<KeyValuePair<string, Panel>> GetQuestions(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(QuestionsInfo, PanelFactory);
        protected virtual void AddQuestions(XmlDeserializer deserializer, QuizPin quizPin)
        {
            var questionPairs = GetQuestions(deserializer);
            if (questionPairs == null)
                return;

            foreach (var panelPair in questionPairs)
                quizPin.Questions.Add(panelPair);
        }
    }
}