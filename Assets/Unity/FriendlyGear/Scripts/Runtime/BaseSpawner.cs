using System.Collections.Generic;
using FriendlyGear.Data;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class BaseSpawner : MonoBehaviour
    {
        public List<AgentDefinition> startingAgents = new List<AgentDefinition>();
        public Transform spawnPoint;
        public List<AgentController> availableAgents = new List<AgentController>();

        void Start()
        {
            foreach (var def in startingAgents)
            {
                SpawnAgent(def);
            }
        }

        public AgentController SpawnAgent(AgentDefinition def)
        {
            if (def == null || def.agentPrefab == null) return null;
            var pos = spawnPoint ? spawnPoint.position : transform.position;
            var rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
            var go = Instantiate(def.agentPrefab, pos, rot);
            var controller = go.GetComponent<AgentController>();
            if (!controller) controller = go.AddComponent<AgentController>();
            controller.definition = def;
            availableAgents.Add(controller);
            return controller;
        }

        public bool TryGetAvailable(out AgentController agent)
        {
            agent = null;
            for (int i = 0; i < availableAgents.Count; i++)
            {
                var a = availableAgents[i];
                if (a && a.IsAvailable)
                {
                    agent = a;
                    return true;
                }
            }
            return false;
        }
    }
}
