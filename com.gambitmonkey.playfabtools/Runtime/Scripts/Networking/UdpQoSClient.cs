using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;

namespace GambitMonkey.Networking
{
    public static class UdpQoSClient
    {
        public const int listenPort = 3075;

        private static void Ping()
        {
            using (UdpClient udpClient = new UdpClient())
            {
                //Creates an IPEndPoint to record the IP Address and port number of the sender. 
                // The IPEndPoint will allow you to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {

                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    //Asynchronous Stopwatch functions
                    //watch.Start();
                    //var t1 = Task.Factory.StartNew(MyMethod1);
                    //var t2 = Task.Factory.StartNew(MyMethod2);
                    //Task.Factory.ContinueWhenAll(new[] { t1, t2 }, tasks => watch.Stop());

                    UnityEngine.Debug.Log("[UdpQoSClient] This is the message you received " + returnData.ToString());
                    UnityEngine.Debug.Log("[UdpQoSClient] This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("[UdpQoSClient] Error: " + e.ToString());
                }
            }
        }

        //public static void Ping(string hostName, int port, string message)
        //{
        //    using (UdpClient udpClient = new UdpClient())
        //    {
        //        try
        //        {
        //            udpClient.Client.SendTimeout = 1000;
        //            udpClient.Client.ReceiveTimeout = 1000;
        //            udpClient.Connect(hostName, port);

        //            // Sends a message to the host to which you have connected.
        //            Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

        //            var watch = new Stopwatch();
        //            watch.Start();

        //            udpClient.Send(sendBytes, sendBytes.Length);

        //            //IPEndPoint object will allow us to read datagrams sent from any source.
        //            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        //            // Blocks until a message returns on this socket from a remote host.
        //            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
        //            string returnData = Encoding.ASCII.GetString(receiveBytes);

        //            watch.Stop();
        //            var roundTripLatency = watch.ElapsedMilliseconds;

        //            // Uses the IPEndPoint object to determine which host responded.
        //            UnityEngine.Debug.Log("[UdpQoSClient] This is the message you received " + returnData.ToString());
        //            UnityEngine.Debug.Log("[UdpQoSClient] This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
        //            UnityEngine.Debug.Log("[UdpQoSClient] RoundTripTime (RTT): " + roundTripLatency + "ms");
        //            udpClient.Close();
        //            watch.Reset();
        //        }
        //        catch (Exception e)
        //        {
        //            UnityEngine.Debug.LogError("[UdpQoSClient] Error: " + e.ToString());
        //        }
        //    }
        //}

        public static async Task Ping(string hostName, int port, string message)
        {
            SendMessage(hostName, port, message);
            ReceiveMessages();
        }

        public static IEnumerator Ping(string hostName, int port, byte[] message)
        {

            //SendMessage(hostName, port, message);
            //ReceiveMessages();
            yield return Send(hostName, port, message);
            //yield return null;

            //var task2watch = new Stopwatch();

            //task2watch.Start();
            //var watchtask2 = Send(hostName, port, message).ContinueWith(x =>
            //{
            //    task2watch.Stop();
            //    UnityEngine.Debug.Log("[PlayFabQoS] Server: " + hostName + " replied in " + task2watch.ElapsedMilliseconds + " ms");
            //});

            //return watchtask2;
        }

        public static IEnumerator Ping(string hostName, Dictionary<string,long> servers, int port, byte[] message)
        {

            //SendMessage(hostName, port, message);
            //ReceiveMessages();
            long rtt = Send(hostName, port, message);
            servers[hostName] = rtt;
            yield return rtt;
            //yield return null;

            //var task2watch = new Stopwatch();

            //task2watch.Start();
            //var watchtask2 = Send(hostName, port, message).ContinueWith(x =>
            //{
            //    task2watch.Stop();
            //    UnityEngine.Debug.Log("[PlayFabQoS] Server: " + hostName + " replied in " + task2watch.ElapsedMilliseconds + " ms");
            //});

            //return watchtask2;
        }

        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP)
        {
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }

        private struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
            public long l;
        }

        private static bool messageReceived = false;

        private static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);

            UnityEngine.Debug.Log($"Received: {receiveString}");
            messageReceived = true;
        }

        private static void ReceiveMessages()
        {
            messageReceived = false;
            // Receive a message and write it to the console.
            IPEndPoint e = new IPEndPoint(IPAddress.Any, listenPort);
            UdpClient u = new UdpClient(e);

            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

            //Console.WriteLine("listening for messages");
            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

            // Do some work while we wait for a message. For this example, we'll just sleep
            //while (!messageReceived)
            //{
            //    Thread.Sleep(100);
            //}
        }

        private static bool messageSent = false;

        private static void SendCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)ar.AsyncState;

            UnityEngine.Debug.Log($"number of bytes sent: {u.EndSend(ar)}");
            messageSent = true;
        }

        private static void SendMessage(string server, int port, string message)
        {
            messageSent = false;

            // create the udp socket
            UdpClient u = new UdpClient();
            byte[] sendBytes = Encoding.ASCII.GetBytes(message);

            // resolve the server name
            IPHostEntry heserver = Dns.GetHostEntry(server);

            IPEndPoint e = new IPEndPoint(heserver.AddressList[0], port);

            // send the message
            // the destination is defined by the IPEndPoint
            u.BeginSend(sendBytes, sendBytes.Length, e, new AsyncCallback(SendCallback), u);

            // Do some work while we wait for the send to complete. For this example, we'll just sleep
            //while (!messageSent)
            //{
            //    Thread.Sleep(100);
            //}
        }

        private static void SendMessage(string server, int port, byte[] message)
        {
            messageSent = false;

            // create the udp socket
            UdpClient u = new UdpClient();

            // resolve the server name
            IPHostEntry heserver = Dns.GetHostEntry(server);

            IPEndPoint e = new IPEndPoint(heserver.AddressList[0], port);

            // send the message
            // the destination is defined by the IPEndPoint
            //u.BeginSend(message, message.Length, e, new AsyncCallback(SendCallback), u);
            u.BeginSend(message, 2, new AsyncCallback(SendCallback), u);
            //u.Send(message, 2);
            // Do some work while we wait for the send to complete. For this example, we'll just sleep
            //while (!messageSent)
            //{
            //    Thread.Sleep(100);
            //}
        }

        /// <summary>
        /// Send Message and return RTT in Milliseconds
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        /// <returns>RTT in Milliseconds</returns>
        private static long Send(string server, int port, byte[] message)
        {
            UdpClient udpClient = new UdpClient();
            Stopwatch rtt = new Stopwatch();
            //try
            //{
            //UnityEngine.Debug.Log("Starting UDP connect");
            udpClient.Connect(server, port);

            udpClient.Send(message, 2);
            //DateTime sendTime = DateTime.Now;
            rtt.Start();
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
            //DateTime receiveTime = DateTime.Now;
            rtt.Stop();

            string returnData = Encoding.ASCII.GetString(receiveBytes);
            //UnityEngine.Debug.Log("Data Returned " + returnData);

            //TimeSpan measuredPing = receiveTime - sendTime;
            UnityEngine.Debug.Log("Ping from Server " + server + " took " + rtt.ElapsedMilliseconds + "ms"); //measuredPing.TotalSeconds + "ms");
            udpClient.Close();
            return rtt.ElapsedMilliseconds;
            //}
            //catch (Exception e)
            //{
            //    UnityEngine.Debug.Log(e.Message);
            //    UnityEngine.Debug.Log(e.StackTrace);
            //}
            //finally
            //{
            //    udpClient.Close();
            //}
        }        
    }
}