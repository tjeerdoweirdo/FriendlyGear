using System.Collections.Generic;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class WaypointPath : MonoBehaviour
    {
        public List<Transform> points = new List<Transform>();

        public IList<Transform> GetPoints() => points;

        public Transform GetStart() => points.Count > 0 ? points[0] : null;
    }
}
