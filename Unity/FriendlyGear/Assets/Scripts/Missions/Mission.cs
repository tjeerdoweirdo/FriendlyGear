using UnityEngine;
using System.Collections.Generic;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Represents a mission where agents are dispatched to handle a distress call.
    /// Non-MonoBehaviour class that manages the mission state and resolution.
    /// </summary>
    public class Mission
    {
        public DistressCallInstance distressCall;
        public List<AgentController> assignedAgents;
        public MissionOutcome outcome;

        /// <summary>
        /// Gets the target Transform of the distress call.
        /// </summary>
        public Transform Target
        {
            get
            {
                if (distressCall != null)
                    return distressCall.transform;
                return null;
            }
        }

        public Mission(DistressCallInstance call, List<AgentController> agents)
        {
            distressCall = call;
            assignedAgents = new List<AgentController>(agents);
            outcome = MissionOutcome.Pending;
        }

        /// <summary>
        /// Calculate the success chance based on combined agent stats vs required stats.
        /// Returns a value between 0 and 1.
        /// </summary>
        public float CalculateSuccessChance()
        {
            if (distressCall == null || distressCall.definition == null || assignedAgents == null || assignedAgents.Count == 0)
            {
                return 0f;
            }

            // Combine all agent stats
            AgentStats combinedStats = new AgentStats();
            foreach (var agent in assignedAgents)
            {
                if (agent != null)
                {
                    combinedStats = combinedStats + agent.currentStats;
                }
            }

            // Get required stats
            AgentStats requiredStats = distressCall.definition.requiredStats;
            float difficulty = distressCall.definition.baseDifficulty;

            // Calculate weighted comparison
            // Each stat contributes equally to the overall success chance
            float fightingRatio = CalculateStatRatio(combinedStats.fighting, requiredStats.fighting);
            float defenseRatio = CalculateStatRatio(combinedStats.defense, requiredStats.defense);
            float mobilityRatio = CalculateStatRatio(combinedStats.mobility, requiredStats.mobility);
            float luckRatio = CalculateStatRatio(combinedStats.luck, requiredStats.luck);
            float intelligenceRatio = CalculateStatRatio(combinedStats.intelligence, requiredStats.intelligence);
            float charismaRatio = CalculateStatRatio(combinedStats.charisma, requiredStats.charisma);
            float mentalStabilityRatio = CalculateStatRatio(combinedStats.mentalStability, requiredStats.mentalStability);

            // Average all ratios
            float averageRatio = (fightingRatio + defenseRatio + mobilityRatio + luckRatio + 
                                 intelligenceRatio + charismaRatio + mentalStabilityRatio) / 7f;

            // Apply difficulty modifier
            float successChance = averageRatio / difficulty;

            // Clamp to [0, 1]
            return Mathf.Clamp01(successChance);
        }

        /// <summary>
        /// Calculate the ratio between agent stat and required stat.
        /// Allows ratios above 1.0 so over-specced agents can compensate for difficulty or other stats.
        /// </summary>
        private float CalculateStatRatio(float agentStat, float requiredStat)
        {
            if (requiredStat <= 0f)
                return 1f; // If no requirement, consider it met

            return agentStat / requiredStat;
        }

        /// <summary>
        /// Resolve the mission by rolling against the success chance.
        /// Triggers abilities' OnBeforeResolve hooks.
        /// </summary>
        public MissionOutcome Resolve()
        {
            if (outcome != MissionOutcome.Pending)
            {
                Debug.LogWarning("Mission has already been resolved!");
                return outcome;
            }

            // Trigger OnBeforeResolve for all agent abilities
            foreach (var agent in assignedAgents)
            {
                if (agent != null && agent.definition != null && agent.definition.abilities != null)
                {
                    foreach (var ability in agent.definition.abilities)
                    {
                        if (ability != null)
                        {
                            ability.OnBeforeResolve(agent, this);
                        }
                    }
                }
            }

            // Calculate success chance
            float successChance = CalculateSuccessChance();

            // Roll for outcome
            float roll = Random.Range(0f, 1f);

            if (roll <= successChance)
            {
                // Success thresholds
                if (successChance >= 0.9f)
                {
                    outcome = MissionOutcome.Success;
                }
                else if (successChance >= 0.5f)
                {
                    outcome = MissionOutcome.PartialSuccess;
                }
                else
                {
                    // Low success chance but got lucky
                    outcome = MissionOutcome.PartialSuccess;
                }
            }
            else
            {
                // Failed the roll
                if (successChance >= 0.5f)
                {
                    // Had a decent chance but unlucky
                    outcome = MissionOutcome.PartialSuccess;
                }
                else
                {
                    outcome = MissionOutcome.Failure;
                }
            }

            Debug.Log($"Mission resolved! Success Chance: {successChance:P0}, Roll: {roll:F2}, Outcome: {outcome}");

            return outcome;
        }

        /// <summary>
        /// Check if all assigned agents have reached the target location.
        /// </summary>
        public bool HaveAgentsReachedTarget()
        {
            if (assignedAgents == null || assignedAgents.Count == 0)
                return false;

            foreach (var agent in assignedAgents)
            {
                if (agent != null && !agent.HasReachedDestination())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
