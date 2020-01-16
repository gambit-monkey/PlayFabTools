using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GambitMonkey.ScriptableObjects
{

    [CreateAssetMenu(fileName = "IntVariable", menuName = "GambitMonkey/Create/Variables/Int")]
    public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        //Float Value
        [NonSerialized]
        public int Value;

        //When the game starts, the starting Value we use (so we can reset if need be)
        [SerializeField]
        private int startingValue = 0;

        /// <summary>
        /// Set sInt Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(int _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set Value to another sInt Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(IntVariable _value)
        {
            Value = _value.Value;
        }

        /// <summary>
        /// Add a int Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(int _value)
        {
            Value += _value;
        }

        /// <summary>
        /// Add another sInt Value to the Value
        /// </summary>
        /// <param name="_value"></param>
        public void ApplyChange(IntVariable _value)
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
