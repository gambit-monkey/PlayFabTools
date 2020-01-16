using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ListGameEvent", menuName = "GambitMonkey/Create/Events/ListGameEvent")]
    [Serializable]
    public class ListEvent : BaseGameEvent<List<object>>
    {

    }
}
