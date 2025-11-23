using System;
using System.Collections.Generic;
using FriendlyGear.Data;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class MissionManager : MonoBehaviour
    {
        public event Action<DistressCallInstance> OnDistressCreated;
        public event Action<DistressCallInstance, bool> OnDistressResolved;

        public float travelTimePerMeter = 0.2f;
        public AnimationCurve successCurve = AnimationCurve.Linear(0, 0, 1, 1);

        readonly List<DistressCallInstance> activeCalls = new List<DistressCallInstance>();

        void Update()
        {
            for (int i = activeCalls.Count - 1; i >= 0; i--)
            {
                var call = activeCalls[i];
                if (!call.IsResolved && Time.time >= call.resolveAtTime)
                {
                    bool success = RollSuccess(call);
                    call.Resolve(success);
                    OnDistressResolved?.Invoke(call, success);
                    activeCalls.RemoveAt(i);
                }
            }
        }

        public DistressCallInstance CreateDistress(DistressCallDefinition def, Vector3 position)
        {
            float duration = UnityEngine.Random.Range(def.minDurationSeconds, def.maxDurationSeconds);
            var call = new DistressCallInstance(def, position, duration);
            activeCalls.Add(call);
            OnDistressCreated?.Invoke(call);
            return call;
        }

        public void DispatchAgentsToCall(DistressCallInstance call, IList<AgentController> agents)
        {
            if (call == null || agents == null) return;
            foreach (var agent in agents)
            {
                call.AssignAgent(agent);
                agent.SetDestination(call.worldPosition);
            }
            float longestTravel = 0f;
            foreach (var agent in agents)
            {
                float mobility = Mathf.Max(0.1f, agent.stats.mobility);
                float distance = Vector3.Distance(agent.transform.position, call.worldPosition);
                float t = distance / (mobility + 1f);
                longestTravel = Mathf.Max(longestTravel, t);
            }
            call.resolveAtTime = Mathf.Max(call.resolveAtTime, Time.time + longestTravel);
        }

        bool RollSuccess(DistressCallInstance call)
        {
            if (call.assignedAgents.Count == 0) return false;
            float groupScore = 0f;
            foreach (var agent in call.assignedAgents)
            {
                groupScore += ScoreAgentAgainstCall(agent, call.definition);
            }
            float avgScore = groupScore / call.assignedAgents.Count;
            float luckBoost = 0f;
            foreach (var agent in call.assignedAgents)
            {
                luckBoost += agent.stats.luck * 0.02f;
            }
            float stabilityMin = float.MaxValue;
            foreach (var agent in call.assignedAgents)
            {
                stabilityMin = Mathf.Min(stabilityMin, agent.stats.mentalStability);
            }
            float stabilityFactor = Mathf.InverseLerp(2f, 10f, stabilityMin);
            float diffFactor = Mathf.InverseLerp(10f, 0f, call.definition.baseDifficulty);
            float raw = Mathf.Clamp01(avgScore * 0.7f + luckBoost * 0.2f + stabilityFactor * 0.1f);
            float shaped = successCurve.Evaluate(raw * diffFactor);
            float roll = UnityEngine.Random.value;
            return roll <= shaped;
        }

        float ScoreAgentAgainstCall(AgentController agent, DistressCallDefinition def)
        {
            float sumWeights = def.fightingWeight + def.defenseWeight + def.mobilityWeight + def.luckWeight + def.intelligenceWeight + def.charismaWeight + def.mentalStabilityWeight;
            if (sumWeights <= 0f) sumWeights = 1f;
            float score = 0f;
            score += agent.stats.fighting * def.fightingWeight;
            score += agent.stats.defense * def.defenseWeight;
            score += agent.stats.mobility * def.mobilityWeight;
            score += agent.stats.luck * def.luckWeight;
            score += agent.stats.intelligence * def.intelligenceWeight;
            score += agent.stats.charisma * def.charismaWeight;
            score += agent.stats.mentalStability * def.mentalStabilityWeight;
            score /= sumWeights * 10f;
            return Mathf.Clamp01(score);
        }
    }
}
