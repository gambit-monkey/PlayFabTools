using System;

namespace GambitMonkey.PlayFabTools.Models
{
    [Serializable]
    public class PlayFabQosServerModel
    {
        /// <summary>
        /// The region the QoS server is located in.
        /// </summary>
        public string Region;
        /// <summary>
        /// The QoS server URL.
        /// </summary>
        public string ServerUrl;
        /// <summary>
        /// Round Trip Time (RTT) of Client to PlayFab Server in milliseconds
        /// </summary>
        public long RTT;
    }
}