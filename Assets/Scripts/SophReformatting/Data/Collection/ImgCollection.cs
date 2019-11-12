﻿using System;
using System.Xml;
using UnityEngine;


namespace SimEncounters
{
    public class ImgCollection : SimCollection<SpriteHolderScript>
    {
        protected const string IMG_DATA_NODE_NAME = "imgData";

        protected const string REF_NODE_NAME = "reference";
        protected const string COLOR_NODE_NAME = "iconColor";

        protected const string WIDTH_NODE_NAME = "width";
        protected const string HEIGHT_NODE_NAME = "height";
        protected const string DATA_NODE_NAME = "data";

        public override string CollectionNodeName => "images";
        protected override string ValueNodeName => "image";

        public ImgCollection() : base() { }
        public ImgCollection(XmlNode encounterNode) : base(encounterNode) { }

        // Legacy versions stored images directly in the root node.
        protected override XmlNode GetLegacyCollectionNode(XmlNode encounterNode)
        {
            return encounterNode;
        }

        // Old image nodes had keys in a child named "key"
        protected override string GetLegacyKey(XmlNode valueNode)
        {
            string key = valueNode["key"]?.InnerText;
            if (key == null)
                return null;

            key = key.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "");
            return key;
        }

        // Old image nodes were named "image<#>"
        protected override bool ValidLegacyNodeName(string valueNodeName)
        {
            return valueNodeName.StartsWith(ValueNodeName, StringComparison.OrdinalIgnoreCase);
        }


        // Gets the sprite holder from an image node
        protected override SpriteHolderScript ReadValueNode(XmlNode imgNode)
        {
            // Legacy image data was stored in an internal "imgData" node
            var imgData = imgNode[IMG_DATA_NODE_NAME];
            if (imgData != null)
                imgNode = imgData;


            SpriteHolderScript spriteHolder;

            // Some images are references, while some hold data directly
            var reference = imgNode[REF_NODE_NAME]?.InnerText;
            if (reference != null) {
                spriteHolder = new SpriteHolderScript(reference);
            } else {
                spriteHolder = GetSpriteHolderByData(imgNode);
                if (spriteHolder == null)
                    return null;
            }


            string colorValue = imgNode[COLOR_NODE_NAME]?.InnerText;
            SetColor(spriteHolder, colorValue);

            return spriteHolder;
        }


        // Creates a sprite holder from image data asssumed not to be a reference
        protected virtual SpriteHolderScript GetSpriteHolderByData(XmlNode imgNode)
        {
            var dataStr = imgNode[DATA_NODE_NAME]?.InnerText;
            var widthStr = imgNode[WIDTH_NODE_NAME]?.InnerText;
            var heightStr = imgNode[HEIGHT_NODE_NAME]?.InnerText;

            if (dataStr == null 
                || !int.TryParse(widthStr, out int width) 
                || !int.TryParse(heightStr, out int height)) { 

                return null;
            }

            Texture2D temp = new Texture2D(2, 2);
            temp.LoadImage(Convert.FromBase64String(dataStr));
            Sprite newSprite = Sprite.Create(temp, new Rect(0, 0, width, height), new Vector2(0, 0), 100);
            return new SpriteHolderScript(newSprite);
        }

        // Sets a sprite holder's color from a string with four int values
        protected virtual void SetColor(SpriteHolderScript spriteHolder, string colorValue)
        {
            if (string.IsNullOrWhiteSpace(colorValue))
                return;

            string[] vars = colorValue.Split(',');
            if (vars.Length < 4)
                return;

            if (float.TryParse(vars[0], out var r) && float.TryParse(vars[0], out var g)
                && float.TryParse(vars[0], out var b) && float.TryParse(vars[0], out var a)) {

                spriteHolder.useColor = true;
                spriteHolder.color.r = r;
                spriteHolder.color.g = g;
                spriteHolder.color.b = b;
                spriteHolder.color.a = a;
            }
        }


        protected override void WriteValueElem(XmlElement valueElement, SpriteHolderScript value)
        {
            // TODO: this
            throw new NotImplementedException();
        }
    }
}