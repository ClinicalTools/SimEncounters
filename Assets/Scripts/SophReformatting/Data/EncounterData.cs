using System.Xml;

namespace SimEncounters
{
    public class EncounterData
    {
        protected const string VARS_NODE_NAME = "vars";
        protected const string CONDS_NODE_NAME = "conditionals";

        public virtual SectionCollection Sections { get; }
        public virtual ImgCollection Images { get; }

        public virtual BoolCollection EncounterBools { get; }
        public virtual IntCollection EncounterInts { get; }
        public virtual BoolConditionalCollection BoolConditions { get; }
        public virtual IntConditionalCollection IntConditions { get; }

        public EncounterData()
        {
            Sections = new SectionCollection();
            Images = new ImgCollection();

            EncounterBools = new BoolCollection();
            EncounterInts = new IntCollection();

            BoolConditions = new BoolConditionalCollection();
            IntConditions = new IntConditionalCollection();

            Sections = new SectionCollection();
        }
        public EncounterData(XmlNode cedNode, XmlNode ceiNode)
        {
            Sections = new SectionCollection(cedNode);
            Images = new ImgCollection(ceiNode);

            var caseVarsNode = cedNode[VARS_NODE_NAME];
            EncounterBools = new BoolCollection(caseVarsNode);
            EncounterInts = new IntCollection(caseVarsNode);

            var conditionalsNode = cedNode[CONDS_NODE_NAME];
            BoolConditions = new BoolConditionalCollection(conditionalsNode);
            IntConditions = new IntConditionalCollection(conditionalsNode);

            Sections = new SectionCollection(cedNode);
        }


        public virtual XmlElement GetXml()
        {
            var encounterData = XmlHelper.CreateElement("encounter");
            
            if (Sections.Count > 0)
                encounterData.AppendChild(Sections.GetXml());
            if (Images.Count > 0)
                encounterData.AppendChild(Images.GetXml());

            if (EncounterBools.Count > 0)
                encounterData.AppendChild(EncounterBools.GetXml());
            if (EncounterInts.Count > 0)
                encounterData.AppendChild(EncounterInts.GetXml());

            if (BoolConditions.Count > 0)
                encounterData.AppendChild(BoolConditions.GetXml());
            if (IntConditions.Count > 0)
                encounterData.AppendChild(IntConditions.GetXml());

            return encounterData;
        }
    }
}