using System.Xml;

namespace SimEncounters
{
    public class EncounterData
    {
        protected const string VARS_NODE_NAME = "vars";
        protected const string CONDS_NODE_NAME = "conditionals";

        public virtual OldSectionCollection OldSections { get; }
        public virtual SectionCollection Sections { get; }
        public virtual ImgCollection Images { get; }

        public virtual BoolCollection EncounterBools { get; }
        public virtual IntCollection EncounterInts { get; }
        public virtual BoolConditionalCollection BoolConditions { get; }
        public virtual IntConditionalCollection IntConditions { get; }

        public EncounterData()
        {
            OldSections = new OldSectionCollection();
            Sections = new SectionCollection();

            Images = new ImgCollection();

            EncounterBools = new BoolCollection();
            EncounterInts = new IntCollection();

            BoolConditions = new BoolConditionalCollection();
            IntConditions = new IntConditionalCollection();

            OldSections = new OldSectionCollection();
        }
        public EncounterData(XmlNode cedNode, XmlNode ceiNode)
        {
            OldSections = new OldSectionCollection(cedNode);
            Sections = new SectionCollection(cedNode);

            Images = new ImgCollection(ceiNode);

            var caseVarsNode = cedNode[VARS_NODE_NAME];
            EncounterBools = new BoolCollection(caseVarsNode);
            EncounterInts = new IntCollection(caseVarsNode);

            var conditionalsNode = cedNode[CONDS_NODE_NAME];
            BoolConditions = new BoolConditionalCollection(conditionalsNode);
            IntConditions = new IntConditionalCollection(conditionalsNode);

            OldSections = new OldSectionCollection(cedNode);
        }


        public virtual XmlElement GetXml()
        {
            var encounterData = XmlHelper.CreateElement("encounter");
            
            if (Sections.Count > 0)
                encounterData.AppendChild(Sections.GetXml());

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

        public virtual XmlElement GetImageXml()
        {
            var encounterData = XmlHelper.CreateElement("encounter");
            if (Images.Count > 0)
                encounterData.AppendChild(Images.GetXml());

            return encounterData;
        }
    }
}