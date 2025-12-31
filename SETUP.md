# Setup Guide for Network Traffic Visualizer

## Unity Setup

### 1. System Requirements
- **Unity Version**: 2020.3 LTS or later recommended
- **Operating System**: Windows 10/11, macOS 10.13+, or Ubuntu 18.04+
- **RAM**: 8GB minimum, 16GB recommended
- **Graphics**: DirectX 11 or Metal compatible GPU

### 2. Installing Unity

#### Using Unity Hub (Recommended)
1. Download Unity Hub from [unity.com](https://unity.com/download)
2. Install Unity Hub
3. In Unity Hub, click "Installs" → "Add"
4. Select Unity 2020.3 LTS or later
5. Add modules if needed:
   - Build Support for your target platform
   - Documentation (optional)

### 3. Opening the Project

1. Launch Unity Hub
2. Click "Open" or "Add"
3. Navigate to the cloned repository folder
4. Select the root folder containing `Assets` and `ProjectSettings`
5. Unity will open and import the project

### 4. Initial Configuration

#### Required Packages
The project uses these Unity packages (automatically imported):
- TextMeshPro - For UI text rendering
- Unity UI - For user interface

#### First Time Setup
1. Wait for Unity to finish importing all assets
2. If prompted to import TextMeshPro, click "Import TMP Essentials"
3. Open the main scene: `Assets/Scenes/MainScene.unity`

### 5. Creating the Main Scene

Since this is a new project, you'll need to set up the scene:

#### Step 1: Create the Scene
1. In Unity, go to File → New Scene
2. Save as `MainScene.unity` in `Assets/Scenes/`

#### Step 2: Add Core Components
1. Create an empty GameObject: GameObject → Create Empty
2. Name it "NetworkVisualizationManager"
3. Add these components in the Inspector:
   - `NetworkVisualizationManager.cs`
   - `PacketFlowManager.cs`
   - `ProtocolDistribution.cs`
   - `AnomalyDetector.cs`
   - `PacketVisualizer.cs`
   - `NetworkNodeVisualizer.cs`
   - `FlowLineRenderer.cs`
   - `SampleDataGenerator.cs`

#### Step 3: Setup Camera
1. Select the Main Camera
2. Add the `CameraController.cs` component
3. Set the initial position to (0, 20, -20)
4. Set rotation to (30, 0, 0)

#### Step 4: Create UI Canvas
1. GameObject → UI → Canvas
2. Set Canvas Scaler to "Scale With Screen Size"
3. Reference resolution: 1920x1080

#### Step 5: Add UI Panels
Create child GameObjects under Canvas:

**Statistics Panel:**
1. UI → Panel (name it "StatisticsPanel")
2. Add `StatisticsPanel.cs` component
3. Create child TextMeshPro elements:
   - TotalPacketsText
   - TotalBytesText
   - ActivePacketsText
   - AnomalyCountText
   - ProtocolBreakdownText
4. Link these in the StatisticsPanel component

**Control Panel:**
1. UI → Panel (name it "ControlPanel")
2. Add `ControlPanel.cs` component
3. Add UI toggles for protocol filters
4. Add slider for packet speed
5. Add buttons for camera reset and clear

**Anomaly Alert Panel:**
1. UI → Panel (name it "AnomalyAlertPanel")
2. Add `AnomalyAlertPanel.cs` component
3. Create alert prefab with TextMeshPro

#### Step 6: Connect Components
In the NetworkVisualizationManager Inspector:
1. Drag and drop the components you added to the reference fields
2. Enable "Auto Generate Traffic" for testing
3. Set packet generation interval (0.5s recommended)

### 6. Testing the Setup

1. Press the Play button in Unity
2. You should see:
   - Network nodes appearing in 3D space
   - Packets flowing between nodes
   - Statistics updating in the UI
   - Occasional anomaly alerts

### 7. Building the Application

#### For Standalone (Windows/Mac/Linux):
1. File → Build Settings
2. Select your target platform
3. Click "Switch Platform" if needed
4. Add the MainScene to "Scenes in Build"
5. Click "Build" and choose output location

#### Build Settings Recommendations:
- **Development Build**: Enable for testing
- **Compression Method**: LZ4 for faster loading
- **Target Architecture**: x64

## Troubleshooting

### Common Issues

**Issue**: TextMeshPro errors
- **Solution**: Import TextMeshPro essentials: Window → TextMeshPro → Import TMP Essential Resources

**Issue**: Components not showing in Inspector
- **Solution**: Check for compilation errors in Console. Fix any C# syntax errors.

**Issue**: No packets appearing
- **Solution**: Check that "Auto Generate Traffic" is enabled in NetworkVisualizationManager

**Issue**: UI not visible
- **Solution**: Ensure Canvas render mode is "Screen Space - Overlay"

**Issue**: Performance issues
- **Solution**: Reduce "Max Active Packets" in PacketFlowManager
- Reduce "Packet Generation Interval" in NetworkVisualizationManager

### Getting Help

1. Check the Unity Console for errors (Window → General → Console)
2. Review component settings in Inspector
3. Verify all script references are connected
4. Check that scene is saved after setup

## Next Steps

1. Review the main README.md for usage instructions
2. Explore the API documentation
3. Customize visualization settings
4. Integrate with real network data sources

## Advanced Configuration

### Custom Materials
1. Create materials in `Assets/Materials/`
2. Assign custom shaders for effects
3. Link materials to visualizer components

### Performance Tuning
- Adjust Quality Settings: Edit → Project Settings → Quality
- Reduce shadow quality for better performance
- Disable VSync if needed: Edit → Project Settings → Quality → VSync Count

### Custom Layouts
Modify node layout in NetworkNodeVisualizer:
- Change layout type (Circular, Grid, Spherical)
- Adjust node spacing
- Customize positioning algorithm
