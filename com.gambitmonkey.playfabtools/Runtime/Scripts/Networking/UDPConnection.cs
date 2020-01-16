using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace GambitMonkey.Networking
{
    public class UdpConnection
    {
        private UdpClient udpClient;

        private readonly Queue<string> incomingQueue = new Queue<string>();
        Thread receiveThread;
        private bool threadRunning = false;
        private string senderIp;
        private int senderPort;

        public void StartConnection(string sendIp, int sendPort, int receivePort)
        {
            try { udpClient = new UdpClient(receivePort); }
            catch (Exception e)
            {
                Debug.Log("[UDPConnection] Failed to listen for UDP at port " + receivePort + ": " + e.Message);
                return;
            }
            Debug.Log("[UDPConnection] Created receiving client at ip  and port " + receivePort);
            this.senderIp = sendIp;
            this.senderPort = sendPort;

            Debug.Log("[UDPConnection] Set sendee at ip " + sendIp + " and port " + sendPort);

            StartReceiveThread();
        }

        private void StartReceiveThread()
        {
            receiveThread = new Thread(() => ListenForMessages(udpClient));
            receiveThread.IsBackground = true;
            threadRunning = true;
            receiveThread.Start();
        }

        private void ListenForMessages(UdpClient client)
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (threadRunning)
            {
                try
                {
                    Byte[] receiveBytes = client.Receive(ref remoteIpEndPoint); // Blocks until a message returns on this socket from a remote host.
                    string returnData = Encoding.UTF8.GetString(receiveBytes);

                    lock (incomingQueue)
                    {
                        incomingQueue.Enqueue(returnData);
                    }
                }
                catch (SocketException e)
                {
                    // 10004 thrown when socket is closed
                    if (e.ErrorCode != 10004) Debug.Log("[UDPConnection] Socket exception while receiving data from udp client: " + e.Message);
                }
                catch (Exception e)
                {
                    Debug.Log("[UDPConnection] Error receiving data from udp client: " + e.Message);
                }
                Thread.Sleep(1);
            }
        }

        public string[] getMessages()
        {
            string[] pendingMessages = new string[0];
            lock (incomingQueue)
            {
                pendingMessages = new string[incomingQueue.Count];
                int i = 0;
                while (incomingQueue.Count != 0)
                {
                    pendingMessages[i] = incomingQueue.Dequeue();
                    i++;
                }
            }

            return pendingMessages;
        }

        public void Send(string message)
        {
            Debug.Log(String.Format("Send msg to ip:{0} port:{1} msg:{2}", senderIp, senderPort, message));
            IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(senderIp), senderPort);
            Byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            udpClient.Send(sendBytes, sendBytes.Length, serverEndpoint);
        }

        public void SendHostName(string message)
        {
            IPEndPoint serverEndpoint = GetIPEndPointFromHostName(senderIp, senderPort, false);
            Debug.Log(String.Format("[UDPConnection] Send msg to ip:{0} port:{1} msg:{2}", serverEndpoint.Address, senderPort, message));
            Byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            udpClient.Send(sendBytes, sendBytes.Length, serverEndpoint);
        }

        public void Stop()
        {
            threadRunning = false;
            receiveThread.Abort();
            udpClient.Close();
        }

        public IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP)
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
    }
}