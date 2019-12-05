namespace ClinicalTools.SimEncounters.XmlSerialization
{
    public class CollectionInfo
    {
        public NodeInfo CollectionNode { get; }
        public NodeInfo ElementNode { get; }

        public CollectionInfo(string collectionName, string elementName)
        {
            CollectionNode = new NodeInfo(collectionName);
            ElementNode = new NodeInfo(elementName);
        }
        public CollectionInfo(NodeInfo collectionNode, NodeInfo elementNode)
        {
            CollectionNode = collectionNode;
            ElementNode = elementNode;
        }
    }
}