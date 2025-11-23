using UnityEngine;

namespace FriendlyGear.Runtime
{
    [AddComponentMenu("FriendlyGear/Agent")]
    [DisallowMultipleComponent]
    public class Agent : AgentController
    {
        // Inherits all behavior from AgentController (NavMesh, stats, abilities).
        // Add Agent-specific hooks or visuals here if needed later.
    }
}
