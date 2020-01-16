using GambitMonkey.Networking;
using GambitMonkey.ScriptableObjects;
using PlayFab;
using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Unity.Jobs;
using Unity.Collections;
using GambitMonkey.Networking.Jobs;
using System.Net.Sockets;
using System.Net;
using GambitMonkey.Threading;

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
    [SerializeField]
    [Tooltip("Port to receive ping datagrams on")]
    private int receivePort = 3075;
    [SerializeField]
    [Tooltip("Port to send ping datagrams on")]
    private int sendPort = 3075;

    /// <summary>
    /// PlayFabClientInstance singleton
    /// </summary>
    public static PlayFabQoS singleton;

    //PlayFab QoS Servers returned by PlayFab 
    public static Dictionary<string, long> QoSServersByPing = new Dictionary<string, long>();
    public static Dictionary<string, string> QoSServersByRegion = new Dictionary<string, string>();
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
            //Dictionary of Servers by RTT
            QoSServersByPing.Add(server.ServerUrl, 1000);
            //dictionary of Servers by Region
            QoSServersByRegion.Add(server.Region, server.ServerUrl);
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
        worker.Start(UdpQoSClient.Ping(serverUrl, sendPort, datagram));


        //string sendIp = serverUrl;
        //connection = new UdpConnection();
        //connection.StartConnection(sendIp, sendPort, receivePort);
        //connection.Send("1111 1111 1111 1111");
        //foreach (var message in connection.getMessages()) Debug.Log(message);
        //connection.Stop();

        //var task2watch = new Stopwatch();
        //task2watch.Start();
        //byte firstByte = 255;
        //byte secondByte = 255;

        //Byte[] datagram = { firstByte, secondByte };

        ////byte[] sendBytes = Encoding.ASCII.GetBytes("1111111111111111");

        //var watchtask2 = UdpQoSClient.Ping(serverUrl, sendPort, datagram).ContinueWith(x =>
        //{
        //    task2watch.Stop();
        //    if(showPingMessages)UnityEngine.Debug.Log("[PlayFabQoS] Server: " + serverUrl + " replied in " + task2watch.ElapsedMilliseconds + " ms");
        //});
    }

    /// <summary>
    /// Pings all servers in the QoSServers Dictonary
    /// </summary>
    public void PingAllServers()
    {
        byte firstByte = 255;
        byte secondByte = 255;

        Byte[] datagram = { firstByte, secondByte };
        //NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);

        //Create an Array of Threadworkers for each QoSServer we need to ping
        ThreadWorker[] threadWorkers = new ThreadWorker[QoSServersByPing.Count];
        //index of threadWorkers;
        int indexThread = 0;
        foreach (KeyValuePair<string, long> server in QoSServersByPing)
        {

            //UdpClient udpClient = new UdpClient();
            //IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //JobHandle pingJob = PingJob(udpClient, remoteIpEndPoint, server.Value, sendPort, datagram);
            //jobHandleList.Add(pingJob);

            threadWorkers[indexThread] = new ThreadWorker();
            threadWorkers[indexThread].Start(UdpQoSClient.Ping(server.Key, QoSServersByPing, sendPort, datagram));
            indexThread++;
            //var task2watch = new Stopwatch();

            //task2watch.Start();
            //var watchtask2 = UdpQoSClient.Ping(server.Value, sendPort, datagram).ContinueWith(x =>
            //{
            //    task2watch.Stop();
            //   if(showPingMessages)UnityEngine.Debug.Log("[PlayFabQoS] Server: " + server.Value + " replied in " + task2watch.ElapsedMilliseconds + " ms");
            //});

            //StartCoroutine(Ping(server.Value,sendPort,datagram));

        //    Test().ContinueWith(
        //async (task) =>
        //{
        //    System.Console.WriteLine("Enter callback");
        //    await Task.Delay(1000);
        //    System.Console.WriteLine("Leave callback");
        //},
        //TaskContinuationOptions.AttachedToParent).Wait();

            //string sendIp = server.Value;
            //connection = new UdpConnection();
            //connection.StartConnection(sendIp, sendPort, receivePort);
            //connection.SendHostName("1111 1111 1111 1111");
            //Debug.Log("[PlayFabQoS] Pinging " + server.Key);
            //foreach (var message in connection.getMessages())
            //{
            //    if(showDebugMessages) Debug.Log("[PlayFabQoS] Received From " + server.Key);
            //}
            //connection.Stop();
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
        if (OnSOListServers)
            OnSOListServers.Raise(null);

        //JobHandle.CompleteAll(jobHandleList);
        //jobHandleList.Dispose();
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
