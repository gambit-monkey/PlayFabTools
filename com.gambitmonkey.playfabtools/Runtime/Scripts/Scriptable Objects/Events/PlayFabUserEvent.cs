using GambitMonkey.PlayFabTools;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayFabUserGameEvent", menuName = "GambitMonkey/Create/Events/PlayFabUserGameEvent")]
    [Serializable]
    public class PlayFabUserEvent : BaseGameEvent<PlayFabUser>
    {

    }
}
