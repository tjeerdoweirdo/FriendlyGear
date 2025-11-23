using UnityEngine;
using UnityEngine.AI;

namespace FriendlyGear.Core
{
    /// <summary>
    /// MonoBehaviour controlling an agent's movement and mission behavior.
    /// Requires a NavMeshAgent component for pathfinding.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        // Velocity threshold for determining if agent has stopped moving (squared for sqrMagnitude comparison)
        private const float VELOCITY_STOP_THRESHOLD_SQR = 0.01f; // 0.1f * 0.1f

        [Header("Agent Data")]
        public AgentDefinition definition;

        [Header("Runtime Stats")]
        [Tooltip("Current stats (modified by abilities and effects)")]
        public AgentStats currentStats;

        [Header("Mission")]
        [Tooltip("The mission this agent is currently assigned to")]
        public Mission currentMission;

        private NavMeshAgent navMeshAgent;
        private Vector3[] waypointPath;
        private int currentWaypointIndex = 0;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            if (definition != null)
            {
                currentStats = definition.baseStats;
            }
        }

        /// <summary>
        /// Assign a mission to this agent and start moving toward the target.
        /// </summary>
        public void AssignMission(Mission mission)
        {
            currentMission = mission;

            if (mission != null && mission.Target != null)
            {
                // Trigger abilities
                if (definition != null && definition.abilities != null)
                {
                    foreach (var ability in definition.abilities)
                    {
                        if (ability != null)
                        {
                            ability.OnMissionStart(this, mission);
                        }
                    }
                }

                // Set NavMesh destination to the mission target
                SetDestination(mission.Target.position);
            }
        }

        /// <summary>
        /// Set the NavMeshAgent's destination.
        /// </summary>
        public void SetDestination(Vector3 destination)
        {
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(destination);
            }
        }

        /// <summary>
        /// Follow a series of waypoints in order.
        /// </summary>
        public void SetWaypointPath(Vector3[] waypoints)
        {
            waypointPath = waypoints;
            currentWaypointIndex = 0;
            
            if (waypoints != null && waypoints.Length > 0)
            {
                SetDestination(waypoints[0]);
            }
        }

        /// <summary>
        /// Apply a stat modifier (positive or negative) to the agent's current stats.
        /// </summary>
        public void ApplyStatModifier(AgentStats delta)
        {
            currentStats = (currentStats + delta).Clamped();
        }

        /// <summary>
        /// Check if the agent has reached its current NavMesh destination.
        /// </summary>
        public bool HasReachedDestination()
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
                return false;

            // Check if we've reached the destination
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < VELOCITY_STOP_THRESHOLD_SQR)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Reset the agent's stats to base stats.
        /// </summary>
        public void ResetStats()
        {
            if (definition != null)
            {
                currentStats = definition.baseStats;
            }
        }

        /// <summary>
        /// Clear the current mission assignment.
        /// </summary>
        public void ClearMission()
        {
            currentMission = null;
        }

        private void Update()
        {
            // Handle waypoint progression
            if (waypointPath != null && waypointPath.Length > 0 && currentWaypointIndex < waypointPath.Length)
            {
                if (HasReachedDestination())
                {
                    currentWaypointIndex++;
                    if (currentWaypointIndex < waypointPath.Length)
                    {
                        SetDestination(waypointPath[currentWaypointIndex]);
                    }
                }
            }

            // Trigger ability tick
            if (currentMission != null && definition != null && definition.abilities != null)
            {
                foreach (var ability in definition.abilities)
                {
                    if (ability != null)
                    {
                        ability.OnMissionTick(this, currentMission, Time.deltaTime);
                    }
                }
            }
        }
    }
}
