using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameObjectVariable", menuName = "GambitMonkey/Create/Variables/GameObject")]
    public class GameObjectVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        //Float Value
        [NonSerialized]
        public GameObject Value;

        //When the game starts, the starting Value we use (so we can reset if need be)
        [SerializeField]
        private GameObject startingValue = null;

        /// <summary>
        /// Set sGameObject Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(GameObject _value)
        {
            Value = _value;
        }

        /// <summary>
        /// Set Value to another sGameObject Value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(GameObjectVariable _value)
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
        /// Reset the Value to it's inital Value if it's resettable
        /// </summary>
        public void ResetValue()
        {
            Value = startingValue;
        }
    }
}
