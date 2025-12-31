using UnityEngine;
using System.Collections;

namespace NetworkTrafficVisualizer.Core
{
    using Data;
    using Visualization;
    using UI;

    /// <summary>
    /// Main manager that integrates all visualization components
    /// </summary>
    public class NetworkVisualizationManager : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private PacketFlowManager packetFlowManager;
        [SerializeField] private ProtocolDistribution protocolDistribution;
        [SerializeField] private AnomalyDetector anomalyDetector;
        [SerializeField] private PacketVisualizer packetVisualizer;
        [SerializeField] private NetworkNodeVisualizer nodeVisualizer;
        [SerializeField] private FlowLineRenderer flowLineRenderer;
        [SerializeField] private ControlPanel controlPanel;

        [Header("Simulation Settings")]
        [SerializeField] private bool autoGenerateTraffic = true;
        [SerializeField] private float packetGenerationInterval = 0.5f;
        [SerializeField] private int maxNodesInSimulation = 10;

        private bool isRunning = false;

        private void Start()
        {
            InitializeComponents();
            SetupEventListeners();

            if (autoGenerateTraffic)
            {
                StartVisualization();
            }
        }

        private void InitializeComponents()
        {
            // Find components if not assigned
            if (packetFlowManager == null)
                packetFlowManager = FindObjectOfType<PacketFlowManager>();
            if (protocolDistribution == null)
                protocolDistribution = FindObjectOfType<ProtocolDistribution>();
            if (anomalyDetector == null)
                anomalyDetector = FindObjectOfType<AnomalyDetector>();
            if (packetVisualizer == null)
                packetVisualizer = FindObjectOfType<PacketVisualizer>();
            if (nodeVisualizer == null)
                nodeVisualizer = FindObjectOfType<NetworkNodeVisualizer>();
            if (flowLineRenderer == null)
                flowLineRenderer = FindObjectOfType<FlowLineRenderer>();
            if (controlPanel == null)
                controlPanel = FindObjectOfType<ControlPanel>();
        }

        private void SetupEventListeners()
        {
            if (packetFlowManager != null)
            {
                packetFlowManager.OnPacketReceived += HandlePacketReceived;
            }

            if (anomalyDetector != null)
            {
                anomalyDetector.OnAnomalyDetected += HandleAnomalyDetected;
            }

            if (controlPanel != null)
            {
                controlPanel.OnResetCamera += HandleResetCamera;
                controlPanel.OnClearVisualization += HandleClearVisualization;
            }
        }

        /// <summary>
        /// Start the visualization
        /// </summary>
        public void StartVisualization()
        {
            if (!isRunning)
            {
                isRunning = true;
                StartCoroutine(GenerateTrafficCoroutine());
            }
        }

        /// <summary>
        /// Stop the visualization
        /// </summary>
        public void StopVisualization()
        {
            isRunning = false;
            StopAllCoroutines();
        }

        /// <summary>
        /// Process a network packet through the visualization pipeline
        /// </summary>
        public void ProcessPacket(NetworkPacket packet)
        {
            if (packet == null) return;

            // Apply filters
            if (controlPanel != null)
            {
                if (!controlPanel.IsProtocolEnabled(packet.protocol))
                    return;
            }

            // Analyze packet for anomalies
            if (anomalyDetector != null)
            {
                anomalyDetector.AnalyzePacket(packet);
            }

            // Update protocol distribution
            if (protocolDistribution != null)
            {
                protocolDistribution.AnalyzePacket(packet);
            }

            // Apply anomaly-only filter
            if (controlPanel != null && controlPanel.ShowAnomaliesOnly() && !packet.isAnomaly)
            {
                return;
            }

            // Get or create network nodes
            var sourceNode = nodeVisualizer?.GetOrCreateNode(packet.sourceIP);
            var destNode = nodeVisualizer?.GetOrCreateNode(packet.destinationIP);

            if (sourceNode != null)
            {
                sourceNode.totalPacketsSent++;
                if (packet.isAnomaly)
                {
                    sourceNode.isSuspicious = true;
                    nodeVisualizer?.MarkNodeAsSuspicious(packet.sourceIP);
                }
            }

            if (destNode != null)
            {
                destNode.totalPacketsReceived++;
            }

            // Create packet visualization
            if (packetVisualizer != null && sourceNode != null && destNode != null)
            {
                GameObject packetObj = packetVisualizer.CreatePacketVisual(packet, sourceNode.position);

                // Add to flow manager
                if (packetFlowManager != null)
                {
                    packetFlowManager.AddPacket(packet, sourceNode.position, destNode.position, packetObj);
                }

                // Create flow line
                if (flowLineRenderer != null)
                {
                    flowLineRenderer.CreateAnimatedFlowLine(
                        sourceNode.position,
                        destNode.position,
                        1f,
                        packet.isAnomaly
                    );
                }
            }
        }

        private IEnumerator GenerateTrafficCoroutine()
        {
            while (isRunning)
            {
                // Generate sample packets (this would be replaced with real data in production)
                var generator = GetComponent<SampleDataGenerator>();
                if (generator != null)
                {
                    NetworkPacket packet = generator.GenerateRandomPacket();
                    ProcessPacket(packet);
                }

                yield return new WaitForSeconds(packetGenerationInterval);
            }
        }

        private void HandlePacketReceived(NetworkPacket packet)
        {
            // Packet has completed its journey
            // Could trigger additional effects here
        }

        private void HandleAnomalyDetected(NetworkAnomaly anomaly)
        {
            Debug.Log($"Anomaly detected: {anomaly.reason}");
        }

        private void HandleResetCamera()
        {
            var camera = FindObjectOfType<CameraController>();
            camera?.ResetCamera();
        }

        private void HandleClearVisualization()
        {
            if (flowLineRenderer != null)
            {
                flowLineRenderer.ClearAllLines();
            }
            
            if (protocolDistribution != null)
            {
                protocolDistribution.ResetStatistics();
            }
        }

        private void OnDestroy()
        {
            if (packetFlowManager != null)
            {
                packetFlowManager.OnPacketReceived -= HandlePacketReceived;
            }

            if (anomalyDetector != null)
            {
                anomalyDetector.OnAnomalyDetected -= HandleAnomalyDetected;
            }
        }
    }
}
