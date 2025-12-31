using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NetworkTrafficVisualizer.UI
{
    using Core;

    /// <summary>
    /// Displays network traffic statistics in the UI
    /// </summary>
    public class StatisticsPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI totalPacketsText;
        [SerializeField] private TextMeshProUGUI totalBytesText;
        [SerializeField] private TextMeshProUGUI activePacketsText;
        [SerializeField] private TextMeshProUGUI anomalyCountText;
        [SerializeField] private TextMeshProUGUI protocolBreakdownText;

        [Header("Update Settings")]
        [SerializeField] private float updateInterval = 0.5f;

        private PacketFlowManager flowManager;
        private ProtocolDistribution protocolDistribution;
        private AnomalyDetector anomalyDetector;
        private float lastUpdateTime;

        private void Start()
        {
            // Find components in scene
            flowManager = FindObjectOfType<PacketFlowManager>();
            protocolDistribution = FindObjectOfType<ProtocolDistribution>();
            anomalyDetector = FindObjectOfType<AnomalyDetector>();

            lastUpdateTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateStatistics();
                lastUpdateTime = Time.time;
            }
        }

        private void UpdateStatistics()
        {
            // Update total packets
            if (totalPacketsText != null && protocolDistribution != null)
            {
                totalPacketsText.text = $"Total Packets: {protocolDistribution.GetTotalPackets()}";
            }

            // Update total bytes
            if (totalBytesText != null && protocolDistribution != null)
            {
                long bytes = protocolDistribution.GetTotalBytes();
                totalBytesText.text = $"Total Data: {FormatBytes(bytes)}";
            }

            // Update active packets
            if (activePacketsText != null && flowManager != null)
            {
                activePacketsText.text = $"Active Packets: {flowManager.GetActivePacketCount()}";
            }

            // Update anomaly count
            if (anomalyCountText != null && anomalyDetector != null)
            {
                int anomalies = anomalyDetector.GetTotalAnomaliesDetected();
                anomalyCountText.text = $"Anomalies: {anomalies}";
                
                // Change color based on anomaly count
                if (anomalies > 10)
                    anomalyCountText.color = Color.red;
                else if (anomalies > 5)
                    anomalyCountText.color = Color.yellow;
                else
                    anomalyCountText.color = Color.white;
            }

            // Update protocol breakdown
            if (protocolBreakdownText != null && protocolDistribution != null)
            {
                UpdateProtocolBreakdown();
            }
        }

        private void UpdateProtocolBreakdown()
        {
            var protocolCounts = protocolDistribution.GetProtocolCounts();
            string breakdown = "Protocol Distribution:\n";

            foreach (var kvp in protocolCounts)
            {
                if (kvp.Value > 0)
                {
                    float percentage = protocolDistribution.GetProtocolPercentage(kvp.Key);
                    breakdown += $"{kvp.Key}: {kvp.Value} ({percentage:F1}%)\n";
                }
            }

            protocolBreakdownText.text = breakdown;
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
