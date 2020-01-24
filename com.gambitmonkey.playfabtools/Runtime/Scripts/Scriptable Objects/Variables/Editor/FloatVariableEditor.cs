#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(FloatVariable))]
    [CanEditMultipleObjects]
    internal class SOFloatEditor : Editor
    {

        private float floatModifyValue = 0.0f;

        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            FloatVariable script = (FloatVariable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("Current value: " + script.Value, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            //Display a float input field and button to add the inputted value to the current value
            floatModifyValue = EditorGUILayout.FloatField("Modify current value by: ", floatModifyValue);

            if (GUILayout.Button("Modify"))
            {
                script.ApplyChange(floatModifyValue);
            }

            EditorGUILayout.EndHorizontal();

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

