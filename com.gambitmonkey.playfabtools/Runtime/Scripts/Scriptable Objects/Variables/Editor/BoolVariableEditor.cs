#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(BoolVariable))]
    [CanEditMultipleObjects]
    internal class BoolVariableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            BoolVariable script = (BoolVariable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("Current value: " + script.Value, EditorStyles.boldLabel);

            //Display button that toggles the bool value
            if (GUILayout.Button("Toggle Value"))
            {
                if (EditorApplication.isPlaying)
                {
                    script.Toggle();
                }
            }

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

