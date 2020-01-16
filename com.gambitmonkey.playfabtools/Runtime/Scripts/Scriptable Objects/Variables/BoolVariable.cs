using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "GambitMonkey/Create/Variables/Bool")]
    public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        [NonSerialized]
        public bool Value;

        //Can the value be reset in game
        //public bool resettable;

        //When the game starts, the starting value we use (so we can reset if need be)
        [SerializeField]
        private bool startingValue = false;

        /// <summary>
        /// Set sBool value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(bool _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set value to another sBool value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(BoolVariable _value)
        {
            Value = _value.Value;
        }

        /// <summary>
        /// Swap the bool value
        /// </summary>
        public void Toggle()
        {
            Value = !Value;
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
        /// Reset the value to it's inital value if it's resettable
        /// </summary>
        public void ResetValue()
        {
            Value = startingValue;
        }
    }
}
