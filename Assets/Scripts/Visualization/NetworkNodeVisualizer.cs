using UnityEngine;
using System.Collections.Generic;

namespace NetworkTrafficVisualizer.Visualization
{
    using Data;

    /// <summary>
    /// Visualizes network nodes as 3D objects in the scene
    /// </summary>
    public class NetworkNodeVisualizer : MonoBehaviour
    {
        [Header("Node Prefab")]
        [SerializeField] private GameObject nodePrefab;

        [Header("Visual Settings")]
        [SerializeField] private float normalNodeSize = 1f;
        [SerializeField] private float suspiciousNodeSize = 1.5f;
        [SerializeField] private Color normalNodeColor = Color.blue;
        [SerializeField] private Color suspiciousNodeColor = Color.red;

        [Header("Layout Settings")]
        [SerializeField] private float nodeSpacing = 10f;
        [SerializeField] private LayoutType layoutType = LayoutType.Circular;

        private Dictionary<string, NetworkNode> activeNodes = new Dictionary<string, NetworkNode>();
        private int nodeCounter = 0;

        public enum LayoutType
        {
            Circular,
            Grid,
            Spherical
        }

        /// <summary>
        /// Get or create a network node visualization
        /// </summary>
        public NetworkNode GetOrCreateNode(string ipAddress)
        {
            if (activeNodes.ContainsKey(ipAddress))
            {
                return activeNodes[ipAddress];
            }

            // Calculate position based on layout
            Vector3 position = CalculateNodePosition(nodeCounter);
            NetworkNode node = new NetworkNode(ipAddress, position);

            // Create visual representation
            node.visualRepresentation = CreateNodeVisual(node);

            activeNodes[ipAddress] = node;
            nodeCounter++;

            return node;
        }

        private GameObject CreateNodeVisual(NetworkNode node)
        {
            GameObject nodeObj;

            if (nodePrefab != null)
            {
                nodeObj = Instantiate(nodePrefab, node.position, Quaternion.identity);
            }
            else
            {
                // Create a default cube if no prefab is assigned
                nodeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                nodeObj.transform.position = node.position;
            }

            nodeObj.name = $"Node_{node.ipAddress}";
            nodeObj.transform.localScale = Vector3.one * normalNodeSize;

            // Apply material
            var renderer = nodeObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = normalNodeColor;
            }

            // Add node data component
            var nodeData = nodeObj.AddComponent<NodeDataComponent>();
            nodeData.node = node;

            return nodeObj;
        }

        /// <summary>
        /// Update node visual based on activity
        /// </summary>
        public void UpdateNodeVisual(NetworkNode node)
        {
            if (node?.visualRepresentation == null) return;

            var renderer = node.visualRepresentation.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (node.isSuspicious)
                {
                    renderer.material.color = suspiciousNodeColor;
                    node.visualRepresentation.transform.localScale = Vector3.one * suspiciousNodeSize;
                    
                    // Add emission for suspicious nodes
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", suspiciousNodeColor * 0.3f);
                }
                else
                {
                    renderer.material.color = normalNodeColor;
                    node.visualRepresentation.transform.localScale = Vector3.one * normalNodeSize;
                }
            }
        }

        private Vector3 CalculateNodePosition(int index)
        {
            switch (layoutType)
            {
                case LayoutType.Circular:
                    return CalculateCircularPosition(index);
                case LayoutType.Grid:
                    return CalculateGridPosition(index);
                case LayoutType.Spherical:
                    return CalculateSphericalPosition(index);
                default:
                    return Vector3.zero;
            }
        }

        private Vector3 CalculateCircularPosition(int index)
        {
            float angle = index * (360f / Mathf.Max(1, activeNodes.Count + 1)) * Mathf.Deg2Rad;
            float radius = nodeSpacing;
            return new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );
        }

        private Vector3 CalculateGridPosition(int index)
        {
            int gridSize = Mathf.CeilToInt(Mathf.Sqrt(index + 1));
            int x = index % gridSize;
            int z = index / gridSize;
            return new Vector3(
                (x - gridSize / 2f) * nodeSpacing,
                0,
                (z - gridSize / 2f) * nodeSpacing
            );
        }

        private Vector3 CalculateSphericalPosition(int index)
        {
            float phi = Mathf.Acos(1 - 2 * (index + 0.5f) / (activeNodes.Count + 1));
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * index;
            float radius = nodeSpacing;

            return new Vector3(
                Mathf.Cos(theta) * Mathf.Sin(phi) * radius,
                Mathf.Cos(phi) * radius,
                Mathf.Sin(theta) * Mathf.Sin(phi) * radius
            );
        }

        public Dictionary<string, NetworkNode> GetAllNodes()
        {
            return new Dictionary<string, NetworkNode>(activeNodes);
        }

        public void MarkNodeAsSuspicious(string ipAddress)
        {
            if (activeNodes.ContainsKey(ipAddress))
            {
                activeNodes[ipAddress].isSuspicious = true;
                UpdateNodeVisual(activeNodes[ipAddress]);
            }
        }
    }

    /// <summary>
    /// Component to attach node data to game objects
    /// </summary>
    public class NodeDataComponent : MonoBehaviour
    {
        public NetworkNode node;
    }
}
