using System;
using UnityEngine;

namespace FriendlyGear.Data
{
    [CreateAssetMenu(fileName = "Ability", menuName = "FriendlyGear/Ability", order = 10)]
    public class AbilityDefinition : ScriptableObject
    {
        [Header("Info")]
        public string abilityId;
        public string displayName;
        [TextArea]
        public string description;

        [Header("Timing")]
        public float durationSeconds = 10f;
        public float cooldownSeconds = 20f;
        public bool isPassive;

        [Header("Stat Modifiers (Flat)")]
        public float fightingMod;
        public float defenseMod;
        public float mobilityMod;
        public float luckMod;
        public float intelligenceMod;
        public float charismaMod;
        public float mentalStabilityMod;

        [Header("Stat Modifiers (Percent, 1 = +100%)")]
        public float fightingPct;
        public float defensePct;
        public float mobilityPct;
        public float luckPct;
        public float intelligencePct;
        public float charismaPct;
        public float mentalStabilityPct;
    }
}
