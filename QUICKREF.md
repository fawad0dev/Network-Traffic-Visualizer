# Quick Reference Guide

## Camera Controls

| Action | Controls |
|--------|----------|
| Move Forward | `W` |
| Move Backward | `S` |
| Move Left | `A` |
| Move Right | `D` |
| Move Up | `E` |
| Move Down | `Q` |
| Fast Move | Hold `Shift` + Movement |
| Rotate Camera | Right Mouse Button + Drag |
| Rotate Left | `Left Arrow` |
| Rotate Right | `Right Arrow` |
| Rotate Up | `Up Arrow` |
| Rotate Down | `Down Arrow` |
| Zoom In/Out | Mouse Scroll Wheel |
| Reset Camera | Click Reset Button in UI |

## Protocol Colors

| Protocol | Color | Hex |
|----------|-------|-----|
| HTTP | Light Blue | #33CCFF |
| HTTPS | Dark Blue | #0066CC |
| FTP | Orange | #FF9900 |
| SSH | Purple | #8000CC |
| DNS | Green | #00CC66 |
| SMTP | Yellow | #FFCC00 |
| TCP | Gray | #999999 |
| UDP | Pink | #CC6699 |
| ICMP | Cyan | #00FFFF |
| Unknown | Dark Gray | #4D4D4D |
| **Anomaly** | **Red** | **#FF0000** |

## Key Components

### NetworkVisualizationManager
- **Location**: Core
- **Purpose**: Main orchestrator
- **Key Method**: `ProcessPacket(NetworkPacket)`

### AnomalyDetector
- **Location**: Core
- **Purpose**: Threat detection
- **Detects**: Port scans, DDoS, large packets
- **Thresholds**: Configurable in Inspector

### PacketFlowManager
- **Location**: Core
- **Purpose**: Packet animation
- **Settings**: Speed, lifetime, max packets

### NetworkNodeVisualizer
- **Location**: Visualization
- **Purpose**: IP address visualization
- **Layouts**: Circular, Grid, Spherical

## Default Settings

| Component | Setting | Default Value |
|-----------|---------|---------------|
| PacketFlowManager | Packet Speed | 5 |
| PacketFlowManager | Max Active Packets | 100 |
| PacketFlowManager | Packet Lifetime | 10s |
| AnomalyDetector | Port Scan Threshold | 10 ports |
| AnomalyDetector | DDoS Threshold | 50 packets |
| AnomalyDetector | Time Window | 5 seconds |
| AnomalyDetector | Unusual Packet Size | 10000 bytes |
| NetworkVisualizationManager | Packet Generation Interval | 0.5s |
| NetworkVisualizationManager | Auto Generate Traffic | true |
| CameraController | Move Speed | 10 |
| CameraController | Rotation Speed | 100 |
| FlowLineRenderer | Line Duration | 2s |

## Anomaly Types

| Type | Description | Trigger |
|------|-------------|---------|
| Port Scan | Multiple port connections | >10 unique ports from one IP |
| DDoS | High packet rate | >50 packets in 5s from one IP |
| Large Packet | Unusual packet size | >10000 bytes |

## Common Tasks

### Start Visualization
```csharp
var manager = FindObjectOfType<NetworkVisualizationManager>();
manager.StartVisualization();
```

### Stop Visualization
```csharp
var manager = FindObjectOfType<NetworkVisualizationManager>();
manager.StopVisualization();
```

### Process Custom Packet
```csharp
var packet = new NetworkPacket("192.168.1.1", "192.168.1.2", ProtocolType.HTTPS, 1024);
var manager = FindObjectOfType<NetworkVisualizationManager>();
manager.ProcessPacket(packet);
```

### Get Statistics
```csharp
var protocolDist = FindObjectOfType<ProtocolDistribution>();
int totalPackets = protocolDist.GetTotalPackets();
long totalBytes = protocolDist.GetTotalBytes();
float httpsPercent = protocolDist.GetProtocolPercentage(ProtocolType.HTTPS);
```

### Check for Anomalies
```csharp
var detector = FindObjectOfType<AnomalyDetector>();
var recentAnomalies = detector.GetRecentAnomalies(60f); // Last 60 seconds
int totalAnomalies = detector.GetTotalAnomaliesDetected();
```

### Focus Camera on Position
```csharp
var camera = FindObjectOfType<CameraController>();
camera.FocusOn(new Vector3(10, 0, 10), 20f);
```

### Clear Visualization
```csharp
var flowRenderer = FindObjectOfType<FlowLineRenderer>();
flowRenderer.ClearAllLines();

var protocolDist = FindObjectOfType<ProtocolDistribution>();
protocolDist.ResetStatistics();
```

## File Structure Reference

```
Network-Traffic-Visualizer/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity          # Main visualization scene
│   ├── Scripts/
│   │   ├── Core/                    # Core logic
│   │   │   ├── AnomalyDetector.cs
│   │   │   ├── NetworkVisualizationManager.cs
│   │   │   ├── PacketFlowManager.cs
│   │   │   ├── ProtocolDistribution.cs
│   │   │   └── SampleDataGenerator.cs
│   │   ├── Data/                    # Data models
│   │   │   ├── NetworkNode.cs
│   │   │   └── NetworkPacket.cs
│   │   ├── UI/                      # User interface
│   │   │   ├── AnomalyAlertPanel.cs
│   │   │   ├── CameraController.cs
│   │   │   ├── ControlPanel.cs
│   │   │   └── StatisticsPanel.cs
│   │   └── Visualization/           # 3D visualization
│   │       ├── FlowLineRenderer.cs
│   │       ├── NetworkNodeVisualizer.cs
│   │       ├── PacketVisualizer.cs
│   │       └── ProtocolColorMapper.cs
│   ├── Materials/                   # Visual materials
│   └── Prefabs/                     # Reusable objects
├── ProjectSettings/                 # Unity settings
├── README.md                        # Main documentation
├── SETUP.md                        # Setup instructions
├── API.md                          # API documentation
├── EXAMPLES.md                     # Usage examples
└── ARCHITECTURE.md                 # System architecture
```

## Inspector Setup Checklist

When setting up the scene in Unity:

### NetworkVisualizationManager GameObject
- [ ] Add NetworkVisualizationManager component
- [ ] Add PacketFlowManager component
- [ ] Add ProtocolDistribution component
- [ ] Add AnomalyDetector component
- [ ] Add PacketVisualizer component
- [ ] Add NetworkNodeVisualizer component
- [ ] Add FlowLineRenderer component
- [ ] Add SampleDataGenerator component
- [ ] Link all components in NetworkVisualizationManager inspector

### Main Camera
- [ ] Add CameraController component
- [ ] Set initial position (0, 20, -20)
- [ ] Set initial rotation (30, 0, 0)

### UI Canvas
- [ ] Create Canvas with Screen Space Overlay
- [ ] Add StatisticsPanel
- [ ] Add ControlPanel
- [ ] Add AnomalyAlertPanel
- [ ] Link UI elements to their respective scripts

## Performance Tips

| Issue | Solution |
|-------|----------|
| Low FPS | Reduce Max Active Packets to 50 |
| Too many nodes | Increase packet generation interval |
| UI lag | Increase statistics update interval |
| Memory usage | Reduce packet lifetime |
| Stuttering | Enable VSync in Quality Settings |

## Troubleshooting Quick Fixes

| Problem | Quick Fix |
|---------|-----------|
| No packets appearing | Enable Auto Generate Traffic |
| Components not found | Check all references in Inspector |
| UI not visible | Set Canvas to Screen Space Overlay |
| Camera not moving | Check CameraController is attached |
| No anomalies detected | Lower detection thresholds |
| Compilation errors | Check all namespaces match |

## Keyboard Shortcuts Summary

| Shortcut | Action |
|----------|--------|
| `WASD` | Move camera |
| `QE` | Up/Down |
| `Shift` | Fast move |
| `Right Click + Drag` | Rotate |
| `Arrow Keys` | Rotate |
| `Scroll` | Zoom |

## Protocol Port Numbers

| Protocol | Default Port |
|----------|-------------|
| HTTP | 80 |
| HTTPS | 443 |
| FTP | 21 |
| SSH | 22 |
| DNS | 53 |
| SMTP | 25 |
| TCP/UDP | Random (1024-65535) |

## Sample IP Addresses

Default sample IPs used by generator:
- `192.168.1.100-102` - Local network
- `10.0.0.50-51` - Private network
- `172.16.0.10-11` - Private network
- `8.8.8.8` - Google DNS
- `1.1.1.1` - Cloudflare DNS
- `203.0.113.5` - Test network

## Event Subscriptions

Subscribe to anomaly detection:
```csharp
void Start() {
    var detector = FindObjectOfType<AnomalyDetector>();
    detector.OnAnomalyDetected += HandleAnomaly;
}

void HandleAnomaly(NetworkAnomaly anomaly) {
    Debug.Log($"Threat: {anomaly.reason}");
}
```

Subscribe to packet completion:
```csharp
void Start() {
    var flowManager = FindObjectOfType<PacketFlowManager>();
    flowManager.OnPacketReceived += HandlePacketComplete;
}

void HandlePacketComplete(NetworkPacket packet) {
    Debug.Log($"Packet delivered: {packet.packetId}");
}
```

## Build Settings

Recommended build configuration:
- Platform: Standalone (Windows/Mac/Linux)
- Architecture: x86_64
- Compression: LZ4
- Development Build: For testing
- Script Debugging: For development

## Version Requirements

- Unity: 2020.3 LTS or later
- .NET: Standard 2.1
- TextMeshPro: Included with Unity
- Operating System: Windows 10+, macOS 10.13+, Ubuntu 18.04+

## Getting Help

1. Check README.md for overview
2. Review SETUP.md for installation
3. Consult API.md for detailed API docs
4. See EXAMPLES.md for usage scenarios
5. Review ARCHITECTURE.md for system design
6. Check Unity Console for errors
7. Verify Inspector settings
8. Check GitHub Issues

## Quick Start Commands

```bash
# Clone repository
git clone https://github.com/fawad0dev/Network-Traffic-Visualizer.git

# Open in Unity Hub
# Unity Hub → Add → Select folder

# Play in Unity
# Open MainScene.unity → Press Play button
```

## Customization Quick Reference

### Change Colors
Modify `ProtocolColorMapper.InitializeColorMapping()`

### Adjust Detection
Modify thresholds in `AnomalyDetector` Inspector

### Change Layout
Set `layoutType` in `NetworkNodeVisualizer` Inspector

### Custom Packet Visuals
Create prefab → Assign to `PacketVisualizer.packetPrefab`

### Custom Node Visuals
Create prefab → Assign to `NetworkNodeVisualizer.nodePrefab`

## Key Namespaces

```csharp
using NetworkTrafficVisualizer.Core;
using NetworkTrafficVisualizer.Data;
using NetworkTrafficVisualizer.Visualization;
using NetworkTrafficVisualizer.UI;
```

## Unity Menu Items

- **File → Build Settings**: Configure build
- **Edit → Project Settings**: Adjust project settings
- **Window → General → Console**: View errors/logs
- **Window → Analysis → Profiler**: Performance analysis
