using UnityEngine;
using UnityEngine.Events;

namespace FriendlyGear.Core
{
    /// <summary>
    /// MonoBehaviour representing an active distress call instance in the scene.
    /// Placed at the location where agents should respond.
    /// </summary>
    public class DistressCallInstance : MonoBehaviour
    {
        [Header("Definition")]
        public DistressCallDefinition definition;

        [Header("State")]
        public bool isActive = true;
        public float remainingTime;

        [Header("Events")]
        [Tooltip("Invoked when the call expires")]
        public UnityEvent onCallExpired;

        [Tooltip("Invoked when agents arrive and resolve the call")]
        public UnityEvent onCallResolved;

        private void Start()
        {
            if (definition != null)
            {
                remainingTime = definition.timeLimitSeconds;
            }
        }

        private void Update()
        {
            if (!isActive)
                return;

            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                ExpireCall();
            }
        }

        /// <summary>
        /// Mark the call as expired.
        /// </summary>
        private void ExpireCall()
        {
            isActive = false;
            remainingTime = 0f;
            
            onCallExpired?.Invoke();
            
            Debug.LogWarning($"Distress call '{definition?.title}' has expired!");
        }

        /// <summary>
        /// Mark the call as resolved by agents.
        /// </summary>
        public void ResolveCall()
        {
            if (!isActive)
                return;

            isActive = false;
            
            onCallResolved?.Invoke();
            
            Debug.Log($"Distress call '{definition?.title}' has been resolved!");
        }

        /// <summary>
        /// Get the progress (0 to 1) of the remaining time.
        /// </summary>
        public float GetTimeProgress()
        {
            if (definition == null || definition.timeLimitSeconds <= 0)
                return 0f;

            return Mathf.Clamp01(remainingTime / definition.timeLimitSeconds);
        }
    }
}
