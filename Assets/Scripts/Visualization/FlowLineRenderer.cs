using UnityEngine;
using System.Collections.Generic;

namespace NetworkTrafficVisualizer.Visualization
{
    using Data;

    /// <summary>
    /// Renders connection lines between network nodes
    /// </summary>
    public class FlowLineRenderer : MonoBehaviour
    {
        [Header("Line Settings")]
        [SerializeField] private Material lineMaterial;
        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private float lineDuration = 2f;

        [Header("Colors")]
        [SerializeField] private Color normalFlowColor = new Color(0.5f, 0.5f, 1f, 0.5f);
        [SerializeField] private Color anomalyFlowColor = new Color(1f, 0f, 0f, 0.8f);

        private List<ConnectionLine> activeLines = new List<ConnectionLine>();

        private void Update()
        {
            UpdateLines();
        }

        /// <summary>
        /// Create a line between two positions
        /// </summary>
        public void CreateFlowLine(Vector3 start, Vector3 end, bool isAnomaly = false)
        {
            GameObject lineObj = new GameObject("FlowLine");
            lineObj.transform.parent = transform;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            
            if (lineMaterial != null)
            {
                lr.material = lineMaterial;
            }
            else
            {
                // Create a default material
                lr.material = new Material(Shader.Find("Sprites/Default"));
            }

            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

            Color lineColor = isAnomaly ? anomalyFlowColor : normalFlowColor;
            lr.startColor = lineColor;
            lr.endColor = lineColor;

            // Add to active lines
            activeLines.Add(new ConnectionLine
            {
                lineObject = lineObj,
                lineRenderer = lr,
                createdAt = Time.time,
                duration = lineDuration,
                isAnomaly = isAnomaly
            });
        }

        /// <summary>
        /// Create an animated line that grows from start to end
        /// </summary>
        public void CreateAnimatedFlowLine(Vector3 start, Vector3 end, float animationDuration, bool isAnomaly = false)
        {
            GameObject lineObj = new GameObject("AnimatedFlowLine");
            lineObj.transform.parent = transform;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            
            if (lineMaterial != null)
            {
                lr.material = lineMaterial;
            }
            else
            {
                lr.material = new Material(Shader.Find("Sprites/Default"));
            }

            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, start); // Start at same position

            Color lineColor = isAnomaly ? anomalyFlowColor : normalFlowColor;
            lr.startColor = lineColor;
            lr.endColor = lineColor;

            // Add animated line component
            var animator = lineObj.AddComponent<LineAnimator>();
            animator.Initialize(lr, start, end, animationDuration);

            // Add to active lines
            activeLines.Add(new ConnectionLine
            {
                lineObject = lineObj,
                lineRenderer = lr,
                createdAt = Time.time,
                duration = lineDuration + animationDuration,
                isAnomaly = isAnomaly
            });
        }

        private void UpdateLines()
        {
            for (int i = activeLines.Count - 1; i >= 0; i--)
            {
                var line = activeLines[i];
                float age = Time.time - line.createdAt;

                if (age >= line.duration)
                {
                    // Fade out
                    float fadeOutTime = 0.5f;
                    float alpha = 1f - ((age - line.duration) / fadeOutTime);

                    if (alpha <= 0)
                    {
                        Destroy(line.lineObject);
                        activeLines.RemoveAt(i);
                    }
                    else
                    {
                        Color color = line.isAnomaly ? anomalyFlowColor : normalFlowColor;
                        color.a *= alpha;
                        line.lineRenderer.startColor = color;
                        line.lineRenderer.endColor = color;
                    }
                }
            }
        }

        public void ClearAllLines()
        {
            foreach (var line in activeLines)
            {
                if (line.lineObject != null)
                {
                    Destroy(line.lineObject);
                }
            }
            activeLines.Clear();
        }

        private class ConnectionLine
        {
            public GameObject lineObject;
            public LineRenderer lineRenderer;
            public float createdAt;
            public float duration;
            public bool isAnomaly;
        }
    }

    /// <summary>
    /// Animates a line renderer from start to end position
    /// </summary>
    public class LineAnimator : MonoBehaviour
    {
        private LineRenderer lr;
        private Vector3 startPos;
        private Vector3 endPos;
        private float duration;
        private float startTime;

        public void Initialize(LineRenderer lineRenderer, Vector3 start, Vector3 end, float animDuration)
        {
            lr = lineRenderer;
            startPos = start;
            endPos = end;
            duration = animDuration;
            startTime = Time.time;
        }

        private void Update()
        {
            if (lr == null) return;

            float elapsed = Time.time - startTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            Vector3 currentEnd = Vector3.Lerp(startPos, endPos, progress);
            lr.SetPosition(1, currentEnd);

            if (progress >= 1f)
            {
                Destroy(this);
            }
        }
    }
}
