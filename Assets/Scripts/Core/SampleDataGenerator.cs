using UnityEngine;

namespace NetworkTrafficVisualizer.Core
{
    using Data;

    /// <summary>
    /// Generates sample network traffic data for testing and demonstration
    /// </summary>
    public class SampleDataGenerator : MonoBehaviour
    {
        [Header("IP Pool Settings")]
        [SerializeField] private string[] sampleIPs = new string[]
        {
            "192.168.1.100",
            "192.168.1.101",
            "192.168.1.102",
            "10.0.0.50",
            "10.0.0.51",
            "172.16.0.10",
            "172.16.0.11",
            "8.8.8.8",
            "1.1.1.1",
            "203.0.113.5"
        };

        [Header("Traffic Patterns")]
        [SerializeField] private float anomalyProbability = 0.1f; // 10% chance of anomaly
        [SerializeField] private int minPacketSize = 64;
        [SerializeField] private int maxPacketSize = 1500;

        private int packetCount = 0;

        /// <summary>
        /// Generate a random network packet
        /// </summary>
        public NetworkPacket GenerateRandomPacket()
        {
            string sourceIP = GetRandomIP();
            string destIP = GetRandomIP();
            
            // Ensure source and dest are different
            while (destIP == sourceIP)
            {
                destIP = GetRandomIP();
            }

            ProtocolType protocol = GetRandomProtocol();
            int packetSize = Random.Range(minPacketSize, maxPacketSize);

            NetworkPacket packet = new NetworkPacket(sourceIP, destIP, protocol, packetSize);

            // Occasionally generate anomalous traffic patterns
            if (Random.value < anomalyProbability)
            {
                packet = GenerateAnomalousPacket(sourceIP, destIP);
            }

            packetCount++;
            return packet;
        }

        /// <summary>
        /// Generate a packet with anomalous characteristics
        /// </summary>
        private NetworkPacket GenerateAnomalousPacket(string sourceIP, string destIP)
        {
            int anomalyType = Random.Range(0, 3);
            NetworkPacket packet;

            switch (anomalyType)
            {
                case 0: // Large packet
                    packet = new NetworkPacket(sourceIP, destIP, GetRandomProtocol(), Random.Range(5000, 15000));
                    break;
                case 1: // Unusual protocol
                    packet = new NetworkPacket(sourceIP, destIP, ProtocolType.Unknown, Random.Range(minPacketSize, maxPacketSize));
                    break;
                case 2: // Rapid fire from same source (will be detected by AnomalyDetector)
                default:
                    packet = new NetworkPacket(sourceIP, destIP, GetRandomProtocol(), Random.Range(minPacketSize, maxPacketSize));
                    break;
            }

            return packet;
        }

        /// <summary>
        /// Generate a burst of packets (simulating DDoS or port scan)
        /// </summary>
        public NetworkPacket[] GeneratePacketBurst(int count, string sourceIP = null)
        {
            if (string.IsNullOrEmpty(sourceIP))
            {
                sourceIP = GetRandomIP();
            }

            NetworkPacket[] packets = new NetworkPacket[count];
            
            for (int i = 0; i < count; i++)
            {
                string destIP = GetRandomIP();
                ProtocolType protocol = GetRandomProtocol();
                int packetSize = Random.Range(minPacketSize, maxPacketSize);

                packets[i] = new NetworkPacket(sourceIP, destIP, protocol, packetSize);
            }

            return packets;
        }

        /// <summary>
        /// Generate normal traffic pattern
        /// </summary>
        public NetworkPacket GenerateNormalPacket()
        {
            string sourceIP = GetRandomIP();
            string destIP = GetRandomIP();
            
            while (destIP == sourceIP)
            {
                destIP = GetRandomIP();
            }

            // Normal protocols are more common
            ProtocolType[] normalProtocols = {
                ProtocolType.HTTP,
                ProtocolType.HTTPS,
                ProtocolType.DNS,
                ProtocolType.TCP
            };

            ProtocolType protocol = normalProtocols[Random.Range(0, normalProtocols.Length)];
            int packetSize = Random.Range(minPacketSize, maxPacketSize);

            return new NetworkPacket(sourceIP, destIP, protocol, packetSize);
        }

        private string GetRandomIP()
        {
            return sampleIPs[Random.Range(0, sampleIPs.Length)];
        }

        private ProtocolType GetRandomProtocol()
        {
            // Weight towards common protocols
            float rand = Random.value;

            if (rand < 0.3f)
                return ProtocolType.HTTPS;
            else if (rand < 0.5f)
                return ProtocolType.HTTP;
            else if (rand < 0.6f)
                return ProtocolType.DNS;
            else if (rand < 0.7f)
                return ProtocolType.TCP;
            else if (rand < 0.8f)
                return ProtocolType.UDP;
            else if (rand < 0.85f)
                return ProtocolType.SSH;
            else if (rand < 0.9f)
                return ProtocolType.FTP;
            else if (rand < 0.95f)
                return ProtocolType.SMTP;
            else if (rand < 0.98f)
                return ProtocolType.ICMP;
            else
                return ProtocolType.Unknown;
        }

        public int GetPacketCount()
        {
            return packetCount;
        }

        public void ResetCount()
        {
            packetCount = 0;
        }
    }
}
