/* 
 * MIT License

Copyright (c) 2019 - 2020 Thanut Panichyotai

Permission is hereby granted, free of charge, 
to any person obtaining a copy of this software 
and associated documentation files (the "Software"), 
to deal in the Software without restriction, 
including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit 
persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice 
shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF 
ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN 
AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.
 */

// Uses code from https://github.com/LuviKunG/ScriptDefineSymbolsEditor

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ScriptingDefineSymbolsEditorWindow : EditorWindow
    {
        private static readonly GUIContent CONTENT_BUTTON_REVERT 
            = new GUIContent("Revert", "Revert all Scripting Define Symbols changes.");
        private static readonly GUIContent CONTENT_BUTTON_APPLY 
            = new GUIContent("Apply", "Apply all Scripting Define Symbols changes. " +
                "This will made unity recompile if your current build target group is active.");

        private static readonly GUIContent CONTENT_DEMO_TOGGLE = new GUIContent("Demo");
        private static readonly GUIContent CONTENT_MOBILE_TOGGLE = new GUIContent("Mobile");
        private static readonly GUIContent CONTENT_STANDALONE_TOGGLE 
            = new GUIContent("Standalone", "Is this the only scene in the build.");

        private static readonly GUIContent CONTENT_LABEL_DESCRIPTION 
            = new GUIContent("Scripting Define Symbols");

        private const string DemoSymbol = "DEMO";
        private const string MobileSymbol = "MOBILE";
        private const string StandaloneSymbol = "STANDALONE_SCENE";

        private const string WindowTitle = "Scripting Define Symbols";

        [MenuItem("Window/Sim Encounters Scripting Define Symbols", false)]
        public static ScriptingDefineSymbolsEditorWindow OpenWindow()
        {
            var window = GetWindow<ScriptingDefineSymbolsEditorWindow>(false, WindowTitle, true);
            window.Show();
            return window;
        }

        private BuildTargetGroup buildTargetGroup = BuildTargetGroup.Unknown;
        private BuildTargetGroup inspectorBuildTargetGroup = BuildTargetGroup.Unknown;
        private HashSet<string> sdsSet = new HashSet<string>();
        private bool isDirty = false;

        private void OnEnable()
        {
            UpdateActiveBuildTargetGroup();
            UpdateScriptDefineSymbolsParameters();
        }

        private void OnGUI()
        {
            using (var horizontalScope = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar)) {
                if (GUILayout.Button(CONTENT_BUTTON_REVERT, EditorStyles.toolbarButton)) {
                    UpdateScriptDefineSymbolsParameters();
                }
                using (var disableScope = new EditorGUI.DisabledGroupScope(!isDirty)) {
                    if (GUILayout.Button(CONTENT_BUTTON_APPLY, EditorStyles.toolbarButton)) {
                        ApplyChangesScriptingDefineSymbols();
                    }
                }
                GUILayout.FlexibleSpace();
                using (var changeScope = new EditorGUI.ChangeCheckScope()) {
                    inspectorBuildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup(GUIContent.none, buildTargetGroup, EditorStyles.toolbarDropDown, GUILayout.Width(100.0f));
                    if (changeScope.changed && buildTargetGroup != inspectorBuildTargetGroup) {
                        if (isDirty) {
                            if (EditorUtility.DisplayDialog("Warning", "All changes on current active build target will be revert. Do you want to apply change?", "Yes", "No")) {
                                ApplyChangesScriptingDefineSymbols();
                            }
                        }
                        buildTargetGroup = inspectorBuildTargetGroup;
                        UpdateScriptDefineSymbolsParameters();
                    }
                }
            }

            if (buildTargetGroup == BuildTargetGroup.Unknown) {
                EditorGUILayout.HelpBox("Scripting Define Symbols is disabled on this build target group.", MessageType.Info, true);
                return;
            }

            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(CONTENT_LABEL_DESCRIPTION, EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            ShowToggle(DemoSymbol, CONTENT_DEMO_TOGGLE);
            ShowToggle(MobileSymbol, CONTENT_MOBILE_TOGGLE);
            ShowToggle(StandaloneSymbol, CONTENT_STANDALONE_TOGGLE);
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        private void ShowToggle(string symbol, GUIContent content)
        {
            var hasSymbol = sdsSet.Contains(symbol);
            if (EditorGUILayout.Toggle(content, hasSymbol)) {
                if (hasSymbol)
                    return;

                sdsSet.Add(symbol);
                isDirty = true;
            } else if (hasSymbol) {
                sdsSet.Remove(symbol);
                isDirty = true;
            }
        }

        private List<string> GetListScriptDefineSymbolsParameters(string sds)
                => new List<string>(sds.Split(';'));
        private HashSet<string> GetHashSetScriptDefineSymbolsParameters(string sds)
        {
            var listSds = GetListScriptDefineSymbolsParameters(sds);
            var setSds = new HashSet<string>();
            foreach (var symbol in listSds) {
                if (!setSds.Contains(symbol))
                    setSds.Add(symbol);
            }
            return setSds;
        }

        private void UpdateActiveBuildTargetGroup()
        {
            buildTargetGroup = inspectorBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        }

        private void UpdateScriptDefineSymbolsParameters()
        {
            var sds = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (string.IsNullOrWhiteSpace(sds))
                sdsSet = new HashSet<string>();
            else
                sdsSet = GetHashSetScriptDefineSymbolsParameters(sds);

            isDirty = false;
        }

        private void ApplyChangesScriptingDefineSymbols()
        {
            if (sdsSet.Count > 0) {
                StringBuilder sb = new StringBuilder();
                var sdsArr = sdsSet.ToArray();
                sb.Append(sdsArr[0]);
                for (int i = 1; i < sdsArr.Length; i++) {
                    var symbol = sdsArr[i];
                    if (!string.IsNullOrWhiteSpace(symbol)) {
                        sb.Append(';');
                        sb.Append(symbol);
                    } else {
                        sdsSet.Remove(symbol);
                    }
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, sb.ToString());
            } else {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, "");
            }
        }
    }
}