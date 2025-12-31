using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NetworkTrafficVisualizer.Core
{
    using Data;

    /// <summary>
    /// Analyzes protocol distribution and provides statistics
    /// </summary>
    public class ProtocolDistribution : MonoBehaviour
    {
        private Dictionary<ProtocolType, int> protocolCounts = new Dictionary<ProtocolType, int>();
        private Dictionary<ProtocolType, long> protocolBytes = new Dictionary<ProtocolType, long>();

        [Header("Statistics")]
        [SerializeField] private int totalPacketsAnalyzed = 0;
        [SerializeField] private long totalBytesAnalyzed = 0;

        private void Start()
        {
            InitializeProtocolDictionaries();
        }

        private void InitializeProtocolDictionaries()
        {
            foreach (ProtocolType protocol in System.Enum.GetValues(typeof(ProtocolType)))
            {
                protocolCounts[protocol] = 0;
                protocolBytes[protocol] = 0;
            }
        }

        /// <summary>
        /// Register a packet for protocol analysis
        /// </summary>
        public void AnalyzePacket(NetworkPacket packet)
        {
            if (packet == null) return;

            protocolCounts[packet.protocol]++;
            protocolBytes[packet.protocol] += packet.packetSize;
            totalPacketsAnalyzed++;
            totalBytesAnalyzed += packet.packetSize;
        }

        /// <summary>
        /// Get the percentage of traffic for a specific protocol
        /// </summary>
        public float GetProtocolPercentage(ProtocolType protocol)
        {
            if (totalPacketsAnalyzed == 0) return 0f;
            return (float)protocolCounts[protocol] / totalPacketsAnalyzed * 100f;
        }

        /// <summary>
        /// Get protocol counts dictionary
        /// </summary>
        public Dictionary<ProtocolType, int> GetProtocolCounts()
        {
            return new Dictionary<ProtocolType, int>(protocolCounts);
        }

        /// <summary>
        /// Get the most common protocol
        /// </summary>
        public ProtocolType GetMostCommonProtocol()
        {
            if (protocolCounts.Count == 0) return ProtocolType.Unknown;
            return protocolCounts.OrderByDescending(kvp => kvp.Value).First().Key;
        }

        /// <summary>
        /// Get total bytes for a protocol
        /// </summary>
        public long GetProtocolBytes(ProtocolType protocol)
        {
            return protocolBytes.ContainsKey(protocol) ? protocolBytes[protocol] : 0;
        }

        /// <summary>
        /// Reset all statistics
        /// </summary>
        public void ResetStatistics()
        {
            InitializeProtocolDictionaries();
            totalPacketsAnalyzed = 0;
            totalBytesAnalyzed = 0;
        }

        public int GetTotalPackets() => totalPacketsAnalyzed;
        public long GetTotalBytes() => totalBytesAnalyzed;
    }
}
