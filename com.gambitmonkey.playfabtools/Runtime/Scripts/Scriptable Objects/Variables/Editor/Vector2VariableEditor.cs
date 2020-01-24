#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(Vector2Variable))]
    [CanEditMultipleObjects]
    internal class SOVector2Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            Vector2Variable script = (Vector2Variable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("Current value: " + script.Value, EditorStyles.boldLabel);

            //Display button that resets the value to the starting value
            if (GUILayout.Button("Reset Value"))
            {
                if (EditorApplication.isPlaying)
                {
                    script.ResetValue();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
#endif

