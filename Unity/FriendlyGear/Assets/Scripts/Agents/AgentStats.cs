using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Serializable struct containing agent stats.
    /// </summary>
    [System.Serializable]
    public struct AgentStats
    {
        [Range(0, 100)] public float fighting;
        [Range(0, 100)] public float defense;
        [Range(0, 100)] public float mobility;
        [Range(0, 100)] public float luck;
        [Range(0, 100)] public float intelligence;
        [Range(0, 100)] public float charisma;
        [Range(0, 100)] public float mentalStability;

        public AgentStats(float fighting, float defense, float mobility, float luck, 
                         float intelligence, float charisma, float mentalStability)
        {
            this.fighting = fighting;
            this.defense = defense;
            this.mobility = mobility;
            this.luck = luck;
            this.intelligence = intelligence;
            this.charisma = charisma;
            this.mentalStability = mentalStability;
        }

        /// <summary>
        /// Add two AgentStats together.
        /// </summary>
        public static AgentStats operator +(AgentStats a, AgentStats b)
        {
            return new AgentStats(
                a.fighting + b.fighting,
                a.defense + b.defense,
                a.mobility + b.mobility,
                a.luck + b.luck,
                a.intelligence + b.intelligence,
                a.charisma + b.charisma,
                a.mentalStability + b.mentalStability
            );
        }

        /// <summary>
        /// Subtract one AgentStats from another.
        /// </summary>
        public static AgentStats operator -(AgentStats a, AgentStats b)
        {
            return new AgentStats(
                a.fighting - b.fighting,
                a.defense - b.defense,
                a.mobility - b.mobility,
                a.luck - b.luck,
                a.intelligence - b.intelligence,
                a.charisma - b.charisma,
                a.mentalStability - b.mentalStability
            );
        }

        /// <summary>
        /// Returns a scaled version of these stats.
        /// </summary>
        public AgentStats Scaled(float factor)
        {
            return new AgentStats(
                fighting * factor,
                defense * factor,
                mobility * factor,
                luck * factor,
                intelligence * factor,
                charisma * factor,
                mentalStability * factor
            );
        }

        /// <summary>
        /// Returns the sum of all stat values.
        /// </summary>
        public float GetTotal()
        {
            return fighting + defense + mobility + luck + intelligence + charisma + mentalStability;
        }

        /// <summary>
        /// Returns a clamped version of these stats (all values between 0 and 100).
        /// </summary>
        public AgentStats Clamped()
        {
            return new AgentStats(
                Mathf.Clamp(fighting, 0, 100),
                Mathf.Clamp(defense, 0, 100),
                Mathf.Clamp(mobility, 0, 100),
                Mathf.Clamp(luck, 0, 100),
                Mathf.Clamp(intelligence, 0, 100),
                Mathf.Clamp(charisma, 0, 100),
                Mathf.Clamp(mentalStability, 0, 100)
            );
        }
    }
}
