
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class QuizPinFactory : ISerializationFactory<QuizPin>
    {
        // pins are created by panels, so a lazy injection needs to be used to prevent an infinite loop
        protected virtual LazyInject<ISerializationFactory<Panel>> PanelFactory { get; }
        public QuizPinFactory(LazyInject<ISerializationFactory<Panel>> panelFactory)
        {
            PanelFactory = panelFactory;
        }

        protected virtual CollectionInfo QuestionsInfo { get; } = new CollectionInfo("questions", "panel");

        public virtual bool ShouldSerialize(QuizPin value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, QuizPin value)
        {
            serializer.AddKeyValuePairs(QuestionsInfo, value.Questions, PanelFactory.Value);
        }

        public virtual QuizPin Deserialize(XmlDeserializer deserializer)
        {
            var quizPin = CreateQuizPin(deserializer);

            AddQuestions(deserializer, quizPin);

            return quizPin;
        }

        protected virtual QuizPin CreateQuizPin(XmlDeserializer deserializer) => new QuizPin();

        protected virtual List<KeyValuePair<string, Panel>> GetQuestions(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(QuestionsInfo, PanelFactory.Value);
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