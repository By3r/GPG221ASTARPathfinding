#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GOAP.WorldStates;

[CustomEditor(typeof(WorldState))]
public class WorldStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector for any public fields
        DrawDefaultInspector();

        // Get a reference to the WorldState instance
        WorldState worldState = (WorldState)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World State Facts", EditorStyles.boldLabel);

        // Check if there are any facts to display
        if (worldState.facts != null && worldState.facts.Count > 0)
        {
            foreach (var fact in worldState.facts)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(fact.Key, GUILayout.MaxWidth(150));
                EditorGUILayout.LabelField(fact.Value.ToString());
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No facts available.");
        }

        // Force the inspector to repaint during play mode
        if (Application.isPlaying)
        {
            Repaint();
        }
    }
}
#endif
