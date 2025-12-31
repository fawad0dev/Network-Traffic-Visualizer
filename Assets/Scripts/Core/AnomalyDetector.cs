using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NetworkTrafficVisualizer.Core
{
    using Data;

    /// <summary>
    /// Detects network anomalies based on various heuristics
    /// </summary>
    public class AnomalyDetector : MonoBehaviour
    {
        [Header("Detection Thresholds")]
        [SerializeField] private int portScanThreshold = 10; // Connections to different ports in time window
        [SerializeField] private float timeWindow = 5f; // Time window in seconds
        [SerializeField] private int ddosThreshold = 50; // Packets from same source in time window
        [SerializeField] private int unusualPacketSizeThreshold = 10000; // Bytes

        private Dictionary<string, List<PacketRecord>> recentPacketsBySource = new Dictionary<string, List<PacketRecord>>();
        private Dictionary<string, HashSet<int>> portAccessBySource = new Dictionary<string, HashSet<int>>();
        private List<NetworkAnomaly> detectedAnomalies = new List<NetworkAnomaly>();

        public delegate void AnomalyDetectedHandler(NetworkAnomaly anomaly);
        public event AnomalyDetectedHandler OnAnomalyDetected;

        private void Update()
        {
            CleanupOldRecords();
        }

        /// <summary>
        /// Analyze a packet for potential anomalies
        /// </summary>
        public bool AnalyzePacket(NetworkPacket packet)
        {
            bool isAnomalous = false;
            string reason = "";

            // Track packet
            TrackPacket(packet);

            // Check for port scanning
            if (DetectPortScan(packet.sourceIP, packet.destinationPort, out string portScanReason))
            {
                isAnomalous = true;
                reason = portScanReason;
            }

            // Check for DDoS patterns
            if (DetectDDoS(packet.sourceIP, out string ddosReason))
            {
                isAnomalous = true;
                reason = reason == "" ? ddosReason : reason + "; " + ddosReason;
            }

            // Check for unusual packet sizes
            if (packet.packetSize > unusualPacketSizeThreshold)
            {
                isAnomalous = true;
                reason = reason == "" ? "Unusually large packet" : reason + "; Unusually large packet";
            }

            if (isAnomalous)
            {
                packet.isAnomaly = true;
                packet.anomalyReason = reason;

                var anomaly = new NetworkAnomaly
                {
                    packet = packet,
                    detectedAt = Time.time,
                    reason = reason
                };

                detectedAnomalies.Add(anomaly);
                OnAnomalyDetected?.Invoke(anomaly);
            }

            return isAnomalous;
        }

        private void TrackPacket(NetworkPacket packet)
        {
            // Track by source
            if (!recentPacketsBySource.ContainsKey(packet.sourceIP))
            {
                recentPacketsBySource[packet.sourceIP] = new List<PacketRecord>();
            }
            recentPacketsBySource[packet.sourceIP].Add(new PacketRecord
            {
                timestamp = Time.time,
                destinationPort = packet.destinationPort
            });

            // Track port access
            if (!portAccessBySource.ContainsKey(packet.sourceIP))
            {
                portAccessBySource[packet.sourceIP] = new HashSet<int>();
            }
            portAccessBySource[packet.sourceIP].Add(packet.destinationPort);
        }

        private bool DetectPortScan(string sourceIP, int port, out string reason)
        {
            reason = "";
            if (portAccessBySource.ContainsKey(sourceIP))
            {
                var uniquePorts = portAccessBySource[sourceIP].Count;
                if (uniquePorts > portScanThreshold)
                {
                    reason = $"Port scan detected: {uniquePorts} unique ports accessed";
                    return true;
                }
            }
            return false;
        }

        private bool DetectDDoS(string sourceIP, out string reason)
        {
            reason = "";
            if (recentPacketsBySource.ContainsKey(sourceIP))
            {
                var recentPackets = recentPacketsBySource[sourceIP]
                    .Where(p => Time.time - p.timestamp <= timeWindow)
                    .Count();

                if (recentPackets > ddosThreshold)
                {
                    reason = $"Potential DDoS: {recentPackets} packets in {timeWindow}s";
                    return true;
                }
            }
            return false;
        }

        private void CleanupOldRecords()
        {
            float cutoffTime = Time.time - timeWindow * 2;

            foreach (var kvp in recentPacketsBySource.ToList())
            {
                recentPacketsBySource[kvp.Key] = kvp.Value
                    .Where(p => p.timestamp > cutoffTime)
                    .ToList();

                if (recentPacketsBySource[kvp.Key].Count == 0)
                {
                    recentPacketsBySource.Remove(kvp.Key);
                }
            }
        }

        public List<NetworkAnomaly> GetRecentAnomalies(float seconds)
        {
            float cutoff = Time.time - seconds;
            return detectedAnomalies.Where(a => a.detectedAt > cutoff).ToList();
        }

        public int GetTotalAnomaliesDetected()
        {
            return detectedAnomalies.Count;
        }

        private struct PacketRecord
        {
            public float timestamp;
            public int destinationPort;
        }
    }

    /// <summary>
    /// Represents a detected network anomaly
    /// </summary>
    public class NetworkAnomaly
    {
        public NetworkPacket packet;
        public float detectedAt;
        public string reason;
    }
}
