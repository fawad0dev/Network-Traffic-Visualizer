using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NetworkTrafficVisualizer.Core
{
    using Data;

    /// <summary>
    /// Manages the flow of network packets and their lifecycle
    /// </summary>
    public class PacketFlowManager : MonoBehaviour
    {
        [Header("Flow Settings")]
        [SerializeField] private float packetSpeed = 5f;
        [SerializeField] private int maxActivePackets = 100;
        [SerializeField] private float packetLifetime = 10f;

        private List<PacketFlow> activePacketFlows = new List<PacketFlow>();
        private Queue<NetworkPacket> packetQueue = new Queue<NetworkPacket>();

        public delegate void PacketReceivedHandler(NetworkPacket packet);
        public event PacketReceivedHandler OnPacketReceived;

        private void Update()
        {
            UpdatePacketFlows();
            ProcessPacketQueue();
        }

        /// <summary>
        /// Add a packet to be visualized
        /// </summary>
        public void AddPacket(NetworkPacket packet, Vector3 sourcePos, Vector3 destPos, GameObject visualObject)
        {
            if (activePacketFlows.Count < maxActivePackets)
            {
                var flow = new PacketFlow(packet, sourcePos, destPos, visualObject, packetSpeed, packetLifetime);
                activePacketFlows.Add(flow);
            }
            else
            {
                packetQueue.Enqueue(packet);
            }
        }

        private void UpdatePacketFlows()
        {
            for (int i = activePacketFlows.Count - 1; i >= 0; i--)
            {
                var flow = activePacketFlows[i];
                flow.Update(Time.deltaTime);

                if (flow.IsComplete)
                {
                    OnPacketReceived?.Invoke(flow.packet);
                    if (flow.visualObject != null)
                    {
                        Destroy(flow.visualObject);
                    }
                    activePacketFlows.RemoveAt(i);
                }
            }
        }

        private void ProcessPacketQueue()
        {
            while (packetQueue.Count > 0 && activePacketFlows.Count < maxActivePackets)
            {
                // Would need to recreate visualization for queued packets
                packetQueue.Dequeue();
            }
        }

        public int GetActivePacketCount()
        {
            return activePacketFlows.Count;
        }

        /// <summary>
        /// Internal class to track packet flow progress
        /// </summary>
        private class PacketFlow
        {
            public NetworkPacket packet;
            public Vector3 sourcePosition;
            public Vector3 destinationPosition;
            public GameObject visualObject;
            public float speed;
            public float lifetime;
            public float progress;
            public float age;

            public bool IsComplete => progress >= 1f || age >= lifetime;

            public PacketFlow(NetworkPacket packet, Vector3 source, Vector3 dest, GameObject visual, float speed, float lifetime)
            {
                this.packet = packet;
                this.sourcePosition = source;
                this.destinationPosition = dest;
                this.visualObject = visual;
                this.speed = speed;
                this.lifetime = lifetime;
                this.progress = 0f;
                this.age = 0f;
            }

            public void Update(float deltaTime)
            {
                age += deltaTime;
                float distance = Vector3.Distance(sourcePosition, destinationPosition);
                progress += (speed * deltaTime) / Mathf.Max(distance, 0.1f);
                progress = Mathf.Clamp01(progress);

                if (visualObject != null)
                {
                    visualObject.transform.position = Vector3.Lerp(sourcePosition, destinationPosition, progress);
                }
            }
        }
    }
}
