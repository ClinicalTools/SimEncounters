using System.Collections.Generic;
using UnityEditor;

namespace ClinicalTools.EditorElements
{
    public class DefineSymbolsManager
    {
        protected BuildTargetGroup TargetGroup { get; }

        protected HashSet<string> DefineSymbols { get; }
        public DefineSymbolsManager(BuildTargetGroup targetGroup)
        {
            DefineSymbols = new HashSet<string>();
            TargetGroup = targetGroup;
            AddInitialDefineSymbols(targetGroup);
        }

        protected virtual void AddInitialDefineSymbols(BuildTargetGroup targetGroup)
        {
            var defineSymbolsStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            var defineSymbolsArr = defineSymbolsStr.Split(';');
            foreach (var defineSymbol in defineSymbolsArr)
                AddDefineSymbol(defineSymbol);
        }

        public virtual bool HasDefineSymbol(string defineSymbol) => DefineSymbols.Contains(defineSymbol);
        public virtual void SetDefineSymbol(bool isOn, string defineSymbol)
        {
            if (isOn)
                AddDefineSymbol(defineSymbol);
            else
                RemoveDefineSymbol(defineSymbol);
        }
        protected virtual void AddDefineSymbol(string defineSymbol)
        {
            if (!DefineSymbols.Contains(defineSymbol))
                DefineSymbols.Add(defineSymbol);
        }
        protected virtual void RemoveDefineSymbol(string defineSymbol)
        {
            if (DefineSymbols.Contains(defineSymbol))
                DefineSymbols.Remove(defineSymbol);
        }

        public virtual void Apply()
        {
            var defineSymbolsStr = "";
            foreach (var defineSymbol in DefineSymbols)
                defineSymbolsStr += defineSymbol + ';';

            PlayerSettings.SetScriptingDefineSymbolsForGroup(TargetGroup, defineSymbolsStr);
        }
    }
}