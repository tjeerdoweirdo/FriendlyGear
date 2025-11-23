using FriendlyGear.Data;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class BanterSystem : MonoBehaviour
    {
        public AudioSource dispatcherAudioSource;

        void Awake()
        {
            if (!dispatcherAudioSource) dispatcherAudioSource = GetComponent<AudioSource>();
        }

        public void PlayAgentLine(AgentController agent, AudioClip[] lines)
        {
            if (agent == null || lines == null || lines.Length == 0) return;
            var clip = lines[Random.Range(0, lines.Length)];
            agent.PlayOneShot(clip);
        }

        public void PlayDispatcherLine(AudioClip[] lines)
        {
            if (!dispatcherAudioSource || lines == null || lines.Length == 0) return;
            var clip = lines[Random.Range(0, lines.Length)];
            dispatcherAudioSource.PlayOneShot(clip);
        }

        public void PlayDistressLine(DistressCallDefinition def, Vector3 position)
        {
            if (def == null || def.distressLines == null || def.distressLines.Length == 0) return;
            var clip = def.distressLines[Random.Range(0, def.distressLines.Length)];
            if (!clip) return;
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
