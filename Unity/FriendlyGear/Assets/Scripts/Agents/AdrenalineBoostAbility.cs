using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Example ability that boosts an agent's fighting and mobility stats when a mission starts.
    /// </summary>
    [CreateAssetMenu(fileName = "AdrenalineBoostAbility", menuName = "FriendlyGear/Abilities/Adrenaline Boost")]
    public class AdrenalineBoostAbility : AgentAbility
    {
        [Header("Stat Boosts")]
        [Range(0, 50)]
        public float fightingBoost = 10f;
        
        [Range(0, 50)]
        public float mobilityBoost = 15f;

        public override void OnMissionStart(AgentController agent, Mission mission)
        {
            base.OnMissionStart(agent, mission);

            // Create a stat modifier with the boosts
            AgentStats boost = new AgentStats(
                fighting: fightingBoost,
                defense: 0,
                mobility: mobilityBoost,
                luck: 0,
                intelligence: 0,
                charisma: 0,
                mentalStability: 0
            );

            // Apply the boost to the agent
            agent.ApplyStatModifier(boost);
            
            Debug.Log($"{agent.definition.displayName} activated Adrenaline Boost! " +
                     $"Fighting +{fightingBoost}, Mobility +{mobilityBoost}");
        }
    }
}
