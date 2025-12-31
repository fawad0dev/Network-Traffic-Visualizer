using UnityEngine;
using System;

namespace NetworkTrafficVisualizer.Data
{
    /// <summary>
    /// Represents a network packet with essential metadata for visualization
    /// </summary>
    [Serializable]
    public class NetworkPacket
    {
        public string packetId;
        public string sourceIP;
        public string destinationIP;
        public int sourcePort;
        public int destinationPort;
        public ProtocolType protocol;
        public int packetSize;
        public float timestamp;
        public bool isAnomaly;
        public string anomalyReason;

        public NetworkPacket(string sourceIP, string destinationIP, ProtocolType protocol, int packetSize)
        {
            this.packetId = Guid.NewGuid().ToString();
            this.sourceIP = sourceIP;
            this.destinationIP = destinationIP;
            this.protocol = protocol;
            this.packetSize = packetSize;
            this.timestamp = Time.time;
            this.isAnomaly = false;
            this.sourcePort = UnityEngine.Random.Range(1024, 65535);
            this.destinationPort = GetDefaultPortForProtocol(protocol);
        }

        private int GetDefaultPortForProtocol(ProtocolType protocol)
        {
            switch (protocol)
            {
                case ProtocolType.HTTP:
                    return 80;
                case ProtocolType.HTTPS:
                    return 443;
                case ProtocolType.FTP:
                    return 21;
                case ProtocolType.SSH:
                    return 22;
                case ProtocolType.DNS:
                    return 53;
                case ProtocolType.SMTP:
                    return 25;
                default:
                    return UnityEngine.Random.Range(1024, 65535);
            }
        }
    }

    /// <summary>
    /// Network protocol types for visualization
    /// </summary>
    public enum ProtocolType
    {
        HTTP,
        HTTPS,
        FTP,
        SSH,
        DNS,
        SMTP,
        TCP,
        UDP,
        ICMP,
        Unknown
    }
}
