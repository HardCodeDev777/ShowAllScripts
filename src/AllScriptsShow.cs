using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace HardCodeDev.AllScriptsShow
{
    public class AllScriptsShow : EditorWindow
    {
        private string _currentAssetPath;
        private MonoScript _currentScript;
        private bool _canRender;
        private IEnumerable<string> _guids;

        [MenuItem("HardCodeDev/AllScriptsShow")]
        public static void ShowWindow() => GetWindow<AllScriptsShow>("AllScriptsShow");

        private void OnGUI()
        {
            if (GUILayout.Button("Get all scripts"))
            {
                // UnityCsReference - https://github.com/Unity-Technologies/UnityCsReference
                // **********************
                //
                //internal static IEnumerable<string> GetAllScriptGUIDs()
                //{
                //    return AssetDatabase.GetAllAssetPaths()
                //    .Where(asset => (IsScriptOrAssembly(asset) && !UnityEditor.PackageManager.Folders.IsPackagedAssetPath(asset)))
                //    .Select(asset => AssetDatabase.AssetPathToGUID(asset));
                //}
                //
                // **********************

                var windowLayout = typeof(Editor).Assembly.GetType("UnityEditorInternal.InternalEditorUtility");
                var getAllGuids = windowLayout.GetMethod("GetAllScriptGUIDs", BindingFlags.NonPublic | BindingFlags.Static);

                _guids = getAllGuids.Invoke(null, new object[] {}) as IEnumerable<string>;
                _canRender = true;
            }
            if (_canRender)
            {
                var scriptsCount = 0;
                foreach (var guid in _guids)
                {
                    _currentAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                    _currentScript = AssetDatabase.LoadAssetAtPath<MonoScript>(_currentAssetPath);
                    _currentScript = (MonoScript)EditorGUILayout.ObjectField("Script", _currentScript, typeof(MonoScript), false);
                    scriptsCount++;
                }
                var renderCount = $"{scriptsCount}";
                GUI.enabled = false;
                renderCount = EditorGUILayout.TextField("Total scripts count: ", renderCount);
                GUI.enabled = true;
            }
        }
    }
}
#endif