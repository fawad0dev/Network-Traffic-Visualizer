using UnityEngine;
using System.Collections.Generic;

namespace NetworkTrafficVisualizer.Visualization
{
    using Data;

    /// <summary>
    /// Maps network protocols to distinct colors for visualization
    /// </summary>
    public class ProtocolColorMapper : MonoBehaviour
    {
        private Dictionary<ProtocolType, Color> protocolColors = new Dictionary<ProtocolType, Color>();

        private void Awake()
        {
            InitializeColorMapping();
        }

        private void InitializeColorMapping()
        {
            protocolColors[ProtocolType.HTTP] = new Color(0.2f, 0.6f, 1f);      // Light Blue
            protocolColors[ProtocolType.HTTPS] = new Color(0f, 0.4f, 0.8f);     // Dark Blue
            protocolColors[ProtocolType.FTP] = new Color(1f, 0.6f, 0f);         // Orange
            protocolColors[ProtocolType.SSH] = new Color(0.5f, 0f, 0.8f);       // Purple
            protocolColors[ProtocolType.DNS] = new Color(0f, 0.8f, 0.4f);       // Green
            protocolColors[ProtocolType.SMTP] = new Color(1f, 0.8f, 0f);        // Yellow
            protocolColors[ProtocolType.TCP] = new Color(0.6f, 0.6f, 0.6f);     // Gray
            protocolColors[ProtocolType.UDP] = new Color(0.8f, 0.4f, 0.6f);     // Pink
            protocolColors[ProtocolType.ICMP] = new Color(0f, 1f, 1f);          // Cyan
            protocolColors[ProtocolType.Unknown] = new Color(0.3f, 0.3f, 0.3f); // Dark Gray
        }

        /// <summary>
        /// Get the color associated with a protocol
        /// </summary>
        public Color GetColorForProtocol(ProtocolType protocol)
        {
            if (protocolColors.ContainsKey(protocol))
            {
                return protocolColors[protocol];
            }
            return Color.white;
        }

        /// <summary>
        /// Get all protocol-color mappings
        /// </summary>
        public Dictionary<ProtocolType, Color> GetAllMappings()
        {
            return new Dictionary<ProtocolType, Color>(protocolColors);
        }
    }
}
