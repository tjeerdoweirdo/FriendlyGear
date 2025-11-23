using System.Collections;
using System.Collections.Generic;
using FriendlyGear.Data;
using UnityEngine;
using UnityEngine.AI;

namespace FriendlyGear.Runtime
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        public AgentDefinition definition;
        public StatBlock stats = new StatBlock();
        public AudioSource audioSource;

        NavMeshAgent nav;
        readonly List<AbilityDefinition> activeAbilities = new List<AbilityDefinition>();
        readonly Dictionary<AbilityDefinition, float> abilityEndTimes = new Dictionary<AbilityDefinition, float>();

        public bool IsAvailable => !nav.pathPending && nav.remainingDistance <= nav.stoppingDistance && (!nav.hasPath || nav.velocity.sqrMagnitude == 0f);

        void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            if (!audioSource) audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            if (definition != null)
            {
                stats.FromAgent(definition);
                nav.speed = Mathf.Max(0.1f, definition.baseMoveSpeed + stats.mobility * 0.1f);
            }
        }

        void Update()
        {
            if (activeAbilities.Count > 0)
            {
                float now = Time.time;
                for (int i = activeAbilities.Count - 1; i >= 0; i--)
                {
                    var ab = activeAbilities[i];
                    if (abilityEndTimes.TryGetValue(ab, out float end) && now >= end)
                    {
                        activeAbilities.RemoveAt(i);
                        abilityEndTimes.Remove(ab);
                        RecalculateStats();
                    }
                }
            }
        }

        public void SetDestination(Vector3 worldPosition)
        {
            nav.SetDestination(worldPosition);
        }

        public void FollowPath(IList<Transform> waypoints)
        {
            StartCoroutine(FollowPathRoutine(waypoints));
        }

        IEnumerator FollowPathRoutine(IList<Transform> waypoints)
        {
            foreach (var wp in waypoints)
            {
                if (wp == null) continue;
                nav.SetDestination(wp.position);
                yield return new WaitUntil(() => IsAvailable);
            }
        }

        public void ActivateAbility(AbilityDefinition ability)
        {
            if (ability == null) return;
            if (!activeAbilities.Contains(ability))
            {
                activeAbilities.Add(ability);
            }
            abilityEndTimes[ability] = Time.time + Mathf.Max(0.01f, ability.durationSeconds);
            RecalculateStats();
        }

        void RecalculateStats()
        {
            if (definition == null) return;
            stats.FromAgent(definition);
            foreach (var ab in activeAbilities)
            {
                stats.ApplyFlat(ab);
            }
            foreach (var ab in activeAbilities)
            {
                stats.ApplyPercent(ab);
            }
            nav.speed = Mathf.Max(0.1f, definition.baseMoveSpeed + stats.mobility * 0.1f);
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            if (!clip) return;
            if (!audioSource) return;
            audioSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }
}
