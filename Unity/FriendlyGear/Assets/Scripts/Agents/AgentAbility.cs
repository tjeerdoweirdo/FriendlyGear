using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Abstract base class for agent abilities that can modify behaviour during missions.
    /// </summary>
    public abstract class AgentAbility : ScriptableObject
    {
        [TextArea(2, 5)]
        public string description;

        /// <summary>
        /// Called when the agent starts a mission.
        /// </summary>
        public virtual void OnMissionStart(AgentController agent, Mission mission)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called every frame while the agent is on a mission.
        /// Optional - can be left empty by default.
        /// </summary>
        public virtual void OnMissionTick(AgentController agent, Mission mission, float deltaTime)
        {
            // Override in derived classes if needed
        }

        /// <summary>
        /// Called just before the mission is resolved.
        /// </summary>
        public virtual void OnBeforeResolve(AgentController agent, Mission mission)
        {
            // Override in derived classes
        }
    }
}
