using PlayFab;
using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GambitMonkey.PlayFabTools
{
    public class PlayFabMultiplayerServer : MonoBehaviour
    {
        //PlayFab Server Sessions 
        Dictionary<string, string> servers = new Dictionary<string, string>();

        [Header("Configuration")]
        [SerializeField]
        private bool dontDestroyOnLoad = true;
        [SerializeField]
        private bool showDebugMessages = false;

        /// <summary>
        /// PlayFabClientInstance singleton
        /// </summary>
        public static PlayFabMultiplayerServer singleton;

        //Awake is called before any other method
        private void Awake()
        {
            InitializeSingleton();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        void InitializeSingleton()
        {
            if (singleton != null && singleton == this)
            {
                return;
            }

            if (dontDestroyOnLoad)
            {
                if (singleton != null)
                {
                    UnityEngine.Debug.LogWarning("[PlayFabQoS] Multiple PlayFabQoS detected in the scene. Only one PlayFabQoS can exist at a time. The duplicate PlayFabQoS will be destroyed.");
                    Destroy(gameObject);
                    return;
                }
                UnityEngine.Debug.Log("[PlayFabQoS] Created PlayFabQoS singleton (DontDestroyOnLoad)");
                singleton = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                UnityEngine.Debug.Log("[PlayFabQoS] Created PlayFabQoS singleton (ForScene)");
                singleton = this;
            }
        }

        private void ShutdownMultiplayerServerSuccess(EmptyResponse result)
        {
            Debug.Log("[PlayFabMultiplayerServer] Successfully Shutdown Down");
        }

        public void RequestMultiplayerServer()
        {
            RequestMultiplayerServerRequest request = new RequestMultiplayerServerRequest();
            request.BuildId = PlayFabServerInstance.singleton.BuildIdKey;
            request.PreferredRegions = new List<string> { "EastUs", "WestUs" };
            request.SessionId = Guid.NewGuid().ToString();
            PlayFabMultiplayerAPI.RequestMultiplayerServer(request, MultiplayerServerResponseSuccess, Error, null, null);
        }

        public void RequestMultiplayerServer(string PlayFabId)
        {
            RequestMultiplayerServerRequest request = new RequestMultiplayerServerRequest();
            request.BuildId = PlayFabServerInstance.singleton.BuildIdKey;
            request.PreferredRegions = new List<string> { "EastUs", "WestUs" };
            request.SessionId = Guid.NewGuid().ToString();
            PlayFabMultiplayerAPI.RequestMultiplayerServer(request, MultiplayerServerResponseSuccess, Error, null, null);
        }

        public void ShutDownMultiplayerServer(string sessionId, string region)
        {
            if (!PlayFabAuthenticationAPI.IsEntityLoggedIn())
            {
                PlayFabServerInstance.singleton.GetEntityToken();
            }

            ShutdownMultiplayerServerRequest request = new ShutdownMultiplayerServerRequest();
            request.BuildId = PlayFabServerInstance.singleton.BuildIdKey;
            request.SessionId = sessionId;
            request.Region = region;
            PlayFabMultiplayerAPI.ShutdownMultiplayerServer(request, ShutdownMultiplayerServerSuccess, Error, null, null);
            Debug.Log("[PlayFabMultiplayerServer] Shutting Down: " + sessionId + " Region: " + region);

        }

        public void ShutDownAllMultiplayerServer()
        {
            if (!PlayFabAuthenticationAPI.IsEntityLoggedIn())
            {
                PlayFabServerInstance.singleton.GetEntityToken();
            }

            foreach (KeyValuePair<string, string> server in servers)
            {
                ShutdownMultiplayerServerRequest request = new ShutdownMultiplayerServerRequest();
                request.BuildId = PlayFabServerInstance.singleton.BuildIdKey;
                request.SessionId = server.Value;
                request.Region = server.Key;
                PlayFabMultiplayerAPI.ShutdownMultiplayerServer(request, ShutdownMultiplayerServerSuccess, Error, null, null);
                Debug.Log("[PlayFabMultiplayerServer] Shutting Down: " + server.Value);
            }
        }

        private void MultiplayerServerResponseSuccess(RequestMultiplayerServerResponse result)
        {
            var portList = new System.Text.StringBuilder();
            foreach (Port port in result.Ports)
            {
                portList.Append("Name: " + port.Name + " Num: " + port.Num + " Prot: " + port.Protocol + ", ");
            }

            Debug.Log("[PlayFab Manager] Multiplayer Server Request Succeeded");
            Debug.Log("[PlayFab Manager] Server Info: IP - " + result.IPV4Address + " | Ports [" + portList.ToString() +  "]  | Region - " + result.Region + "  | Session Id: " + result.SessionId);
            
            //Add server to Dictionary so we can track it.
            servers.Add(result.Region, result.SessionId);
        }

        private void Error(PlayFabError error)
        {
            Debug.LogError("[PlayFabMultiplayerServer] Multiplayer Server Request Failed: " + error.ErrorMessage);
        }
    }
}
