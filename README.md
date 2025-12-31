# Network Traffic Visualizer

An interactive 3D visualization tool for network traffic analysis using Unity's graphics capabilities.

## Overview

This Unity-based application provides real-time 3D visualization of network traffic, enabling cybersecurity professionals and network administrators to:
- Visualize packet flows between network nodes
- Analyze protocol distributions
- Detect and highlight network anomalies
- Monitor network security in real-time

## Features

### Core Visualization
- **3D Network Nodes**: Visual representation of IP addresses in 3D space
- **Packet Flow Animation**: Real-time animated packet movement between nodes
- **Protocol Color Coding**: Different colors for different protocols (HTTP, HTTPS, FTP, SSH, DNS, etc.)
- **Connection Lines**: Animated flow lines showing packet trajectories

### Security Analysis
- **Anomaly Detection**: Automatic detection of suspicious network activity
  - Port scanning detection
  - DDoS pattern recognition
  - Unusual packet size detection
- **Real-time Alerts**: Visual and UI alerts for detected anomalies
- **Suspicious Node Highlighting**: Automatic marking of suspicious IP addresses

### User Interface
- **Interactive Camera**: Full 3D camera control (WASD movement, mouse rotation, zoom)
- **Statistics Dashboard**: Real-time display of network metrics
- **Control Panel**: Filters for protocols and packet types
- **Anomaly Alert Panel**: Live feed of security alerts

### Network Analysis
- **Protocol Distribution**: Real-time analysis of protocol usage
- **Traffic Statistics**: Packet counts, bandwidth usage, and flow rates
- **Data Visualization**: Visual representation of network patterns

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/               # Core network traffic management
│   │   ├── NetworkVisualizationManager.cs
│   │   ├── PacketFlowManager.cs
│   │   ├── ProtocolDistribution.cs
│   │   ├── AnomalyDetector.cs
│   │   └── SampleDataGenerator.cs
│   ├── Data/               # Data models
│   │   ├── NetworkPacket.cs
│   │   └── NetworkNode.cs
│   ├── Visualization/      # 3D visualization components
│   │   ├── PacketVisualizer.cs
│   │   ├── NetworkNodeVisualizer.cs
│   │   ├── FlowLineRenderer.cs
│   │   └── ProtocolColorMapper.cs
│   └── UI/                 # User interface components
│       ├── CameraController.cs
│       ├── StatisticsPanel.cs
│       ├── AnomalyAlertPanel.cs
│       └── ControlPanel.cs
├── Materials/              # Materials for visualization
├── Prefabs/               # Reusable game objects
└── Scenes/                # Unity scenes
```

## Getting Started

### Prerequisites
- Unity 2020.3 LTS or later
- TextMeshPro package (included with Unity)

### Installation
1. Clone this repository
2. Open the project in Unity
3. Open the main scene from `Assets/Scenes/`
4. Press Play to start the visualization

### Basic Usage

#### Camera Controls
- **WASD**: Move camera forward/backward/left/right
- **Q/E**: Move camera down/up
- **Right Mouse Button + Drag**: Rotate camera
- **Arrow Keys**: Rotate camera
- **Mouse Scroll**: Zoom in/out
- **Shift**: Fast movement mode

#### UI Controls
- **Protocol Filters**: Toggle visibility of specific protocols
- **Show Anomalies Only**: Filter to show only anomalous traffic
- **Packet Speed Slider**: Adjust packet animation speed
- **Reset Camera**: Return camera to default position
- **Clear Visualization**: Clear all current visualizations

## Components Documentation

### NetworkVisualizationManager
Main orchestration component that coordinates all visualization systems.

**Key Methods:**
- `StartVisualization()`: Begin traffic visualization
- `StopVisualization()`: Pause visualization
- `ProcessPacket(NetworkPacket)`: Process and visualize a packet

### PacketFlowManager
Manages packet lifecycle and movement animation.

**Settings:**
- `packetSpeed`: Speed of packet animation
- `maxActivePackets`: Maximum simultaneous packets
- `packetLifetime`: How long packets remain visible

### AnomalyDetector
Detects suspicious network patterns using heuristics.

**Detection Methods:**
- Port scanning (multiple port connections)
- DDoS patterns (high packet rate from single source)
- Unusual packet sizes

**Thresholds:**
- `portScanThreshold`: Number of unique ports to trigger alert
- `ddosThreshold`: Packets per time window to trigger alert
- `unusualPacketSizeThreshold`: Packet size in bytes

### ProtocolDistribution
Analyzes and tracks protocol usage statistics.

**Tracked Metrics:**
- Packet count per protocol
- Bytes transferred per protocol
- Protocol percentages
- Total traffic statistics

### NetworkNodeVisualizer
Creates and manages 3D representations of network nodes.

**Layout Types:**
- `Circular`: Nodes arranged in a circle
- `Grid`: Nodes arranged in a grid pattern
- `Spherical`: Nodes distributed on a sphere

### PacketVisualizer
Creates visual representations of network packets.

**Features:**
- Size scaling based on packet size
- Color coding by protocol
- Anomaly highlighting (red glow)

## Protocol Color Scheme

| Protocol | Color |
|----------|-------|
| HTTP | Light Blue |
| HTTPS | Dark Blue |
| FTP | Orange |
| SSH | Purple |
| DNS | Green |
| SMTP | Yellow |
| TCP | Gray |
| UDP | Pink |
| ICMP | Cyan |
| Unknown | Dark Gray |
| **Anomaly** | **Red** |

## Cybersecurity Concepts

### Packet Analysis
The tool analyzes network packets including:
- Source and destination IP addresses
- Protocol types
- Packet sizes
- Port numbers
- Timestamps

### Network Protocols
Visualizes common network protocols:
- **HTTP/HTTPS**: Web traffic
- **FTP**: File transfers
- **SSH**: Secure shell connections
- **DNS**: Domain name resolution
- **SMTP**: Email traffic
- **TCP/UDP**: Transport layer protocols

### Network Security
Implements security monitoring features:
- **Port Scan Detection**: Identifies reconnaissance attempts
- **DDoS Detection**: Recognizes denial-of-service patterns
- **Anomaly Detection**: Flags unusual traffic patterns
- **Real-time Alerting**: Immediate notification of threats

## Customization

### Adding Custom Data Sources
Replace `SampleDataGenerator` with your own data source:

```csharp
public class YourDataSource : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
    }
    
    void ProcessNetworkData(/* your data */)
    {
        NetworkPacket packet = new NetworkPacket(
            sourceIP, destIP, protocol, packetSize
        );
        manager.ProcessPacket(packet);
    }
}
```

### Adjusting Detection Thresholds
Modify detection sensitivity in `AnomalyDetector`:
- `portScanThreshold`: Lower for more sensitive detection
- `ddosThreshold`: Adjust based on expected traffic volume
- `timeWindow`: Change analysis time window

### Custom Visualizations
Create custom packet or node visuals:
1. Create prefabs in Unity
2. Assign to `PacketVisualizer` or `NetworkNodeVisualizer`
3. Customize materials and shaders

## Performance Considerations

- **Maximum Active Packets**: Limit simultaneous packet visualizations
- **Packet Lifetime**: Reduce lifetime for better performance
- **Node Count**: Fewer nodes = better performance
- **Line Rendering**: Reduce line duration for complex scenes

## Future Enhancements

- Real network packet capture integration (pcap files)
- Geographic IP mapping
- Advanced filtering and search
- Historical traffic playback
- Export visualization data
- Machine learning for anomaly detection
- VR/AR support for immersive visualization

## License

This project is provided as-is for educational and professional use.

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## Acknowledgments

Built with Unity's powerful 3D graphics engine, demonstrating the intersection of game development technology and cybersecurity visualization.
