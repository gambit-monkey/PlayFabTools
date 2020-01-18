using GambitMonkey.PlayFabTools;
using GambitMonkey.ScriptableObjects;
using Microsoft.Playfab.Gaming.GSDK.CSharp;
using PlayFab;
using PlayFab.AuthenticationModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.PlayFabTools
{
    public partial class PlayFabServerInstance : MonoBehaviour
    {
        public enum PlayFabRegions
        {
            AustraliaEast,
            AustraliaSoutheast,
            BrazilSouth,
            CentralUs,
            EastAsia,
            EastUs,
            EastUs2,
            JapanEast,
            JapanWest,
            NorthCentralUs,
            NorthEurope,
            SouthCentralUs,
            SoutheastAsia,
            WestEurope,
            WestUs,
            ChinaEast2,
            ChinaNorth2,
            SouthAfricaNorth
        }

        public enum EntityTypes
        {
            title,
            master_player_account,
            character,
            group,
        }

        /// <summary>
        /// A flag to control whether the PlayFabServerInstance object is destroyed when the scene changes.
        /// <para>This should be set if your game has a single PlayFabServerInstance that exists for the lifetime of the process. If there is a PlayFabServerInstance in each scene, then this should not be set.</para>
        /// </summary>
        [Header("Configuration")]
        public bool dontDestroyOnLoad = true;
        public bool showDebugMessages = true;
        public PlayFabSharedSettings playFabSharedSettings;
        //PlayFab Gamer Server Config
        IDictionary<string, string> config;

        [Header("PlayFab Multiplayer Settings")]
        // Here some other useful configuration keys (the full list is in the GameserverSDK class)
        public static string ServerIdKey; // ID given to your game server upon creation
        public static string LogFolderKey; // Path to the folder that should contain all log files
        public static string CertificateFolderKey; // Path to the folder that contains any game certificate files
        
        [Tooltip("PlayFab Build ID for this game server")]
        public string BuildIdKey;
        [Tooltip("PlayFab Region ID for this game server")]
        public PlayFabRegions playFabRegion;
        public static GameServerConnectionInfo connectionInfo;
        private List<ConnectedPlayer> _connectedPlayers;

        // These two keys are only available after allocation (once readyForPlayers returns true)
        public static string SessionCookieKey; // The Session Cookie you passed into the allocation call when you requested a server
        public static string SessionIdKey; // The Session ID you specified in the allocation call when you requested a server

        //PlayFabEvents
        [Header("Unity Events")]
        public UnityEvent OnShutDown;

        ///<summary>
        ///Optional Scriptable Object Events to mimic the Unity Events
        ///We give these as an option as they are easier to debug and raise
        ///</summary>
        [Header("Scriptable Object Events")]
        public GameEvent OnSOShutDown;

        /// <summary>
        /// PlayFabServerInstance singleton
        /// </summary>
        public static PlayFabServerInstance singleton;

        private void Awake()
        {
            InitializeSingleton();
        }

        private void Start()
        {
            //Retrieves the EntityToken
            GetEntityToken();

            // Get all the configuration values
            config = GameserverSDK.getConfigSettings();
            Debug.Log("[PlayFabServerInstance] Received PlayFab Config");
            connectionInfo = GameserverSDK.GetGameServerConnectionInfo();

            //Retrieve a particular configuration value
            if (config.TryGetValue(GameserverSDK.SessionCookieKey, out string sessionCookie))
            {
                //sessionCookie contains the value
            }

            _connectedPlayers = new List<Microsoft.Playfab.Gaming.GSDK.CSharp.ConnectedPlayer>();

            // This will add your log line the GSDK log file, alongside other information logged by the GSDK
            GameserverSDK.LogMessage("[PlayFabServerInstance] GameServer Starting");
            Debug.Log("[PlayFabServerInstance] GameServer Starting");

            // Alternatively, you can log your own files to the log directory
            string logFolder = GameserverSDK.GetLogsDirectory();

            // Call this while your game is initializing, it will start heartbeating to our agent and put the game server in an Initializing state
            GameserverSDK.Start();
            GameserverSDK.RegisterShutdownCallback(OnShutdown);
            GameserverSDK.RegisterMaintenanceCallback(OnMaintenanceScheduled);
            GameserverSDK.RegisterHealthCallback(IsHealthy);

            /* Add any other initializion code your game server needs before players can connect */

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
                    Debug.LogWarning("[PlayFabServerInstance] Multiple PlayFabServerInstance detected in the scene. Only one PlayFabServerInstance can exist at a time. The duplicate PlayFabServerInstance will be destroyed.");
                    Destroy(gameObject);
                    return;
                }
                Debug.Log("[PlayFabServerInstance] Created PlayFabServerInstance singleton (DontDestroyOnLoad)");
                singleton = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("[PlayFabServerInstance] Created PlayFabServerInstance singleton (ForScene)");
                singleton = this;
            }
        }

        // This method will be called on every heartbeat to check if your game is healthy, as such, it should return very quickly
        static bool IsHealthy()
        {
            // Return whether your game server should be considered healthy
            return true;
        }

        // This method will be called in case #3, when Azure will perform maintenance on the virtual machine
        void OnMaintenanceScheduled(DateTimeOffset time)
        {
            /* Perform any necessary cleanup, notify your players, etc. */
            Debug.LogFormat("Maintenance Scheduled for: {0}", time.UtcDateTime.ToLongDateString());
            GameserverSDK.LogMessage("GameServer Maintenance Scheduled for: " + time.UtcDateTime.ToLongDateString());
            //foreach (NetworkConnection conn in manager.Connections)
            //{
            //    conn.Send(CustomGameServerMessageTypes.ShutdownMessage, new MaintenanceMessage
            //    {
            //        ScheduledMaintenanceUTC = time.UtcDateTime
            //    });
            //}
        }

        private IEnumerator ReadyForPlayers()
        {
            yield return new WaitForSeconds(.5f);
            // Call this when your game is done initializing and players can connect
            // Note: This is a blocking call, and will return when this game server is either allocated or terminated
            GameserverSDK.ReadyForPlayers();
            if (showDebugMessages) Debug.Log("[PlayFabServerInstance] Ready For Players");
            // Call this when your game is done initializing and players can connect
            // Note: This is a blocking call, and will return when this game server is either allocated or terminated
            if (GameserverSDK.ReadyForPlayers())
            {
                // readyForPlayers returns true when an allocation call has been done, a player is about to connect!
                GameserverSDK.LogMessage("GameServer is ready for players");
            }
            else
            {
                // readyForPlayers returns false when the server is being terminated
                GameserverSDK.LogMessage("GameServer is NOT ready for players");
            }

        }

        public void OnReadyForPlayers()
        {
            StartCoroutine(ReadyForPlayers());
        }

        private void OnServerActive()
        {
            //Fires Start server event so listners can perform startup
            //playFabServerActiveEvent.Raise();
            Debug.Log("Server Started From Agent Activation");
            GameserverSDK.LogMessage("GameServer has become active");
        }

        public void OnPlayerRemoved(string playfabId)
        {
            if (_connectedPlayers != null)
            {
                ConnectedPlayer player = _connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, StringComparison.OrdinalIgnoreCase));
                _connectedPlayers.Remove(player);
                GameserverSDK.UpdateConnectedPlayers(_connectedPlayers);
                GameserverSDK.LogMessage("GameServer removed player: " + player.PlayerId);
                //PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
            }
        }

        public void OnPlayerAdded(string playfabId)
        {
            if (_connectedPlayers != null)
            {
                _connectedPlayers.Add(new ConnectedPlayer(playfabId));
                GameserverSDK.UpdateConnectedPlayers(_connectedPlayers);
                GameserverSDK.LogMessage("GameServer added player: " + playfabId);
            }
        }

        private void OnAgentError(string error)
        {
            Debug.Log(error);
        }

        private void OnShutdown()
        {
            Debug.Log("[PlayFabServerInstance] Server is Shutting down");
            GameserverSDK.LogMessage("Shutting down...");

            OnShutDown.Invoke();
            StartCoroutine(Shutdown());
        }

        private IEnumerator Shutdown()
        {
            yield return new WaitForSeconds(5f);
            Application.Quit();
        }

        private void OnMaintenance(DateTime? NextScheduledMaintenanceUtc)
        {
            Debug.LogFormat("[PlayFabServerInstance] Maintenance Scheduled for: {0}", NextScheduledMaintenanceUtc.Value.ToLongDateString());
        }

        public void GetEntityToken()
        {
            GetEntityTokenRequest request = new GetEntityTokenRequest();
            
            PlayFabAuthenticationAPI.GetEntityToken(request, GetEntityTokenResponseSuccess, Error, null, null);            
        }

        private void GetEntityTokenResponseSuccess(GetEntityTokenResponse result)
        {
            var entityId = result.Entity.Id;
            var entityType = result.Entity.Type;
            Debug.Log("[PlayFabServerInstance] Retrieved Entity Token: " + entityId + " Type: " + entityType);            
        }

        private void Error(PlayFabError error)
        {

        }
    }
}