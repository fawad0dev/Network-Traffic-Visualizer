using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NetworkTrafficVisualizer.UI
{
    using Data;

    /// <summary>
    /// Control panel for filtering and visualization settings
    /// </summary>
    public class ControlPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Toggle showHTTPToggle;
        [SerializeField] private Toggle showHTTPSToggle;
        [SerializeField] private Toggle showFTPToggle;
        [SerializeField] private Toggle showSSHToggle;
        [SerializeField] private Toggle showDNSToggle;
        [SerializeField] private Toggle showAnomaliesOnlyToggle;
        [SerializeField] private Slider packetSpeedSlider;
        [SerializeField] private Button resetCameraButton;
        [SerializeField] private Button clearVisualizationButton;

        // Protocol filters
        private bool[] protocolFilters = new bool[10];

        public delegate void FilterChangedHandler();
        public event FilterChangedHandler OnFilterChanged;

        public delegate void SpeedChangedHandler(float speed);
        public event SpeedChangedHandler OnSpeedChanged;

        public delegate void ResetCameraHandler();
        public event ResetCameraHandler OnResetCamera;

        public delegate void ClearVisualizationHandler();
        public event ClearVisualizationHandler OnClearVisualization;

        private void Start()
        {
            InitializeControls();
            SetupEventListeners();
        }

        private void InitializeControls()
        {
            // Initialize all filters to true (show all)
            for (int i = 0; i < protocolFilters.Length; i++)
            {
                protocolFilters[i] = true;
            }
        }

        private void SetupEventListeners()
        {
            // Protocol toggles
            if (showHTTPToggle != null)
                showHTTPToggle.onValueChanged.AddListener(val => OnProtocolToggleChanged(ProtocolType.HTTP, val));
            if (showHTTPSToggle != null)
                showHTTPSToggle.onValueChanged.AddListener(val => OnProtocolToggleChanged(ProtocolType.HTTPS, val));
            if (showFTPToggle != null)
                showFTPToggle.onValueChanged.AddListener(val => OnProtocolToggleChanged(ProtocolType.FTP, val));
            if (showSSHToggle != null)
                showSSHToggle.onValueChanged.AddListener(val => OnProtocolToggleChanged(ProtocolType.SSH, val));
            if (showDNSToggle != null)
                showDNSToggle.onValueChanged.AddListener(val => OnProtocolToggleChanged(ProtocolType.DNS, val));

            // Anomaly filter
            if (showAnomaliesOnlyToggle != null)
                showAnomaliesOnlyToggle.onValueChanged.AddListener(OnAnomalyFilterChanged);

            // Speed slider
            if (packetSpeedSlider != null)
                packetSpeedSlider.onValueChanged.AddListener(OnPacketSpeedChanged);

            // Buttons
            if (resetCameraButton != null)
                resetCameraButton.onClick.AddListener(HandleResetCamera);
            if (clearVisualizationButton != null)
                clearVisualizationButton.onClick.AddListener(HandleClearVisualization);
        }

        private void OnProtocolToggleChanged(ProtocolType protocol, bool isEnabled)
        {
            protocolFilters[(int)protocol] = isEnabled;
            OnFilterChanged?.Invoke();
        }

        private void OnAnomalyFilterChanged(bool showOnlyAnomalies)
        {
            OnFilterChanged?.Invoke();
        }

        private void OnPacketSpeedChanged(float speed)
        {
            OnSpeedChanged?.Invoke(speed);
        }

        private void HandleResetCamera()
        {
            OnResetCamera?.Invoke();
        }

        private void HandleClearVisualization()
        {
            OnClearVisualization?.Invoke();
        }

        public bool IsProtocolEnabled(ProtocolType protocol)
        {
            return protocolFilters[(int)protocol];
        }

        public bool ShowAnomaliesOnly()
        {
            return showAnomaliesOnlyToggle != null && showAnomaliesOnlyToggle.isOn;
        }
    }
}
