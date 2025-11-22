using UnityEngine;
using UnityEngine.AI;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Generic component that follows a WaypointPath using NavMeshAgent.
    /// Can be used by agents, enemies, or any other entity that needs to follow waypoints.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class WaypointFollower : MonoBehaviour
    {
        [Header("Path Configuration")]
        [Tooltip("The waypoint path to follow")]
        public WaypointPath waypointPath;

        [Header("Settings")]
        [Tooltip("Distance threshold to consider a waypoint reached")]
        [Range(0.1f, 5f)]
        public float arrivalThreshold = 1f;

        [Tooltip("Start following the path automatically on Start")]
        public bool autoStart = true;

        [Tooltip("Loop back to the beginning when reaching the end")]
        public bool loop = false;

        [Header("State")]
        [Tooltip("Current waypoint index")]
        public int currentWaypointIndex = 0;

        [Tooltip("Is the follower currently active")]
        public bool isFollowing = false;

        private NavMeshAgent navMeshAgent;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            if (autoStart && waypointPath != null && waypointPath.WaypointCount > 0)
            {
                StartFollowing();
            }
        }

        private void Update()
        {
            if (!isFollowing || waypointPath == null || navMeshAgent == null)
                return;

            // Check if we've reached the current waypoint
            if (HasReachedCurrentWaypoint())
            {
                AdvanceToNextWaypoint();
            }
        }

        /// <summary>
        /// Start following the waypoint path from the beginning.
        /// </summary>
        public void StartFollowing()
        {
            if (waypointPath == null || waypointPath.WaypointCount == 0)
            {
                Debug.LogWarning($"{gameObject.name}: Cannot start following - no waypoint path assigned or path is empty");
                return;
            }

            currentWaypointIndex = 0;
            isFollowing = true;
            MoveToCurrentWaypoint();
        }

        /// <summary>
        /// Start following the path from a specific waypoint index.
        /// </summary>
        public void StartFollowingFrom(int waypointIndex)
        {
            if (waypointPath == null || waypointPath.WaypointCount == 0)
            {
                Debug.LogWarning($"{gameObject.name}: Cannot start following - no waypoint path assigned or path is empty");
                return;
            }

            currentWaypointIndex = Mathf.Clamp(waypointIndex, 0, waypointPath.WaypointCount - 1);
            isFollowing = true;
            MoveToCurrentWaypoint();
        }

        /// <summary>
        /// Stop following the waypoint path.
        /// </summary>
        public void StopFollowing()
        {
            isFollowing = false;
            
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.ResetPath();
            }
        }

        /// <summary>
        /// Move to the current waypoint using NavMeshAgent.
        /// </summary>
        private void MoveToCurrentWaypoint()
        {
            if (waypointPath == null || navMeshAgent == null)
                return;

            Vector3 targetPosition = waypointPath.GetWaypointPosition(currentWaypointIndex);
            
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(targetPosition);
            }
        }

        /// <summary>
        /// Check if we've reached the current waypoint.
        /// </summary>
        private bool HasReachedCurrentWaypoint()
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
                return false;

            // Check if we're within the arrival threshold
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= arrivalThreshold)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Advance to the next waypoint in the path.
        /// </summary>
        private void AdvanceToNextWaypoint()
        {
            if (waypointPath == null)
                return;

            currentWaypointIndex++;

            // Check if we've reached the end of the path
            if (currentWaypointIndex >= waypointPath.WaypointCount)
            {
                if (loop)
                {
                    // Loop back to the start
                    currentWaypointIndex = 0;
                    MoveToCurrentWaypoint();
                }
                else
                {
                    // Stop at the end
                    isFollowing = false;
                    Debug.Log($"{gameObject.name}: Reached end of waypoint path");
                }
            }
            else
            {
                // Move to the next waypoint
                MoveToCurrentWaypoint();
            }
        }

        /// <summary>
        /// Set a new waypoint path and optionally start following it.
        /// </summary>
        public void SetWaypointPath(WaypointPath newPath, bool startImmediately = true)
        {
            waypointPath = newPath;
            
            if (startImmediately && waypointPath != null && waypointPath.WaypointCount > 0)
            {
                StartFollowing();
            }
        }

        /// <summary>
        /// Get the current progress through the path (0 to 1).
        /// </summary>
        public float GetPathProgress()
        {
            if (waypointPath == null || waypointPath.WaypointCount == 0)
                return 0f;

            return (float)currentWaypointIndex / waypointPath.WaypointCount;
        }
    }
}
