# API Documentation

## Core Classes

### NetworkPacket

Represents a network packet with metadata for visualization.

```csharp
namespace NetworkTrafficVisualizer.Data
{
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
    }
}
```

**Constructor:**
```csharp
NetworkPacket(string sourceIP, string destinationIP, ProtocolType protocol, int packetSize)
```

**Properties:**
- `packetId`: Unique identifier (auto-generated GUID)
- `sourceIP`: Source IP address
- `destinationIP`: Destination IP address
- `sourcePort`: Source port number (auto-assigned)
- `destinationPort`: Destination port (based on protocol)
- `protocol`: Network protocol type
- `packetSize`: Packet size in bytes
- `timestamp`: Time packet was created
- `isAnomaly`: Whether packet is flagged as anomalous
- `anomalyReason`: Description of anomaly if detected

---

### NetworkNode

Represents a network node (IP address) in 3D space.

```csharp
namespace NetworkTrafficVisualizer.Data
{
    public class NetworkNode
    {
        public string ipAddress;
        public Vector3 position;
        public int totalPacketsSent;
        public int totalPacketsReceived;
        public bool isSuspicious;
        public GameObject visualRepresentation;
    }
}
```

**Constructor:**
```csharp
NetworkNode(string ipAddress, Vector3 position)
```

---

### PacketFlowManager

Manages packet movement and lifecycle.

```csharp
namespace NetworkTrafficVisualizer.Core
{
    public class PacketFlowManager : MonoBehaviour
    {
        // Events
        public event PacketReceivedHandler OnPacketReceived;
        
        // Methods
        public void AddPacket(NetworkPacket packet, Vector3 sourcePos, 
                            Vector3 destPos, GameObject visualObject);
        public int GetActivePacketCount();
    }
}
```

**Settings:**
- `packetSpeed`: Speed of packet animation (default: 5)
- `maxActivePackets`: Maximum simultaneous packets (default: 100)
- `packetLifetime`: Maximum time before packet expires (default: 10s)

**Events:**
- `OnPacketReceived(NetworkPacket packet)`: Fired when packet reaches destination

**Methods:**
- `AddPacket()`: Add a packet to the flow system
- `GetActivePacketCount()`: Get number of currently animating packets

---

### ProtocolDistribution

Analyzes protocol distribution and statistics.

```csharp
namespace NetworkTrafficVisualizer.Core
{
    public class ProtocolDistribution : MonoBehaviour
    {
        // Methods
        public void AnalyzePacket(NetworkPacket packet);
        public float GetProtocolPercentage(ProtocolType protocol);
        public Dictionary<ProtocolType, int> GetProtocolCounts();
        public ProtocolType GetMostCommonProtocol();
        public long GetProtocolBytes(ProtocolType protocol);
        public void ResetStatistics();
        public int GetTotalPackets();
        public long GetTotalBytes();
    }
}
```

**Methods:**
- `AnalyzePacket(packet)`: Process a packet for statistics
- `GetProtocolPercentage(protocol)`: Get percentage of traffic for protocol
- `GetProtocolCounts()`: Get dictionary of protocol counts
- `GetMostCommonProtocol()`: Get the most frequently used protocol
- `GetProtocolBytes(protocol)`: Get total bytes for a protocol
- `ResetStatistics()`: Clear all statistics
- `GetTotalPackets()`: Get total packet count
- `GetTotalBytes()`: Get total bytes transferred

---

### AnomalyDetector

Detects network anomalies using heuristics.

```csharp
namespace NetworkTrafficVisualizer.Core
{
    public class AnomalyDetector : MonoBehaviour
    {
        // Events
        public event AnomalyDetectedHandler OnAnomalyDetected;
        
        // Methods
        public bool AnalyzePacket(NetworkPacket packet);
        public List<NetworkAnomaly> GetRecentAnomalies(float seconds);
        public int GetTotalAnomaliesDetected();
    }
}
```

**Settings:**
- `portScanThreshold`: Unique ports to trigger port scan alert (default: 10)
- `timeWindow`: Time window for analysis (default: 5s)
- `ddosThreshold`: Packets per window to trigger DDoS alert (default: 50)
- `unusualPacketSizeThreshold`: Packet size to flag as unusual (default: 10000 bytes)

**Events:**
- `OnAnomalyDetected(NetworkAnomaly anomaly)`: Fired when anomaly detected

**Methods:**
- `AnalyzePacket(packet)`: Analyze packet for anomalies, returns true if anomalous
- `GetRecentAnomalies(seconds)`: Get anomalies detected in last N seconds
- `GetTotalAnomaliesDetected()`: Get total count of detected anomalies

**Detection Algorithms:**
1. **Port Scan Detection**: Tracks unique destination ports per source IP
2. **DDoS Detection**: Monitors packet rate from single source
3. **Size Anomaly**: Flags unusually large packets

---

### PacketVisualizer

Creates visual representations of packets.

```csharp
namespace NetworkTrafficVisualizer.Visualization
{
    public class PacketVisualizer : MonoBehaviour
    {
        // Methods
        public GameObject CreatePacketVisual(NetworkPacket packet, Vector3 startPosition);
        public void UpdatePacketVisual(GameObject packetObj, NetworkPacket packet);
    }
}
```

**Settings:**
- `packetPrefab`: Optional custom prefab for packets
- `minPacketSize`: Minimum visual size (default: 0.1)
- `maxPacketSize`: Maximum visual size (default: 0.5)
- `minPacketBytes`: Minimum packet bytes for scaling (default: 64)
- `maxPacketBytes`: Maximum packet bytes for scaling (default: 1500)

**Methods:**
- `CreatePacketVisual(packet, position)`: Create visual representation
- `UpdatePacketVisual(obj, packet)`: Update existing visualization

**Visual Features:**
- Size scales with packet size
- Color based on protocol
- Red glow for anomalies

---

### NetworkNodeVisualizer

Manages network node visualizations.

```csharp
namespace NetworkTrafficVisualizer.Visualization
{
    public class NetworkNodeVisualizer : MonoBehaviour
    {
        // Methods
        public NetworkNode GetOrCreateNode(string ipAddress);
        public void UpdateNodeVisual(NetworkNode node);
        public Dictionary<string, NetworkNode> GetAllNodes();
        public void MarkNodeAsSuspicious(string ipAddress);
    }
}
```

**Settings:**
- `nodePrefab`: Optional custom prefab for nodes
- `normalNodeSize`: Size of normal nodes (default: 1.0)
- `suspiciousNodeSize`: Size of suspicious nodes (default: 1.5)
- `normalNodeColor`: Color for normal nodes (default: blue)
- `suspiciousNodeColor`: Color for suspicious nodes (default: red)
- `nodeSpacing`: Space between nodes (default: 10)
- `layoutType`: Node arrangement pattern (Circular/Grid/Spherical)

**Layout Types:**
- `Circular`: Nodes arranged in circle
- `Grid`: Nodes in grid pattern
- `Spherical`: Nodes distributed on sphere surface

**Methods:**
- `GetOrCreateNode(ip)`: Get existing node or create new one
- `UpdateNodeVisual(node)`: Update node appearance
- `GetAllNodes()`: Get dictionary of all nodes
- `MarkNodeAsSuspicious(ip)`: Flag node as suspicious

---

### FlowLineRenderer

Renders connection lines between nodes.

```csharp
namespace NetworkTrafficVisualizer.Visualization
{
    public class FlowLineRenderer : MonoBehaviour
    {
        // Methods
        public void CreateFlowLine(Vector3 start, Vector3 end, bool isAnomaly = false);
        public void CreateAnimatedFlowLine(Vector3 start, Vector3 end, 
                                          float animationDuration, bool isAnomaly = false);
        public void ClearAllLines();
    }
}
```

**Settings:**
- `lineMaterial`: Optional custom material for lines
- `lineWidth`: Width of flow lines (default: 0.1)
- `lineDuration`: How long lines remain visible (default: 2s)
- `normalFlowColor`: Color for normal traffic
- `anomalyFlowColor`: Color for anomalous traffic

**Methods:**
- `CreateFlowLine(start, end, isAnomaly)`: Create static line
- `CreateAnimatedFlowLine(start, end, duration, isAnomaly)`: Create animated line
- `ClearAllLines()`: Remove all lines

---

### NetworkVisualizationManager

Main orchestration component.

```csharp
namespace NetworkTrafficVisualizer.Core
{
    public class NetworkVisualizationManager : MonoBehaviour
    {
        // Methods
        public void StartVisualization();
        public void StopVisualization();
        public void ProcessPacket(NetworkPacket packet);
    }
}
```

**Settings:**
- `autoGenerateTraffic`: Auto-generate sample traffic (default: true)
- `packetGenerationInterval`: Time between generated packets (default: 0.5s)
- `maxNodesInSimulation`: Maximum nodes to simulate (default: 10)

**Component References:**
- `packetFlowManager`: Packet flow system
- `protocolDistribution`: Protocol analyzer
- `anomalyDetector`: Anomaly detection system
- `packetVisualizer`: Packet visualization
- `nodeVisualizer`: Node visualization
- `flowLineRenderer`: Connection line renderer
- `controlPanel`: UI control panel

**Methods:**
- `StartVisualization()`: Begin visualization
- `StopVisualization()`: Stop visualization
- `ProcessPacket(packet)`: Process and visualize a packet

**Pipeline:**
1. Apply protocol filters
2. Analyze for anomalies
3. Update statistics
4. Create/update nodes
5. Create packet visual
6. Animate packet flow
7. Render connection line

---

### CameraController

Controls 3D camera navigation.

```csharp
namespace NetworkTrafficVisualizer.UI
{
    public class CameraController : MonoBehaviour
    {
        // Methods
        public void FocusOn(Vector3 position, float distance = 20f);
        public void ResetCamera();
    }
}
```

**Settings:**
- `moveSpeed`: Camera movement speed (default: 10)
- `fastMoveMultiplier`: Speed multiplier with Shift (default: 3)
- `rotationSpeed`: Rotation speed (default: 100)
- `zoomSpeed`: Zoom speed (default: 10)
- `minZoom`: Minimum zoom distance (default: 5)
- `maxZoom`: Maximum zoom distance (default: 50)
- `enableMouseRotation`: Enable mouse rotation (default: true)
- `mouseSensitivity`: Mouse sensitivity (default: 2)

**Controls:**
- WASD: Move
- Q/E: Up/Down
- Right Mouse: Rotate
- Scroll: Zoom
- Shift: Fast move
- Arrow Keys: Rotate

**Methods:**
- `FocusOn(position, distance)`: Focus camera on position
- `ResetCamera()`: Return to default position

---

### SampleDataGenerator

Generates test network traffic.

```csharp
namespace NetworkTrafficVisualizer.Core
{
    public class SampleDataGenerator : MonoBehaviour
    {
        // Methods
        public NetworkPacket GenerateRandomPacket();
        public NetworkPacket[] GeneratePacketBurst(int count, string sourceIP = null);
        public NetworkPacket GenerateNormalPacket();
        public int GetPacketCount();
        public void ResetCount();
    }
}
```

**Settings:**
- `sampleIPs`: Array of IP addresses to use
- `anomalyProbability`: Chance of anomaly (default: 0.1 = 10%)
- `minPacketSize`: Minimum packet size (default: 64 bytes)
- `maxPacketSize`: Maximum packet size (default: 1500 bytes)

**Methods:**
- `GenerateRandomPacket()`: Generate random packet (may be anomalous)
- `GeneratePacketBurst(count, sourceIP)`: Generate burst of packets (simulates attack)
- `GenerateNormalPacket()`: Generate normal traffic packet
- `GetPacketCount()`: Get total generated count
- `ResetCount()`: Reset packet counter

---

## Enumerations

### ProtocolType

```csharp
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
```

---

## Usage Examples

### Creating a Custom Data Source

```csharp
using UnityEngine;
using NetworkTrafficVisualizer.Core;
using NetworkTrafficVisualizer.Data;

public class CustomDataSource : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
    }
    
    void OnNewPacketData(string sourceIP, string destIP, string protocol, int size)
    {
        // Parse protocol
        ProtocolType protocolType = ParseProtocol(protocol);
        
        // Create packet
        NetworkPacket packet = new NetworkPacket(sourceIP, destIP, protocolType, size);
        
        // Visualize
        manager.ProcessPacket(packet);
    }
    
    ProtocolType ParseProtocol(string protocol)
    {
        // Your parsing logic
        return ProtocolType.TCP;
    }
}
```

### Subscribing to Anomaly Events

```csharp
using UnityEngine;
using NetworkTrafficVisualizer.Core;

public class AnomalyLogger : MonoBehaviour
{
    void Start()
    {
        var detector = FindObjectOfType<AnomalyDetector>();
        detector.OnAnomalyDetected += LogAnomaly;
    }
    
    void LogAnomaly(NetworkAnomaly anomaly)
    {
        Debug.Log($"[ALERT] {anomaly.reason}");
        Debug.Log($"Source: {anomaly.packet.sourceIP}");
        Debug.Log($"Destination: {anomaly.packet.destinationIP}");
        
        // Save to file, send alert, etc.
    }
}
```

### Custom Node Layout

```csharp
using UnityEngine;
using NetworkTrafficVisualizer.Visualization;

public class CustomLayout : MonoBehaviour
{
    private NetworkNodeVisualizer nodeVisualizer;
    
    void Start()
    {
        nodeVisualizer = FindObjectOfType<NetworkNodeVisualizer>();
        // Set to your preferred layout
        // This would require exposing the layout type as public
    }
    
    // Or implement your own positioning
    Vector3 CalculateCustomPosition(int index)
    {
        // Your custom algorithm
        return new Vector3(index * 5, 0, 0);
    }
}
```

---

## Integration Examples

### Reading from PCAP File

```csharp
// Pseudocode - requires external pcap library
public class PcapReader : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
        ReadPcapFile("traffic.pcap");
    }
    
    void ReadPcapFile(string filename)
    {
        // Use pcap library to read file
        foreach (var rawPacket in pcapFile)
        {
            NetworkPacket packet = ConvertToNetworkPacket(rawPacket);
            manager.ProcessPacket(packet);
        }
    }
}
```

### Real-time Network Capture

```csharp
// Pseudocode - requires network capture library
public class LiveCapture : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
        StartCapture();
    }
    
    void OnPacketCaptured(RawPacket rawPacket)
    {
        NetworkPacket packet = ParsePacket(rawPacket);
        manager.ProcessPacket(packet);
    }
}
```
