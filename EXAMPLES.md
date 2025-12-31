# Example Usage Scenarios

## Scenario 1: Detecting a Port Scan

### Description
An attacker is scanning multiple ports on a target system to identify open services.

### How to Simulate
```csharp
public class PortScanSimulation : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
        StartCoroutine(SimulatePortScan());
    }
    
    IEnumerator SimulatePortScan()
    {
        string attackerIP = "203.0.113.5";
        string targetIP = "192.168.1.100";
        
        // Scan 20 different ports rapidly
        for (int i = 0; i < 20; i++)
        {
            var packet = new NetworkPacket(attackerIP, targetIP, ProtocolType.TCP, 64);
            packet.destinationPort = 20 + i; // Different ports
            manager.ProcessPacket(packet);
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
```

### Expected Visualization
- Multiple flow lines from attacker IP to target IP
- Attacker node turns red (suspicious)
- Anomaly alert: "Port scan detected: 20 unique ports accessed"
- Red glowing packets indicating anomalies

---

## Scenario 2: Visualizing Normal Web Traffic

### Description
Normal HTTPS web traffic between clients and servers.

### Expected Behavior
- Blue packets (HTTPS protocol color)
- Smooth flow animation between nodes
- Statistics showing HTTPS as dominant protocol
- No anomaly alerts

### Protocol Distribution Example
```
Protocol Distribution:
HTTPS: 450 (65.2%)
HTTP: 150 (21.7%)
DNS: 80 (11.6%)
TCP: 10 (1.5%)
```

---

## Scenario 3: DDoS Attack Detection

### Description
Multiple packets from a single source overwhelming a target.

### How to Simulate
```csharp
public class DDoSSimulation : MonoBehaviour
{
    private NetworkVisualizationManager manager;
    private SampleDataGenerator generator;
    
    void Start()
    {
        manager = FindObjectOfType<NetworkVisualizationManager>();
        generator = FindObjectOfType<SampleDataGenerator>();
        StartCoroutine(SimulateDDoS());
    }
    
    IEnumerator SimulateDDoS()
    {
        string attackerIP = "203.0.113.5";
        
        // Generate 60 packets rapidly
        var packets = generator.GeneratePacketBurst(60, attackerIP);
        
        foreach (var packet in packets)
        {
            manager.ProcessPacket(packet);
            yield return new WaitForSeconds(0.05f); // Very fast
        }
    }
}
```

### Expected Visualization
- Rapid stream of packets from single source
- Source node turns red and grows larger
- Anomaly alert: "Potential DDoS: 60 packets in 5s"
- Multiple red glowing packets
- High active packet count

---

## Scenario 4: Multi-Protocol Analysis

### Description
Analyzing diverse network traffic with multiple protocols.

### Expected Visualization
- Color-coded packets:
  - Light Blue: HTTP
  - Dark Blue: HTTPS
  - Orange: FTP
  - Purple: SSH
  - Green: DNS
  - Yellow: SMTP
  - Gray: TCP
  - Pink: UDP
- Protocol distribution chart in UI
- Multiple nodes in circular/grid layout
- Varied flow patterns

---

## Scenario 5: Filtering by Protocol

### Description
Using the control panel to filter specific protocols.

### Steps
1. Open Control Panel
2. Disable HTTP toggle
3. Enable HTTPS toggle only

### Expected Result
- Only HTTPS traffic visible
- Blue packets only
- Other protocols hidden from view
- Statistics still track all traffic

---

## Scenario 6: Anomaly-Only Mode

### Description
Focusing on security threats by showing only anomalous traffic.

### Steps
1. Open Control Panel
2. Enable "Show Anomalies Only" toggle

### Expected Result
- Only red packets visible
- Only suspicious nodes shown
- Flow lines only for anomalous connections
- Cleaner view for security analysis

---

## Scenario 7: Large File Transfer

### Description
Detecting unusually large packet transfers.

### How to Simulate
```csharp
public class LargeTransferSimulation : MonoBehaviour
{
    void SimulateLargeTransfer()
    {
        var manager = FindObjectOfType<NetworkVisualizationManager>();
        
        // Create oversized packet
        var packet = new NetworkPacket(
            "192.168.1.100", 
            "192.168.1.101", 
            ProtocolType.FTP, 
            12000  // 12KB - above threshold
        );
        
        manager.ProcessPacket(packet);
    }
}
```

### Expected Visualization
- Large packet visualization (bigger sphere)
- Red color indicating anomaly
- Anomaly alert: "Unusually large packet"
- Packet data shows 12KB size

---

## Scenario 8: Geographic Network Visualization

### Description
Simulating geographically distributed network nodes.

### Concept
Nodes could be positioned based on geographic location:
```csharp
public class GeoLocationLayout : MonoBehaviour
{
    Dictionary<string, Vector3> geoPositions = new Dictionary<string, Vector3>
    {
        {"8.8.8.8", new Vector3(10, 0, 15)},      // Google DNS (West)
        {"1.1.1.1", new Vector3(-10, 0, 15)},      // Cloudflare (West)
        {"192.168.1.100", new Vector3(0, 0, -10)} // Local network
    };
    
    // Use these positions when creating nodes
}
```

---

## Scenario 9: Real-time Dashboard Monitoring

### Expected UI Elements

**Statistics Panel:**
```
Total Packets: 1,245
Total Data: 1.82 MB
Active Packets: 23
Anomalies: 5

Protocol Distribution:
HTTPS: 845 (67.9%)
HTTP: 200 (16.1%)
DNS: 150 (12.0%)
SSH: 30 (2.4%)
FTP: 20 (1.6%)
```

**Anomaly Alert Panel:**
```
[ALERT] Port scan detected: 15 unique ports accessed
Source: 203.0.113.5
Time: 14:23:45

[ALERT] Potential DDoS: 55 packets in 5s
Source: 203.0.113.5
Time: 14:23:42

[ALERT] Unusually large packet
Source: 192.168.1.100
Time: 14:23:38
```

---

## Scenario 10: Camera Navigation Tour

### Navigation Example
```csharp
public class CameraTour : MonoBehaviour
{
    private CameraController cameraController;
    
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        StartCoroutine(TourNetwork());
    }
    
    IEnumerator TourNetwork()
    {
        var nodeVisualizer = FindObjectOfType<NetworkNodeVisualizer>();
        var nodes = nodeVisualizer.GetAllNodes();
        
        foreach (var node in nodes.Values)
        {
            // Focus on each node
            cameraController.FocusOn(node.position, 15f);
            yield return new WaitForSeconds(2f);
        }
        
        // Return to overview
        cameraController.ResetCamera();
    }
}
```

### Controls During Tour
- Right-click and drag: Manual rotation
- Scroll wheel: Adjust distance
- WASD: Override auto-tour with manual control
- Reset button: Return to default view

---

## Scenario 11: Custom Protocol Colors

### Customization Example
```csharp
public class CustomColors : MonoBehaviour
{
    void Start()
    {
        var colorMapper = FindObjectOfType<ProtocolColorMapper>();
        
        // Would need to add a SetColor method to ProtocolColorMapper
        // This is just an example of how it could work
    }
}
```

### Custom Scheme Idea
- Critical protocols: Warm colors (red, orange, yellow)
- Encrypted protocols: Cool colors (blue, purple)
- Unencrypted: Warning colors (yellow, orange)

---

## Scenario 12: Performance Testing

### High Load Simulation
```csharp
public class PerformanceTest : MonoBehaviour
{
    void TestHighLoad()
    {
        var manager = FindObjectOfType<NetworkVisualizationManager>();
        
        // Generate 200 packets rapidly
        StartCoroutine(GenerateHighTraffic());
    }
    
    IEnumerator GenerateHighTraffic()
    {
        var generator = GetComponent<SampleDataGenerator>();
        
        for (int i = 0; i < 200; i++)
        {
            var packet = generator.GenerateRandomPacket();
            manager.ProcessPacket(packet);
            
            if (i % 10 == 0)
                yield return null; // Yield occasionally
        }
    }
}
```

### Performance Metrics to Monitor
- FPS (should stay above 30)
- Active packet count
- Memory usage
- UI responsiveness

### Optimization Tips
- Reduce `maxActivePackets` to 50
- Increase `packetGenerationInterval` to 1.0s
- Disable line rendering for high traffic
- Use object pooling for packets

---

## Scenario 13: SSH Brute Force Attempt

### Description
Multiple failed SSH login attempts from same source.

### Characteristics
- Multiple SSH packets (purple)
- Same source IP
- Same destination port (22)
- Rapid succession

### Detection
- High packet rate from single source
- All targeting SSH port
- Flagged as potential DDoS or port scan

---

## Scenario 14: DNS Tunneling Detection

### Description
Unusually large DNS packets could indicate data exfiltration.

### Simulation
```csharp
var packet = new NetworkPacket(
    "192.168.1.100",
    "8.8.8.8",
    ProtocolType.DNS,
    5000  // Unusually large for DNS
);
```

### Expected Visualization
- Green packet (DNS color)
- Large size (unusual for DNS)
- Flagged as anomaly: "Unusually large packet"
- Could indicate data exfiltration

---

## Scenario 15: Network Segmentation Visualization

### Description
Visualizing traffic between different network segments.

### IP Ranges
- Internal network: 192.168.1.x
- DMZ: 10.0.0.x
- External: Various public IPs

### Visualization Strategy
- Layout nodes by subnet
- Internal nodes clustered together
- DMZ nodes in separate area
- External nodes on perimeter

### Custom Layout Example
```csharp
Vector3 CalculatePositionBySubnet(string ip)
{
    if (ip.StartsWith("192.168"))
        return new Vector3(-10, 0, 0); // Left side
    else if (ip.StartsWith("10.0"))
        return new Vector3(0, 0, 0);   // Center
    else
        return new Vector3(10, 0, 0);  // Right side
}
```

---

## Tips for Effective Visualization

### 1. Color Coding
- Use consistent colors for protocols
- Red always indicates threats
- Brightness indicates activity level

### 2. Size Scaling
- Larger packets = more data
- Larger nodes = more activity
- Use size to draw attention

### 3. Animation Speed
- Faster animation = real-time urgency
- Slower animation = detailed analysis
- Pause for forensic examination

### 4. Filtering
- Start with all protocols
- Filter incrementally
- Use anomaly-only mode for security focus

### 5. Camera Work
- Overview for general monitoring
- Zoom in for specific investigation
- Tour mode for presentations

---

## Integration with Real Systems

### Reading from PCAP Files
1. Use a PCAP parsing library
2. Convert packets to NetworkPacket format
3. Process through NetworkVisualizationManager
4. Control playback speed

### Live Network Capture
1. Capture packets using WinPcap/libpcap
2. Parse in real-time
3. Feed to visualization
4. Handle high traffic rates with buffering

### SIEM Integration
1. Receive alerts from SIEM
2. Highlight affected nodes
3. Play back historical traffic
4. Correlate with other events

### Log File Analysis
1. Parse log files (syslog, Windows Event Log)
2. Reconstruct network events
3. Visualize in 3D
4. Identify patterns and anomalies
