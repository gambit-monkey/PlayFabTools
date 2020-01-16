using GambitMonkey.PlayFabTools;
using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GambitMonkey.PlayFabTools
{
    public partial class PlayFabClientInstance : MonoBehaviour
    {
        /// <summary>
        /// A flag to control whether the PlayFabClientInstance object is destroyed when the scene changes.
        /// <para>This should be set if your game has a single PlayFabClientInstance that exists for the lifetime of the process. If there is a PlayFabClientInstance in each scene, then this should not be set.</para>
        /// </summary>
        [Header("Configuration")]
        public bool dontDestroyOnLoad = true;
        public PlayFabSharedSettings playFabSharedSettings;
        [Tooltip("PlayFab Build ID for this game server")]
        public string BuildIdKey;

        /// <summary>
        /// PlayFabUser signed into client instance
        /// </summary>
        public static PlayFabUser playFabUser;

        /// <summary>
        /// PlayFabClientInstance singleton
        /// </summary>
        public static PlayFabClientInstance singleton;        

        private void Awake()
        {
            InitializeSingleton();
        }

        public void SetPlayFabUser(PlayFabUser user)
        {
            playFabUser = user;
        }

        private void Start()
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
                    Debug.LogWarning("[PlayFabClientInstance] Multiple PlayFabClientInstance detected in the scene. Only one PlayFabClientInstance can exist at a time. The duplicate PlayFabClientInstance will be destroyed.");
                    Destroy(gameObject);
                    return;
                }
                Debug.Log("[PlayFabClientInstance] Created PlayFabClientInstance singleton (DontDestroyOnLoad)");
                singleton = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("[PlayFabClientInstance] Created PlayFabClientInstance singleton (ForScene)");
                singleton = this;
            }
        }

        private void GetPlayFabServers()
        {
            var getTitleEntityTokenRequest = new GetEntityTokenRequest();
            GetEntityTokenRequest tokenRequest = new GetEntityTokenRequest();

            PlayFabAuthenticationAPI.GetEntityToken(tokenRequest, TokenResponseSuccess, Error, null, null);

            ListMultiplayerServersRequest request = new ListMultiplayerServersRequest();
            //request.BuildId = MMOClientInstance.Singleton.PlayFabManager.BuildIdKey;
            //request.Region = MMOClientInstance.Singleton.PlayFabManager.playFabRegion.ToString();

            PlayFabMultiplayerAPI.ListMultiplayerServers(request, GetServerList, Error, null, null);
        }

        private void TokenResponseSuccess(GetEntityTokenResponse result)
        {
            var temp = result.Entity;
        }
        private void Error(PlayFabError error)
        {
            //UISceneGlobal.Singleton.ShowMessageDialog("PlayFab Server Entity Token", error.ErrorMessage);
        }

        private void GetServerList(ListMultiplayerServersResponse response)
        {
            //servers = response.MultiplayerServerSummaries;
        }        
    }
}
