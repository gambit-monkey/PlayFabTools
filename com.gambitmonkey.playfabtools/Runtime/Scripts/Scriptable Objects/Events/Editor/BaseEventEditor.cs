using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(FloatEvent))]
    [CanEditMultipleObjects]
    public class ScriptableFloatGameEventEditor : Editor
    {
#if UNITY_EDITOR
        [SerializeField]
        public float hSliderValue = 0.0F;
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            FloatEvent script = (FloatEvent)target;

            hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0.0F, 10.0F);

            //Draw button
            if (GUILayout.Button("Raise Event"))
            {
                //If the application is playing - raise/trigger the event
                if (EditorApplication.isPlaying)
                {
                    script.Raise(hSliderValue);
                }
            }
        }
#endif
    }
}
