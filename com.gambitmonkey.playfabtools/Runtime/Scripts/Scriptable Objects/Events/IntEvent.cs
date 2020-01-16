using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IntGameEvent", menuName = "GambitMonkey/Create/Events/IntGameEvent")]
    [Serializable]
    public class IntEvent : BaseGameEvent<int>
    {

    }
}