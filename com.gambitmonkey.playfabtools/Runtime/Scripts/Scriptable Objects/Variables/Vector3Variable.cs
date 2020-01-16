using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Vector3Variable", menuName = "GambitMonkey/Create/Variables/Vector3")]
    public class Vector3Variable : ScriptableObject, ISerializationCallbackReceiver
    {
        //Float Value
        [NonSerialized]
        public Vector3 Value;

        //Can the Value be reset in game
        //public bool resettable;

        //When the game starts, the starting Value we use (so we can reset if need be)
        [SerializeField]
        private Vector3 startingValue = Vector3.zero;

        /// <summary>
        /// Set sVector3 Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(Vector3 _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set Value to another sVector3 Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(Vector3Variable _value)
        {
            Value = _value.Value;
        }

        /// <summary>
        /// Add a Vector3 Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(Vector3 _value)
        {
            Value += _value;
        }

        /// <summary>
        /// Add another sVector3 Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(Vector3Variable _value)
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
