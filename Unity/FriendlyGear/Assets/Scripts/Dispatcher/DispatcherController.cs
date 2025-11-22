using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Main controller for the dispatcher, managing agents, missions, and distress calls.
    /// </summary>
    public class DispatcherController : MonoBehaviour
    {
        [Header("Base Configuration")]
        [Tooltip("The home base location where agents return after missions")]
        public Transform baseLocation;

        [Header("Agents")]
        [Tooltip("List of agents currently available at base")]
        public List<AgentController> availableAgents = new List<AgentController>();

        [Header("Missions")]
        [Tooltip("List of currently active missions")]
        public List<Mission> activeMissions = new List<Mission>();

        [Header("Voice Settings")]
        [Tooltip("Voice lines the dispatcher can say")]
        public AudioClip[] dispatcherLines;

        private void Update()
        {
            // Check all active missions to see if agents have reached their targets
            for (int i = activeMissions.Count - 1; i >= 0; i--)
            {
                Mission mission = activeMissions[i];
                
                if (mission != null && mission.outcome == MissionOutcome.Pending)
                {
                    if (mission.HaveAgentsReachedTarget())
                    {
                        OnMissionReachedTarget(mission);
                    }
                }
            }
        }

        /// <summary>
        /// Dispatch selected agents to handle a distress call.
        /// Creates a mission and assigns agents to it.
        /// </summary>
        public Mission DispatchAgentsToCall(DistressCallInstance call, List<AgentController> selectedAgents)
        {
            if (call == null || selectedAgents == null || selectedAgents.Count == 0)
            {
                Debug.LogWarning("Cannot dispatch: invalid call or no agents selected");
                return null;
            }

            // Create the mission
            Mission mission = new Mission(call, selectedAgents);

            // Remove agents from available pool
            foreach (var agent in selectedAgents)
            {
                if (availableAgents.Contains(agent))
                {
                    availableAgents.Remove(agent);
                }

                // Assign mission to each agent (triggers movement and abilities)
                agent.AssignMission(mission);
            }

            // Add to active missions
            activeMissions.Add(mission);

            // Play voice lines
            PlayDispatchVoiceLines(call, selectedAgents);

            Debug.Log($"Dispatched {selectedAgents.Count} agent(s) to '{call.definition?.title}'");

            return mission;
        }

        /// <summary>
        /// Called when a mission's agents have reached the target location.
        /// Resolves the mission and handles post-resolution behaviour.
        /// </summary>
        public void OnMissionReachedTarget(Mission mission)
        {
            if (mission == null)
                return;

            Debug.Log($"Mission agents reached target. Resolving mission...");

            // Resolve the mission
            MissionOutcome outcome = mission.Resolve();

            // Mark the distress call as resolved if the mission succeeded
            if (outcome == MissionOutcome.Success || outcome == MissionOutcome.PartialSuccess)
            {
                if (mission.distressCall != null)
                {
                    mission.distressCall.ResolveCall();
                }
            }

            // Send agents back to base
            ReturnAgentsToBase(mission.assignedAgents);

            // Optional: Handle outcome-specific logic here
            HandleMissionOutcome(mission, outcome);
        }

        /// <summary>
        /// Return agents to the base location and add them back to available agents.
        /// </summary>
        private void ReturnAgentsToBase(List<AgentController> agents)
        {
            if (agents == null || baseLocation == null)
                return;

            foreach (var agent in agents)
            {
                if (agent != null)
                {
                    // Clear the mission
                    agent.currentMission = null;

                    // Reset stats to base
                    agent.ResetStats();

                    // Set destination back to base
                    agent.SetDestination(baseLocation.position);

                    // Add back to available agents if not already there
                    if (!availableAgents.Contains(agent))
                    {
                        availableAgents.Add(agent);
                    }
                }
            }
        }

        /// <summary>
        /// Handle outcome-specific logic (rewards, penalties, etc.)
        /// Override or extend this method for custom behavior.
        /// </summary>
        private void HandleMissionOutcome(Mission mission, MissionOutcome outcome)
        {
            switch (outcome)
            {
                case MissionOutcome.Success:
                    Debug.Log($"Mission SUCCESS! All objectives completed.");
                    break;
                case MissionOutcome.PartialSuccess:
                    Debug.Log($"Mission PARTIAL SUCCESS. Some objectives completed.");
                    break;
                case MissionOutcome.Failure:
                    Debug.LogWarning($"Mission FAILED. Objectives not met.");
                    break;
            }
        }

        /// <summary>
        /// Play voice lines from dispatcher, agents, and caller.
        /// </summary>
        private void PlayDispatchVoiceLines(DistressCallInstance call, List<AgentController> selectedAgents)
        {
            AudioManager audioManager = AudioManager.Instance;
            if (audioManager == null)
                return;

            // Play dispatcher line
            if (dispatcherLines != null && dispatcherLines.Length > 0)
            {
                audioManager.PlayRandomLine(dispatcherLines);
            }

            // Play each selected agent's banter line
            foreach (var agent in selectedAgents)
            {
                if (agent != null && agent.definition != null && agent.definition.banterLines != null)
                {
                    audioManager.PlayRandomLine(agent.definition.banterLines);
                }
            }

            // Play caller line
            if (call != null && call.definition != null && call.definition.callerLines != null)
            {
                audioManager.PlayRandomLine(call.definition.callerLines);
            }
        }

        /// <summary>
        /// Manually add an agent to the available pool.
        /// </summary>
        public void AddAvailableAgent(AgentController agent)
        {
            if (agent != null && !availableAgents.Contains(agent))
            {
                availableAgents.Add(agent);
            }
        }

        /// <summary>
        /// Manually remove an agent from the available pool.
        /// </summary>
        public void RemoveAvailableAgent(AgentController agent)
        {
            if (agent != null && availableAgents.Contains(agent))
            {
                availableAgents.Remove(agent);
            }
        }

        /// <summary>
        /// Get a list of all active distress calls in the scene.
        /// </summary>
        public List<DistressCallInstance> GetActiveDistressCalls()
        {
            DistressCallInstance[] allCalls = FindObjectsOfType<DistressCallInstance>();
            return allCalls.Where(call => call.isActive).ToList();
        }
    }
}
