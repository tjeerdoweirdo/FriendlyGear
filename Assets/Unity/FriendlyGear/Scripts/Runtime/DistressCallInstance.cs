using System.Collections.Generic;
using FriendlyGear.Data;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class DistressCallInstance
    {
        public DistressCallDefinition definition;
        public Vector3 worldPosition;
        public float createdTime;
        public float resolveAtTime;
        public readonly List<AgentController> assignedAgents = new List<AgentController>();
        public bool IsResolved { get; private set; }
        public bool? Success { get; private set; }

        public DistressCallInstance(DistressCallDefinition def, Vector3 pos, float durationSeconds)
        {
            definition = def;
            worldPosition = pos;
            createdTime = Time.time;
            resolveAtTime = createdTime + Mathf.Max(1f, durationSeconds);
        }

        public void AssignAgent(AgentController agent)
        {
            if (agent != null && !assignedAgents.Contains(agent))
            {
                assignedAgents.Add(agent);
            }
        }

        public void Resolve(bool success)
        {
            if (IsResolved) return;
            Success = success;
            IsResolved = true;
        }
    }
}
