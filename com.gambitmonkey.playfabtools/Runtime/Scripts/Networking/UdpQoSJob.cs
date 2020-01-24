using System;
using System.Net.Sockets;
using Unity.Burst;
using Unity.Jobs;

namespace GambitMonkey.Networking.Jobs
{
    [BurstCompile]
    public struct UdpQosJob : IJob
    {
        //public string hostName { get; set; }
        //public int port { get; set; }
        //public byte[] message { get; set; }
        //public UdpClient udpClient { get; set; }
        //public IPEndPoint remoteIpEndPoint { get; set; }

        public void Execute()
        {
            //udpClient = new UdpClient();
            //UnityEngine.Debug.Log("Starting UDP connect");
            //udpClient.Connect(hostName, port);

            //udpClient.Send(message, 2);
            //DateTime sendTime = DateTime.Now;
            //IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            //Byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
            //DateTime receiveTime = DateTime.Now;

            //string returnData = Encoding.ASCII.GetString(receiveBytes);
            //UnityEngine.Debug.Log("Data Returned: " + returnData);

            //TimeSpan measuredPing = receiveTime - sendTime;
            //UnityEngine.Debug.Log("Ping from Server " + measuredPing.TotalSeconds);
            //udpClient.Close();
        }
    }
}