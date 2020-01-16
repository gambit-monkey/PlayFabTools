using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.PlayFabTools
{
    public class MockGameServer : MonoBehaviour
    {
        public UnityEvent OnReadyForPlayers;

        // Start is called before the first frame update
        void Start()
        {
            OnReadyForPlayers.Invoke();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
