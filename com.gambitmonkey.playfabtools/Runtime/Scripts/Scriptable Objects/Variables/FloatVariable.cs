using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "GambitMonkey/Create/Variables/Float")]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        //Float Value
        [NonSerialized]
        public float Value;

        //When the game starts, the starting Value we use (so we can reset if need be)
        [SerializeField]
        private float startingValue = 0;

        /// <summary>
        /// Set sFloat Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(float _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set Value to another sBool Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(FloatVariable _value)
        {
            Value = _value.Value;
        }

        /// <summary>
        /// Add a float Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(float _value)
        {
            Value += _value;
        }

        /// <summary>
        /// Add another sFloat Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(FloatVariable _value)
        {
            Value += _value.Value;
        }

        /// <summary>
        /// Recieve callback after unity deseriallzes the object
        /// </summary>
        public void OnAfterDeserialize()
        {
            Value = startingValue;

        }

        public void OnBeforeSerialize() { }

        /// <summary>
        /// Reset the Value to it's inital Value if it's resettable
        /// </summary>
        public void ResetValue()
        {
            Value = startingValue;
        }
    }
}