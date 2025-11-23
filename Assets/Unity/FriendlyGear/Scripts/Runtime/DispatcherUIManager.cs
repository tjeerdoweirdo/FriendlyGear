using System.Collections.Generic;
using FriendlyGear.Data;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class DispatcherUIManager : MonoBehaviour
    {
        public MissionManager missionManager;
        public BaseSpawner baseSpawner;

        readonly List<AgentController> selectedAgents = new List<AgentController>();

        void Awake()
        {
            if (!missionManager) missionManager = FindObjectOfType<MissionManager>();
            if (!baseSpawner) baseSpawner = FindObjectOfType<BaseSpawner>();
        }

        public void SelectAgent(AgentController agent)
        {
            if (agent != null && !selectedAgents.Contains(agent)) selectedAgents.Add(agent);
        }

        public void DeselectAgent(AgentController agent)
        {
            if (agent != null) selectedAgents.Remove(agent);
        }

        public void ClearSelection()
        {
            selectedAgents.Clear();
        }

        public DistressCallInstance CreateDistress(DistressCallDefinition def, Vector3 position)
        {
            return missionManager ? missionManager.CreateDistress(def, position) : null;
        }

        public void DispatchSelectedTo(DistressCallInstance call)
        {
            if (!missionManager || call == null || selectedAgents.Count == 0) return;
            missionManager.DispatchAgentsToCall(call, selectedAgents);
            selectedAgents.Clear();
        }

        public bool TrySelectAnyAvailable()
        {
            if (!baseSpawner) return false;
            if (baseSpawner.TryGetAvailable(out var agent))
            {
                SelectAgent(agent);
                return true;
            }
            return false;
        }
    }
}
