using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Defines a path of waypoints that can be followed by agents or other entities.
    /// </summary>
    public class WaypointPath : MonoBehaviour
    {
        [Header("Waypoints")]
        [Tooltip("Ordered array of waypoint transforms")]
        public Transform[] waypoints;

        [Header("Settings")]
        [Tooltip("If true, the path loops back to the start")]
        public bool loop = false;

        /// <summary>
        /// Get the waypoint at the specified index.
        /// </summary>
        public Transform GetWaypoint(int index)
        {
            if (waypoints == null || waypoints.Length == 0)
                return null;

            index = Mathf.Clamp(index, 0, waypoints.Length - 1);
            return waypoints[index];
        }

        /// <summary>
        /// Get the position of the waypoint at the specified index.
        /// </summary>
        public Vector3 GetWaypointPosition(int index)
        {
            Transform waypoint = GetWaypoint(index);
            return waypoint != null ? waypoint.position : Vector3.zero;
        }

        /// <summary>
        /// Get all waypoint positions as a Vector3 array.
        /// </summary>
        public Vector3[] GetAllWaypointPositions()
        {
            if (waypoints == null || waypoints.Length == 0)
                return new Vector3[0];

            Vector3[] positions = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                positions[i] = waypoints[i] != null ? waypoints[i].position : Vector3.zero;
            }

            return positions;
        }

        /// <summary>
        /// Get the total number of waypoints in this path.
        /// </summary>
        public int WaypointCount
        {
            get { return waypoints != null ? waypoints.Length : 0; }
        }

        /// <summary>
        /// Draw gizmos to visualize the waypoint path in the editor.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length == 0)
                return;

            Gizmos.color = Color.yellow;

            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null)
                    continue;

                // Draw sphere at waypoint
                Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);

                // Draw line to next waypoint
                if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else if (loop && waypoints.Length > 1 && waypoints[0] != null)
                {
                    // Draw line back to start if looping
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }
}
