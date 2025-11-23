using System;
using System.Collections.Generic;
using UnityEngine;

namespace FriendlyGear.Data
{
    [CreateAssetMenu(fileName = "Agent", menuName = "FriendlyGear/Agent", order = 5)]
    public class AgentDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string agentId;
        public string displayName;
        public Sprite portrait;
        public GameObject agentPrefab;

        [Header("Base Stats")]
        public float fighting = 5f;
        public float defense = 5f;
        public float mobility = 5f;
        public float luck = 5f;
        public float intelligence = 5f;
        public float charisma = 5f;
        public float mentalStability = 5f;

        [Header("Abilities")]
        public List<AbilityDefinition> abilities = new List<AbilityDefinition>();

        [Header("Audio (Optional)")]
        public AudioClip[] banterLines;
        public AudioClip[] dispatchAcks;
        public AudioClip[] successLines;
        public AudioClip[] failureLines;

        [Header("Movement")]
        public float baseMoveSpeed = 3.5f;
    }
}
