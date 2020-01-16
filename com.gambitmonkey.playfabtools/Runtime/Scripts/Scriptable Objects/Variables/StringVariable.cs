using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GambitMonkey.ScriptableObjects
{

    [CreateAssetMenu(fileName = "StringVariable", menuName = "GambitMonkey/Create/Variables/String")]
    public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        //Float value
        [NonSerialized]
        public string Value;

        //When the game starts, the starting value we use (so we can reset if need be)
        [SerializeField]
        private string startingValue = null;

        /// <summary>
        /// Set sString value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(string _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set value to another sString value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(StringVariable _value)
        {
            Value = _value.Value;
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