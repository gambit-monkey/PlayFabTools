using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StringGameEvent", menuName = "GambitMonkey/Create/Events/StringGameEvent")]
    [Serializable]
    public class StringEvent : BaseGameEvent<string>
    {
        
    }
}
