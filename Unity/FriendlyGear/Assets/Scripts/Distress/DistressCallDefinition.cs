using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// ScriptableObject defining a distress call's properties and requirements.
    /// </summary>
    [CreateAssetMenu(fileName = "DistressCallDefinition", menuName = "FriendlyGear/Distress Call Definition")]
    public class DistressCallDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string callId;
        public string title;
        
        [TextArea(3, 6)]
        public string description;

        [Header("Voice Lines")]
        [Tooltip("Array of voice clips from the caller")]
        public AudioClip[] callerLines;

        [Header("Requirements")]
        [Tooltip("Required stats profile for success")]
        public AgentStats requiredStats;

        [Header("Tuning")]
        [Tooltip("Base difficulty multiplier")]
        [Range(0.1f, 5f)]
        public float baseDifficulty = 1f;

        [Tooltip("Time limit in seconds before the call expires")]
        public float timeLimitSeconds = 300f;
    }
}
