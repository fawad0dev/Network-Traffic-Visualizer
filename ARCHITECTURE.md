# Architecture Overview

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Unity Game Engine                            │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │         NetworkVisualizationManager (Main Hub)             │ │
│  │                  (Orchestrates all components)             │ │
│  └─────────────────┬──────────────────────────────────────────┘ │
│                    │                                             │
│  ┌─────────────────┴────────────────────────────────┐          │
│  │                                                    │          │
│  ▼                                                    ▼          │
│  ┌──────────────────────┐         ┌─────────────────────────┐  │
│  │   Core Systems       │         │  Visualization Systems  │  │
│  ├──────────────────────┤         ├─────────────────────────┤  │
│  │ PacketFlowManager    │────────▶│ PacketVisualizer        │  │
│  │ ProtocolDistribution │────────▶│ NetworkNodeVisualizer   │  │
│  │ AnomalyDetector      │────────▶│ FlowLineRenderer        │  │
│  │ SampleDataGenerator  │────────▶│ ProtocolColorMapper     │  │
│  └──────────────────────┘         └─────────────────────────┘  │
│           │                                    │                 │
│           │                                    │                 │
│  ┌────────▼────────────────────────────────────▼──────────────┐ │
│  │                   Data Models                              │ │
│  ├────────────────────────────────────────────────────────────┤ │
│  │ NetworkPacket     │ NetworkNode     │ NetworkAnomaly      │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                   │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │                    UI Layer                                 ││
│  ├─────────────────────────────────────────────────────────────┤│
│  │ CameraController    │ StatisticsPanel   │ ControlPanel     ││
│  │ AnomalyAlertPanel   │ User Input        │ Display Elements ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
```

## Data Flow

```
Input Data (Network Packets)
        │
        ▼
┌───────────────────────┐
│ NetworkPacket Created │
└───────────┬───────────┘
            │
            ▼
┌───────────────────────────────┐
│ NetworkVisualizationManager   │
│    ProcessPacket()            │
└───────┬───────────────────────┘
        │
        ├──────────────────────────────┐
        │                              │
        ▼                              ▼
┌─────────────────┐          ┌──────────────────┐
│ AnomalyDetector │          │ ProtocolDist.   │
│  AnalyzePacket()│          │  AnalyzePacket() │
└────────┬────────┘          └────────┬─────────┘
         │                            │
         │ isAnomaly?                 │ Statistics
         │                            │
         ▼                            ▼
┌─────────────────────────────────────────────┐
│      Get/Create Network Nodes               │
│   (NetworkNodeVisualizer)                   │
└────────────────┬────────────────────────────┘
                 │
                 ▼
┌────────────────────────────────────────────┐
│   Create Packet Visual (PacketVisualizer) │
└────────────────┬───────────────────────────┘
                 │
                 ▼
┌────────────────────────────────────────────┐
│   Add to Flow Manager                      │
│   (PacketFlowManager.AddPacket())         │
└────────────────┬───────────────────────────┘
                 │
                 ▼
┌────────────────────────────────────────────┐
│   Create Flow Line                         │
│   (FlowLineRenderer)                       │
└────────────────┬───────────────────────────┘
                 │
                 ▼
┌────────────────────────────────────────────┐
│   3D Visualization in Scene                │
│   - Animated packet movement               │
│   - Color-coded protocols                  │
│   - Highlighted anomalies                  │
└────────────────────────────────────────────┘
```

## Component Responsibilities

### Core Layer

**NetworkVisualizationManager**
- Central orchestrator
- Coordinates all systems
- Processes incoming packets
- Manages component lifecycle

**PacketFlowManager**
- Manages packet animation
- Controls packet speed and lifetime
- Tracks active packets
- Handles packet completion events

**ProtocolDistribution**
- Analyzes protocol usage
- Maintains statistics
- Calculates percentages
- Tracks bandwidth per protocol

**AnomalyDetector**
- Detects suspicious patterns
- Port scan detection
- DDoS pattern recognition
- Size anomaly detection
- Fires anomaly events

**SampleDataGenerator**
- Generates test data
- Simulates various traffic patterns
- Creates anomalous traffic
- Provides packet bursts

### Data Layer

**NetworkPacket**
- Packet metadata container
- Source/destination info
- Protocol type
- Size and timestamp
- Anomaly flags

**NetworkNode**
- Represents IP addresses
- 3D position in space
- Traffic statistics
- Suspicious status
- Visual reference

**NetworkAnomaly**
- Anomaly details
- Associated packet
- Detection timestamp
- Reason description

### Visualization Layer

**PacketVisualizer**
- Creates packet GameObjects
- Applies protocol colors
- Scales by packet size
- Highlights anomalies

**NetworkNodeVisualizer**
- Creates node GameObjects
- Manages node layout (circular/grid/spherical)
- Updates node appearance
- Marks suspicious nodes

**FlowLineRenderer**
- Draws connection lines
- Animates flow lines
- Color codes by traffic type
- Manages line lifecycle

**ProtocolColorMapper**
- Maps protocols to colors
- Consistent color scheme
- Visual differentiation

### UI Layer

**CameraController**
- WASD movement
- Mouse rotation
- Zoom control
- Focus on objects
- Camera reset

**StatisticsPanel**
- Displays metrics
- Updates in real-time
- Protocol breakdown
- Traffic statistics

**ControlPanel**
- Protocol filters
- Speed adjustment
- View controls
- Clear visualization

**AnomalyAlertPanel**
- Shows alerts
- Real-time notifications
- Alert history
- Color-coded severity

## Event System

```
┌──────────────────────┐
│  AnomalyDetector     │
└──────────┬───────────┘
           │
           │ OnAnomalyDetected
           ▼
┌──────────────────────────────┐
│ NetworkVisualizationManager  │
│ AnomalyAlertPanel            │
└──────────────────────────────┘

┌──────────────────────┐
│  PacketFlowManager   │
└──────────┬───────────┘
           │
           │ OnPacketReceived
           ▼
┌──────────────────────────────┐
│ NetworkVisualizationManager  │
└──────────────────────────────┘

┌──────────────────────┐
│  ControlPanel        │
└──────────┬───────────┘
           │
           ├─ OnFilterChanged
           ├─ OnSpeedChanged
           ├─ OnResetCamera
           └─ OnClearVisualization
           ▼
┌──────────────────────────────┐
│ NetworkVisualizationManager  │
│ CameraController             │
│ FlowLineRenderer             │
└──────────────────────────────┘
```

## Lifecycle of a Packet

1. **Creation**: NetworkPacket instantiated with metadata
2. **Processing**: NetworkVisualizationManager.ProcessPacket()
3. **Analysis**: 
   - AnomalyDetector checks for threats
   - ProtocolDistribution updates statistics
4. **Node Management**:
   - Source node retrieved/created
   - Destination node retrieved/created
   - Node statistics updated
5. **Visualization**:
   - PacketVisualizer creates 3D object
   - Color applied based on protocol
   - Size scaled based on packet size
6. **Animation**:
   - PacketFlowManager handles movement
   - Packet travels from source to destination
   - FlowLineRenderer draws connection
7. **Completion**:
   - Packet reaches destination
   - OnPacketReceived event fired
   - Visual object destroyed
   - Statistics finalized

## Threading Model

Unity's main thread handles:
- All GameObject operations
- Rendering
- Physics
- User input
- UI updates

All components run on main thread (Unity requirement):
- No multi-threading for GameObject manipulation
- Coroutines for time-based operations
- Update() for per-frame processing

## Performance Considerations

### Bottlenecks
1. **GameObject creation**: Limit with maxActivePackets
2. **LineRenderer operations**: Constrain line duration
3. **UI updates**: Throttle with updateInterval
4. **Node count**: Affects layout calculations

### Optimizations
1. **Object pooling**: Reuse packet GameObjects
2. **LOD system**: Simplify distant objects
3. **Culling**: Don't render off-screen objects
4. **Batching**: Combine similar materials
5. **Update throttling**: Don't update every frame

## Extension Points

### Custom Data Sources
Replace `SampleDataGenerator`:
```
Your Data Source → NetworkPacket → ProcessPacket()
```

### Custom Visualizations
Replace default primitives:
- Assign custom prefabs to visualizers
- Create custom materials
- Implement custom shaders

### Custom Layouts
Modify `NetworkNodeVisualizer`:
- Implement geographic mapping
- Subnet-based clustering
- Custom positioning algorithms

### Custom Anomaly Detection
Extend `AnomalyDetector`:
- Add new detection algorithms
- Machine learning integration
- External threat intelligence

### Custom UI
Extend UI components:
- Add new panels
- Custom charts/graphs
- Advanced filtering
- Export capabilities

## Security Concepts Integration

### Packet Analysis
- Source/Destination tracking
- Protocol identification
- Port monitoring
- Size analysis

### Protocol Understanding
- HTTP/HTTPS (web traffic)
- FTP (file transfer)
- SSH (secure shell)
- DNS (name resolution)
- SMTP (email)

### Threat Detection
- **Port Scanning**: Multiple port connections
- **DDoS**: High packet rate from single source
- **Data Exfiltration**: Unusually large packets
- **Suspicious Patterns**: Rapid connections, unusual protocols

### Real-time Monitoring
- Live traffic visualization
- Immediate anomaly alerts
- Pattern recognition
- Statistical analysis

## Future Architecture Enhancements

1. **Data Pipeline**
   - Queue-based processing
   - Batch processing support
   - Historical playback

2. **Machine Learning**
   - Anomaly prediction
   - Pattern classification
   - Adaptive thresholds

3. **Distributed Processing**
   - Multi-threaded analysis
   - GPU acceleration
   - Network distribution

4. **Advanced Visualization**
   - VR/AR support
   - Geographic mapping
   - Time-series graphs
   - Heat maps

5. **Integration APIs**
   - REST API for external systems
   - WebSocket for real-time streaming
   - Plugin architecture
   - Export formats
