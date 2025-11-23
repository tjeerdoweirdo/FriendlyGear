using System;
using FriendlyGear.Data;

namespace FriendlyGear.Runtime
{
    [Serializable]
    public class StatBlock
    {
        public float fighting;
        public float defense;
        public float mobility;
        public float luck;
        public float intelligence;
        public float charisma;
        public float mentalStability;

        public float Get(StatType type)
        {
            switch (type)
            {
                case StatType.Fighting: return fighting;
                case StatType.Defense: return defense;
                case StatType.Mobility: return mobility;
                case StatType.Luck: return luck;
                case StatType.Intelligence: return intelligence;
                case StatType.Charisma: return charisma;
                case StatType.MentalStability: return mentalStability;
                default: return 0f;
            }
        }

        public void FromAgent(FriendlyGear.Data.AgentDefinition def)
        {
            fighting = def.fighting;
            defense = def.defense;
            mobility = def.mobility;
            luck = def.luck;
            intelligence = def.intelligence;
            charisma = def.charisma;
            mentalStability = def.mentalStability;
        }

        public void ApplyFlat(FriendlyGear.Data.AbilityDefinition ability)
        {
            fighting += ability.fightingMod;
            defense += ability.defenseMod;
            mobility += ability.mobilityMod;
            luck += ability.luckMod;
            intelligence += ability.intelligenceMod;
            charisma += ability.charismaMod;
            mentalStability += ability.mentalStabilityMod;
        }

        public void ApplyPercent(FriendlyGear.Data.AbilityDefinition ability)
        {
            fighting += fighting * ability.fightingPct;
            defense += defense * ability.defensePct;
            mobility += mobility * ability.mobilityPct;
            luck += luck * ability.luckPct;
            intelligence += intelligence * ability.intelligencePct;
            charisma += charisma * ability.charismaPct;
            mentalStability += mentalStability * ability.mentalStabilityPct;
        }
    }
}
