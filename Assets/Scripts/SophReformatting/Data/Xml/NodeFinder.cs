using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace SimEncounters.Xml
{
    public enum TagComparison
    {
        NameEquals,
        NameEqualsIgnoreCase,
        NameStartsWith,
        NameEndsWith,
        NameNotEqualTo,
        ChildNameEquals,
        AttributeNameEquals,
        /// <summary>
        /// Gets the root tag and uses its name as the value.
        /// </summary>
        RootName,
        /// <summary>
        /// Gets the root tag and uses its inner text as the value.
        /// </summary>
        RootValue
    }

    public class NodeInfo
    {
        public static NodeInfo RootName { get; } = new NodeInfo(null, TagComparison.RootName);
        public static NodeInfo RootValue { get; } = new NodeInfo(null, TagComparison.RootValue);

        protected virtual TagComparison XmlFinder { get; } = TagComparison.NameEquals;
        public virtual string Name { get; }
        protected virtual NodeInfo SubNodeFinder { get; }

        public NodeInfo(string name, TagComparison xmlFinder = TagComparison.NameEquals, NodeInfo subNodeFinder = null)
        {
            Name = name;
            XmlFinder = xmlFinder;
            SubNodeFinder = subNodeFinder;
        }

        public virtual XmlNode FindNode(XmlNode node)
        {
            if (XmlFinder == TagComparison.NameEquals) { 
                node = node[Name];
            } else if (XmlFinder == TagComparison.ChildNameEquals) {
                foreach (XmlNode childNode in node) {
                    var selectedNode = childNode[Name];
                    if (selectedNode != null) {
                        node = selectedNode;
                        break;
                    }
                }
            } else if (XmlFinder == TagComparison.AttributeNameEquals) {
                node = node.Attributes[Name];
            } else if (XmlFinder != TagComparison.RootValue && XmlFinder != TagComparison.RootName) {
                var childNodes = (IEnumerable<XmlNode>)node.ChildNodes;
                return childNodes.FirstOrDefault(GetComparisonPredicate());
            }

            if (SubNodeFinder != null && node != null)
                return SubNodeFinder.FindNode(node);
            else
                return node;
        }


        public virtual IEnumerable<XmlNode> GetNodeList(XmlNode node)
        {
            if (SubNodeFinder != null)
                Debug.LogError("Cannot use a sub node finder when getting a node list.");

            switch (XmlFinder) {
                case TagComparison.NameEquals:
                case TagComparison.NameEqualsIgnoreCase:
                case TagComparison.NameStartsWith:
                case TagComparison.NameEndsWith:
                case TagComparison.NameNotEqualTo:
                    var childNodes = (IEnumerable<XmlNode>)node.ChildNodes;
                    return childNodes.Where(GetComparisonPredicate());
                case TagComparison.ChildNameEquals:
                    var nodes = new List<XmlNode>();
                    foreach (XmlNode childNode in node) {
                        var selectedNode = childNode[Name];
                        if (selectedNode != null)
                            nodes.Add(selectedNode);
                    }
                    return nodes;

                // The root element is an individual node and can't represent a list
                case TagComparison.RootName:
                case TagComparison.RootValue:
                // XML attributes have unique names and can't represent a list
                case TagComparison.AttributeNameEquals:
                default:
                    return null;
            }
        }

        protected virtual Func<XmlNode, bool> GetComparisonPredicate()
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
                case TagComparison.NameNotEqualTo:
                    return (node => node.Name != Name);
                case TagComparison.ChildNameEquals:
                    return (node => node[Name] != null);
                // The rest don't check children like this
                default:
                    return (node => false);
            }
        }

        /// <summary>
        /// Gets the text value stored in the passed text node.
        /// </summary>
        /// <param name="textNode">Node containing the value to read</param>
        /// <returns>The string value in the node</returns>
        public virtual string GetText(XmlNode textNode)
        {
            string text;
            switch (XmlFinder) {
                // Root is used to get the name of the root node
                case TagComparison.RootName:
                    text = textNode.Name;
                    break;
                // Value is used for the attribute value
                case TagComparison.AttributeNameEquals:
                    text = textNode.Value;
                    break;
                // InnerText is used for all other search methods
                default:
                    text = textNode.InnerText;
                    break;
            }

            if (text == null)
                return null;

            return UnityWebRequest.UnEscapeURL(text);
        }

        /// <summary>
        /// Finds the first matching node and gets its text.
        /// </summary>
        /// <param name="node">Root node to search under</param>
        /// <returns>The string value in the found node</returns>
        public virtual string FindNodeText(XmlNode node)
        {
            var textNode = FindNode(node);
            return GetText(textNode);
        }
    }

}