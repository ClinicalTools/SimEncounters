﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;

public static class VarData
{
    public static HashSet<string> Keys { get; } = new HashSet<string>();

    private static readonly Dictionary<string, EncounterBool> boolDict = new Dictionary<string, EncounterBool>(); //A dictionary of all bool variables for a case
    public static List<EncounterBool> BoolVars => boolDict.Values.ToList();
    public static void AddCaseBool(EncounterBool boolVar)
    {
        // TODO: Fix
        //boolDict.Add(boolVar.Serial, boolVar);
    }
    public static EncounterBool GetCaseBool(string key)
    {
        if (boolDict.ContainsKey(key))
            return boolDict[key];
        else
            return null;
    }
    public static void RemoveCaseBool(string key)
    {
        boolDict.Remove(key);
    }


    private static readonly Dictionary<string, EncounterInt> intDict = new Dictionary<string, EncounterInt>(); //A dictionary of all int variables for a case
    public static List<EncounterInt> IntVars => intDict.Values.ToList();
    public static void AddCaseInt(EncounterInt intVar)
    {
        // TODO: Fix
        //intDict.Add(intVar.Serial, intVar);
    }
    public static EncounterInt GetCaseInt(string key)
    {
        if (intDict.ContainsKey(key))
            return intDict[key];
        else
            return null;
    }
    public static void RemoveCaseInt(string key)
    {
        intDict.Remove(key);
    }



    public static void Reset()
    {
        boolDict.Clear();
        intDict.Clear();
        Keys.Clear();
    }

    public static XmlNode ReadNode(XmlDocument xmlDoc, XmlNode node)
    {
        if (node.Name.ToLower().StartsWith("var")) {
            var key = xmlDoc.GetNodeVal(ref node, "key");

            var type = VarType.GetType(key);

            if (type == VarType.Bool)
                node = ReadBoolNode(xmlDoc, node, key);
            else if (type == VarType.Int)
                node = ReadIntNode(xmlDoc, node, key);
        }

        return node;
    }

    private static XmlNode ReadBoolNode(XmlDocument xmlDoc, XmlNode node, string serial)
    {
        var name = xmlDoc.GetNodeVal(ref node, "name");
        var valStr = xmlDoc.GetNodeVal(ref node, "value");
        if (bool.TryParse(valStr, out var val))
            boolDict.Add(serial, new EncounterBool(name, val));

        return xmlDoc.AdvNode(node);
    }

    private static XmlNode ReadIntNode(XmlDocument xmlDoc, XmlNode node, string serial)
    {
        var name = xmlDoc.GetNodeVal(ref node, "name");
        var valStr = xmlDoc.GetNodeVal(ref node, "value");
        if (int.TryParse(valStr, out var val))
            intDict.Add(serial, new EncounterInt(name, val));

        return xmlDoc.AdvNode(node);
    }

    public static string GetXML()
    {
        var data = "";

        int i = 0;
        foreach (var boolVar in boolDict.Values) {
            data += "<var" + i + ">";
            //data += "<key>" + boolVar.Serial + "</key>";
            data += "<name>" + boolVar.Name + "</name>";
            data += "<value>" + boolVar.Value + "</value>";
            data += "</var" + i + ">";
            i++;
        }

        foreach (var intVar in intDict.Values) {
            data += "<var" + i + ">";
            //data += "<key>" + intVar.Serial + "</key>";
            data += "<name>" + intVar.Name + "</name>";
            data += "<value>" + intVar.Value + "</value>";
            data += "</var" + i + ">";
            i++;
        }

        return data;
    }
}
