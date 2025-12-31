using UnityEngine;
using System.Collections.Generic;

namespace NetworkTrafficVisualizer.Visualization
{
    using Data;

    /// <summary>
    /// Visualizes network packets as 3D objects in the scene
    /// </summary>
    public class PacketVisualizer : MonoBehaviour
    {
        [Header("Packet Prefab")]
        [SerializeField] private GameObject packetPrefab;

        [Header("Size Settings")]
        [SerializeField] private float minPacketSize = 0.1f;
        [SerializeField] private float maxPacketSize = 0.5f;
        [SerializeField] private int minPacketBytes = 64;
        [SerializeField] private int maxPacketBytes = 1500;

        [Header("Colors")]
        [SerializeField] private ProtocolColorMapper colorMapper;

        private void Start()
        {
            if (colorMapper == null)
            {
                colorMapper = gameObject.AddComponent<ProtocolColorMapper>();
            }
        }

        /// <summary>
        /// Create a visual representation of a packet
        /// </summary>
        public GameObject CreatePacketVisual(NetworkPacket packet, Vector3 startPosition)
        {
            GameObject packetObj;

            if (packetPrefab != null)
            {
                packetObj = Instantiate(packetPrefab, startPosition, Quaternion.identity);
            }
            else
            {
                // Create a default sphere if no prefab is assigned
                packetObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                packetObj.transform.position = startPosition;
            }

            // Scale based on packet size
            float normalizedSize = Mathf.InverseLerp(minPacketBytes, maxPacketBytes, packet.packetSize);
            float visualSize = Mathf.Lerp(minPacketSize, maxPacketSize, normalizedSize);
            packetObj.transform.localScale = Vector3.one * visualSize;

            // Apply color based on protocol
            var renderer = packetObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = colorMapper.GetColorForProtocol(packet.protocol);
                
                // Highlight anomalies
                if (packet.isAnomaly)
                {
                    color = Color.red;
                }

                renderer.material.color = color;
                
                // Make anomalies glow
                if (packet.isAnomaly)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", Color.red * 0.5f);
                }
            }

            // Add packet data component
            var packetData = packetObj.AddComponent<PacketDataComponent>();
            packetData.packet = packet;

            return packetObj;
        }

        /// <summary>
        /// Update packet visual based on current state
        /// </summary>
        public void UpdatePacketVisual(GameObject packetObj, NetworkPacket packet)
        {
            if (packetObj == null) return;

            var renderer = packetObj.GetComponent<Renderer>();
            if (renderer != null && packet.isAnomaly)
            {
                renderer.material.color = Color.red;
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.red * 0.5f);
            }
        }
    }

    /// <summary>
    /// Component to attach packet data to game objects
    /// </summary>
    public class PacketDataComponent : MonoBehaviour
    {
        public NetworkPacket packet;
    }
}
