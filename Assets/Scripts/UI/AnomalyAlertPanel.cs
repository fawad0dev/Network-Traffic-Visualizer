using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NetworkTrafficVisualizer.UI
{
    using Core;

    /// <summary>
    /// Displays anomaly alerts in the UI
    /// </summary>
    public class AnomalyAlertPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject alertPrefab;
        [SerializeField] private Transform alertContainer;
        [SerializeField] private int maxVisibleAlerts = 5;
        [SerializeField] private float alertDisplayTime = 5f;

        [Header("Colors")]
        [SerializeField] private Color criticalColor = Color.red;
        [SerializeField] private Color warningColor = Color.yellow;

        private AnomalyDetector anomalyDetector;

        private void Start()
        {
            anomalyDetector = FindObjectOfType<AnomalyDetector>();
            
            if (anomalyDetector != null)
            {
                anomalyDetector.OnAnomalyDetected += HandleAnomalyDetected;
            }
        }

        private void OnDestroy()
        {
            if (anomalyDetector != null)
            {
                anomalyDetector.OnAnomalyDetected -= HandleAnomalyDetected;
            }
        }

        private void HandleAnomalyDetected(NetworkAnomaly anomaly)
        {
            CreateAlertUI(anomaly);
        }

        private void CreateAlertUI(NetworkAnomaly anomaly)
        {
            GameObject alertObj;

            if (alertPrefab != null && alertContainer != null)
            {
                alertObj = Instantiate(alertPrefab, alertContainer);
            }
            else
            {
                Debug.LogWarning("Alert prefab or container not assigned");
                return;
            }

            // Set alert text
            var textComponent = alertObj.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"[ALERT] {anomaly.reason}\n" +
                                   $"Source: {anomaly.packet.sourceIP}\n" +
                                   $"Time: {System.DateTime.Now:HH:mm:ss}";
                
                // Set color based on severity
                if (anomaly.reason.Contains("DDoS") || anomaly.reason.Contains("Port scan"))
                {
                    textComponent.color = criticalColor;
                }
                else
                {
                    textComponent.color = warningColor;
                }
            }

            // Auto-destroy after display time
            Destroy(alertObj, alertDisplayTime);

            // Maintain max alerts
            if (alertContainer != null && alertContainer.childCount > maxVisibleAlerts)
            {
                Destroy(alertContainer.GetChild(0).gameObject);
            }
        }
    }
}
