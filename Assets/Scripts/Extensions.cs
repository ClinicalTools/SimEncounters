using System;
using System.Xml;

public static class Extensions
{

    /**
     * Advance the node to the correct next node in the XmlDoc. Think Depth first search
     */
    // you can't do this like literally please don't
    // It makes it impossible to know how the xml code is supposed to be structred
    public static XmlNode AdvNode(this XmlDocument xmlDoc, XmlNode node)
    {
        if (node == null)
            return node;
        if (node.HasChildNodes)
            node = node.ChildNodes.Item(0);
        else if (node.NextSibling != null)
            node = node.NextSibling;
        else {
            while (node.ParentNode.NextSibling == null) {
                node = node.ParentNode;
                if (node == xmlDoc.DocumentElement || node.ParentNode == null)
                    return null;
            }
            node = node.ParentNode.NextSibling;
            if (node == xmlDoc.DocumentElement.LastChild)
                return node;
        }
        return node;
    }

    // !!!!
    public static string GetNodeVal(this XmlDocument xmlDoc, ref XmlNode node, string nodeName)
    {
        var newNode = node;

        while (newNode != null && !newNode.Name.Equals(nodeName, StringComparison.InvariantCultureIgnoreCase))
            newNode = xmlDoc.AdvNode(newNode);

        if (newNode == null) {
            return "";
        }

        node = newNode;
        return node.InnerText.Replace(GlobalData.EMPTY_WIDTH_SPACE.ToString(), "");
    }

    // Recursive function to read all nodes until uid is found
    public static string FindUID(this XmlNode node)
    {
        if (node.Name == "uid") {
            return node.InnerText;
        }

        foreach (XmlNode n in node.ChildNodes) {
            var uid = n.FindUID();
            if (!string.IsNullOrEmpty(uid))
                return uid;
        }

        return "";
    }
}
