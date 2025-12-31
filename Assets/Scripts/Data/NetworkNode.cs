using UnityEngine;
using System.Collections.Generic;

namespace NetworkTrafficVisualizer.Data
{
    /// <summary>
    /// Represents a network node (IP address) in the visualization
    /// </summary>
    public class NetworkNode
    {
        public string ipAddress;
        public Vector3 position;
        public int totalPacketsSent;
        public int totalPacketsReceived;
        public bool isSuspicious;
        public GameObject visualRepresentation;

        public NetworkNode(string ipAddress, Vector3 position)
        {
            this.ipAddress = ipAddress;
            this.position = position;
            this.totalPacketsSent = 0;
            this.totalPacketsReceived = 0;
            this.isSuspicious = false;
        }
    }
}
