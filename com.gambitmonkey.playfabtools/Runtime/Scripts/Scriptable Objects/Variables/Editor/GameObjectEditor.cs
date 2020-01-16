﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(GameObjectVariable))]
    [CanEditMultipleObjects]
    public class SOGameObjectEditor : Editor
    {
#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            GameObjectVariable script = (GameObjectVariable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            if (script.Value != null)
            {
                EditorGUILayout.LabelField("Current value: " + script.Value.name, EditorStyles.boldLabel);
            }
            else
            {
                EditorGUILayout.LabelField("Current value: ", EditorStyles.boldLabel);
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
#endif
    }
}
