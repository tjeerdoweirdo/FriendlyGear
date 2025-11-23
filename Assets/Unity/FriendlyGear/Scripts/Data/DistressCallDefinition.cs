using System;
using UnityEngine;

namespace FriendlyGear.Data
{
    [CreateAssetMenu(fileName = "DistressCall", menuName = "FriendlyGear/Distress Call", order = 20)]
    public class DistressCallDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string callId;
        public string title;
        [TextArea]
        public string description;

        [Header("Resolution Weights (0..1)")]
        public float fightingWeight = 0.2f;
        public float defenseWeight = 0.2f;
        public float mobilityWeight = 0.2f;
        public float luckWeight = 0.1f;
        public float intelligenceWeight = 0.15f;
        public float charismaWeight = 0.1f;
        public float mentalStabilityWeight = 0.05f;

        [Header("Mission Properties")]
        public int recommendedAgents = 1;
        public float baseDifficulty = 5f;
        public float minDurationSeconds = 10f;
        public float maxDurationSeconds = 30f;

        [Header("Audio (Optional)")]
        public AudioClip[] distressLines;
    }
}
