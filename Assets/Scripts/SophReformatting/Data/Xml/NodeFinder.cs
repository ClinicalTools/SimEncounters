using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine.Networking;

namespace SimEncounters.Xml
{
    public enum TagComparison
    {
        NameEquals,
        NameEqualsIgnoreCase,
        NameStartsWith,
        NameEndsWith,
        ChildNameEquals,
        Root,
        AttributeNameEquals
    }

    public class NodeFinder
    {
        public TagComparison XmlFinder { get; } = TagComparison.NameEquals;
        public string Name { get; }

        public NodeFinder()
        {
            XmlFinder = TagComparison.Root;
        }

        public NodeFinder(string name, TagComparison xmlFinder = TagComparison.NameEquals)
        {
            Name = name;
            XmlFinder = xmlFinder;
        }

        public virtual XmlNode GetNode(XmlNode node)
        {
            switch (XmlFinder) {
                case TagComparison.NameEquals:
                    return node[Name];
                case TagComparison.ChildNameEquals:
                    foreach (XmlNode childNode in node) {
                        var selectedNode = childNode[Name];
                        if (selectedNode != null)
                            return selectedNode;
                    }
                    return null;
                case TagComparison.Root:
                    return node;
                case TagComparison.AttributeNameEquals:
                    return node.Attributes[Name];
                default:
                    var childNodes = (IEnumerable<XmlNode>)node.ChildNodes;
                    return childNodes.FirstOrDefault(ComparisonPredicate());
            }
        }


        public virtual IEnumerable<XmlNode> GetNodeList(XmlNode node)
        {
            switch (XmlFinder) {
                case TagComparison.NameEquals:
                case TagComparison.NameEqualsIgnoreCase:
                case TagComparison.NameStartsWith:
                case TagComparison.NameEndsWith:
                    var childNodes = (IEnumerable<XmlNode>)node.ChildNodes;
                    return childNodes.Where(ComparisonPredicate());
                case TagComparison.ChildNameEquals:
                    var nodes = new List<XmlNode>();
                    foreach (XmlNode childNode in node) {
                        var selectedNode = childNode[Name];
                        if (selectedNode != null)
                            nodes.Add(selectedNode);
                    }
                    return nodes;

                // The root element is an individual node and can't represent a list
                case TagComparison.Root:
                // XML attributes have unique names and can't represent a list
                case TagComparison.AttributeNameEquals:
                default:
                    return null;
            }
        }

        protected virtual Func<XmlNode, bool> ComparisonPredicate()
        {
            switch (XmlFinder) {
                case TagComparison.NameEquals:
                    return (node => node.Name == Name);
                case TagComparison.NameEqualsIgnoreCase:
                    return (node => node.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
                case TagComparison.NameStartsWith:
                    return (node => node.Name.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase));
                case TagComparison.NameEndsWith:
                    return (node => node.Name.EndsWith(Name, StringComparison.InvariantCultureIgnoreCase));
                case TagComparison.ChildNameEquals:
                    return (node => node[Name] != null);
                default:
                    return (node => false);
            }
        }

        public virtual string GetText(XmlNode xmlNode)
        {
            string text;
            switch (XmlFinder) {
                // Root is used to get the name of the root node
                case TagComparison.Root:
                    text = xmlNode.Name;
                    break;
                // Value is used for the attribute value
                case TagComparison.AttributeNameEquals:
                    text = xmlNode.Value;
                    break;
                // InnerText is used for all other search methods
                default:
                    text = xmlNode.InnerText;
                    break;
            }

            if (text == null)
                return null;

            return UnityWebRequest.UnEscapeURL(text);
        }
    }

}