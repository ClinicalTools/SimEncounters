namespace SimEncounters.Xml
{
    public class CollectionXmlInfo
    {
        public NodeInfo CollectionNode { get; }
        public NodeInfo ElementNode { get; }

        public CollectionXmlInfo(string collectionName, string elementName)
        {
            CollectionNode = new NodeInfo(collectionName);
            ElementNode = new NodeInfo(elementName);
        }
        public CollectionXmlInfo(NodeInfo collectionNode, NodeInfo elementNode)
        {
            CollectionNode = collectionNode;
            ElementNode = elementNode;
        }
    }
}