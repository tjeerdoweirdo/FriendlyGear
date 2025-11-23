using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// ScriptableObject defining an agent's characteristics and base stats.
    /// </summary>
    [CreateAssetMenu(fileName = "AgentDefinition", menuName = "FriendlyGear/Agent Definition")]
    public class AgentDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string agentId;
        public string displayName;
        public Sprite portrait;

        [Header("Voice Lines")]
        [Tooltip("Array of voice clips for agent banter")]
        public AudioClip[] banterLines;

        [Header("Stats")]
        public AgentStats baseStats;

        [Header("Abilities")]
        [Tooltip("Special abilities this agent has")]
        public AgentAbility[] abilities;
    }
}
