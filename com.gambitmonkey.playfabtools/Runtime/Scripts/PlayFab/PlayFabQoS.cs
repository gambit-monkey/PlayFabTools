using GambitMonkey.Networking;
using GambitMonkey.ScriptableObjects;
using PlayFab;
using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Jobs;
using Unity.Collections;
using GambitMonkey.Networking.Jobs;
using System.Net.Sockets;
using System.Net;
using GambitMonkey.Threading;
using GambitMonkey.PlayFabTools.Models;

[HelpURL("https://api.playfab.com/documentation/multiplayer/method/ListQosServers#request-properties")]
public partial class PlayFabQoS : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private bool dontDestroyOnLoad = true;
    [SerializeField]
    private bool showDebugMessages = false;
    [SerializeField]
    private bool showPingMessages = false;
    
    [Header("Networking")]
    //[SerializeField]
    //[Tooltip("Port to receive ping datagrams on")]
    //private int receivePort = 3075;
    [SerializeField]
    [Tooltip("Port to send/receive ping datagrams on")]
    private int udpPort = 3075;

    /// <summary>
    /// PlayFabClientInstance singleton
    /// </summary>
    public static PlayFabQoS singleton;

    //PlayFab QoS Servers returned by PlayFab 
    public static List<PlayFabQosServerModel> QosServers = new List<PlayFabQosServerModel>();

    [Header("Unity Events")]
    [SerializeField]
    public UnityEvent OnListServers;

    [Header("Scriptable Object Events")]
    [SerializeField]
    public ListEvent OnSOListServers;

    //awake is called before all other methods
    private void Awake()
    {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GetQoSServers();
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //TODO: when client is connected then we will ping the server in their region so we can calculate latency
    }

    public void GetQoSServers()
    {
        ListQosServersRequest request = new ListQosServersRequest();
        PlayFabMultiplayerAPI.ListQosServers(request, ListQoSServersResponseSuccess, ListQoSServersResponseError, null, null);
    }

    private void ListQoSServersResponseSuccess(ListQosServersResponse result)
    {
        UnityEngine.Debug.Log("[PlayFabQoS] Retrieved " + result.QosServers.Count + " QoS Servers");
        foreach (QosServer server in result.QosServers)
        {
            //List of PlayFabQosServerModel(s)
            PlayFabQosServerModel serverModel = new PlayFabQosServerModel();
            serverModel.Region = server.Region;
            serverModel.ServerUrl = server.ServerUrl;
            //List of QosServers 
            QosServers.Add(serverModel);
        }
        //Retrieved all QoSServers now ping them so we have RTT for them
        PingAllServers();
    }

    private void ListQoSServersResponseError(PlayFabError error)
    {
        UnityEngine.Debug.LogError("[PlayFabQoS] Error trying to retrieve QoS Servers: " + error.ErrorMessage);
    }

    /// <summary>
    /// Pings a PlayFab server by url for QoS metrics
    /// </summary>
    /// <param name="serverUrl"></param>
    public void Ping(string serverUrl)
    {
        byte firstByte = 255;
        byte secondByte = 255;

        Byte[] datagram = { firstByte, secondByte };

        ThreadWorker worker = new ThreadWorker();
        worker.Start(UdpQoSClient.Ping(serverUrl, udpPort, datagram));
    }

    /// <summary>
    /// Pings all servers in the QoSServers Dictonary
    /// </summary>
    public void PingAllServers()
    {
        byte firstByte = 255;
        byte secondByte = 255;

        Byte[] datagram = { firstByte, secondByte };
        
        //Create an Array of Threadworkers for each QoSServer we need to ping
        ThreadWorker[] threadWorkers = new ThreadWorker[QosServers.Count];
        //index of threadWorkers;
        int indexThread = 0;

        foreach (PlayFabQosServerModel server in QosServers)
        {
            threadWorkers[indexThread] = new ThreadWorker();
            threadWorkers[indexThread].Start(UdpQoSClient.Ping(server, udpPort, datagram));
            indexThread++;
        }

        bool threadsWorking = true;
        //Loop through all Ping threads until they have completed and then Invoke completion events
        while (threadsWorking)
        {
            foreach (ThreadWorker worker in threadWorkers)
            {
                if (worker.IsRunning())
                {
                    threadsWorking = true;
                    break;
                }
                else
                {
                    threadsWorking = false;
                }
            }
        }

        threadWorkers = null;
        OnListServers.Invoke();
        //if (OnSOListServers)
        //    OnSOListServers.Raise(QosServers);
    }

    private IEnumerator Ping(string hostName, int port, byte[] message)
    {
        UdpQoSClient.Ping(hostName, port, message);
        yield return null;
    }

    private JobHandle PingJob(UdpClient udpClient, IPEndPoint remoteIpEndPoint, string hostName, int port, byte[] message)
    {
        UdpQosJob job = new UdpQosJob();
        job.udpClient = udpClient;
        //job.remoteIpEndPoint = remoteIpEndPoint;
        job.hostName = hostName;
        job.port = port;
        job.message = message;
        return job.Schedule();
    }

    ///<summary>
    ///Creates the UDP Packet PlayFab requires ot ping their QoS servers
    ///Send a single UDP datagram to port 3075 on the QoS server. The message content must start with 2 bytes of 255 (1111 1111 1111 1111).
    ///The server will reply with a single datagram, with the message contents having first 2 bytes "flipped" to 0 (0000 0000 0000 0000). The rest of the datagram contents will be copied from the initial ping.
    ///Measure the time between sending the UDP message and receiving a response.
    /// </summary>
}
