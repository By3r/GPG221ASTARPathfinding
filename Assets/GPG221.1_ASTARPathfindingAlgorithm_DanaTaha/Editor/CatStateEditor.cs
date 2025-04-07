#if UNITY_EDITOR
using Gameplay;
using GOAP.WorldStates;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(CatState))]
    public class CatStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            #region Draw the default inspector for any public fields
            DrawDefaultInspector();
            #endregion

            #region  Get a reference to the CatState script
            CatState catState = (CatState)target;
            #endregion

            EditorGUILayout.LabelField("World State Facts", EditorStyles.boldLabel);

            #region Check if there are any facts to display
            if (catState.facts != null && catState.facts.Count > 0)
            {
                foreach (var fact in catState.facts)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(fact.Key, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField(fact.Value.ToString());
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No cat facts available.");
            }
            #endregion

            #region Force the inspector to repaint during play mode
            if (Application.isPlaying)
            {
                Repaint();
            }
            #endregion
        }
    }
}
#endif