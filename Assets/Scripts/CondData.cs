using System.Collections.Generic;
using System.Xml;

public static class CondData
{
    public static HashSet<string> Keys { get; } = new HashSet<string>();

    private static readonly Dictionary<string, BoolConditional> boolCondDict = new Dictionary<string, BoolConditional>(); //A dictionary of all conditionals for the bool variables
    public static void AddBoolCond(BoolConditional boolCond)
    {
        boolCondDict.Add(boolCond.Serial, boolCond);
    }
    public static BoolConditional GetBoolCond(string key)
    {
        if (boolCondDict.ContainsKey(key))
            return boolCondDict[key];
        else
            return null;
    }
    public static void RemoveBoolCond(string key)
    {
        boolCondDict.Remove(key);
    }


    private static readonly Dictionary<string, IntConditional> intCondDict = new Dictionary<string, IntConditional>(); //A dictionary of all conditionals for the int variables
    public static void AddIntCond(IntConditional intCond)
    {
        intCondDict.Add(intCond.Serial, intCond);
    }
    public static IntConditional GetIntCond(string key)
    {
        if (intCondDict.ContainsKey(key))
            return intCondDict[key];
        else
            return null;
    }
    public static void RemoveIntCond(string key)
    {
        intCondDict.Remove(key);
    }


    public static void Reset()
    {
        boolCondDict.Clear();
        intCondDict.Clear();
        Keys.Clear();
    }


    public static XmlNode ReadNode(XmlDocument xmlDoc, XmlNode node)
    {
        if (node.Name.ToLower().StartsWith("cond")) {
            var key = xmlDoc.GetNodeVal(ref node, "key");

            var type = VarType.GetType(key);

            if (type == VarType.Bool)
                node = ReadBoolCondNode(xmlDoc, node, key);
            if (type == VarType.Int)
                node = ReadIntCondNode(xmlDoc, node, key);
        }

        return node;
    }

    private static XmlNode ReadBoolCondNode(XmlDocument xmlDoc, XmlNode node, string serial)
    {
        var varKey = xmlDoc.GetNodeVal(ref node, "varKey");
        var valStr = xmlDoc.GetNodeVal(ref node, "value");
        if (bool.TryParse(valStr, out var val))
            boolCondDict.Add(serial, new BoolConditional(serial, varKey, val));

        return xmlDoc.AdvNode(node);
    }

    private static XmlNode ReadIntCondNode(XmlDocument xmlDoc, XmlNode node, string serial)
    {
        var varKey = xmlDoc.GetNodeVal(ref node, "varKey");
        var valStr = xmlDoc.GetNodeVal(ref node, "value");
        var opStr = xmlDoc.GetNodeVal(ref node, "op");
        if (int.TryParse(valStr, out var val) && IntConditional.TryParseOperator(opStr, out var op))
            intCondDict.Add(serial, new IntConditional(serial, varKey, val, op));

        return xmlDoc.AdvNode(node);
    }

    public static string GetXML()
    {
        var data = "";

        int i = 0;
        foreach (var boolVar in boolCondDict.Values) {
            data += "<cond" + i + ">";
            data += "<key>" + boolVar.Serial + "</key>";
            data += "<varKey>" + boolVar.VarSerial + "</varKey>";
            data += "<value>" + boolVar.Value + "</value>";
            data += "</cond" + i + ">";
            i++;
        }

        foreach (var intVar in intCondDict.Values) {
            data += "<cond" + i + ">";
            data += "<key>" + intVar.Serial + "</key>";
            data += "<varKey>" + intVar.VarSerial + "</varKey>";
            data += "<value>" + intVar.Value + "</value>";
            data += "<op>" + intVar.Comparator + "</op>";
            data += "</cond" + i + ">";
            i++;
        }

        return data;
    }
}
