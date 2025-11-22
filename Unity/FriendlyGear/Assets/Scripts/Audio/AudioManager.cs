using UnityEngine;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Singleton audio manager for playing voice lines and other audio.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        /// <summary>
        /// Singleton instance of the AudioManager.
        /// </summary>
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();
                    
                    if (instance == null)
                    {
                        GameObject audioManagerObj = new GameObject("AudioManager");
                        instance = audioManagerObj.AddComponent<AudioManager>();
                    }
                }
                return instance;
            }
        }

        [Header("Audio Sources")]
        [Tooltip("AudioSource for playing voice lines")]
        public AudioSource voiceSource;

        [Header("Settings")]
        [Tooltip("Delay between consecutive voice lines in seconds")]
        [Range(0f, 5f)]
        public float voiceLineDelay = 0.5f;

        private float lastVoiceLineTime = 0f;

        private void Awake()
        {
            // Implement singleton pattern
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // Create voice source if it doesn't exist
            if (voiceSource == null)
            {
                voiceSource = gameObject.AddComponent<AudioSource>();
                voiceSource.playOnAwake = false;
                voiceSource.spatialBlend = 0f; // 2D sound
            }
        }

        /// <summary>
        /// Play a specific voice line clip.
        /// </summary>
        public void PlayVoiceLine(AudioClip clip)
        {
            if (clip == null || voiceSource == null)
                return;

            // Check if enough time has passed since last voice line
            if (Time.time - lastVoiceLineTime < voiceLineDelay)
                return;

            voiceSource.PlayOneShot(clip);
            lastVoiceLineTime = Time.time;
        }

        /// <summary>
        /// Play a random voice line from an array of clips.
        /// </summary>
        public void PlayRandomLine(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
                return;

            // Filter out null clips
            AudioClip[] validClips = System.Array.FindAll(clips, clip => clip != null);
            
            if (validClips.Length == 0)
                return;

            // Select a random clip
            int randomIndex = Random.Range(0, validClips.Length);
            PlayVoiceLine(validClips[randomIndex]);
        }

        /// <summary>
        /// Stop the current voice line.
        /// </summary>
        public void StopVoiceLine()
        {
            if (voiceSource != null && voiceSource.isPlaying)
            {
                voiceSource.Stop();
            }
        }

        /// <summary>
        /// Check if a voice line is currently playing.
        /// </summary>
        public bool IsPlayingVoiceLine()
        {
            return voiceSource != null && voiceSource.isPlaying;
        }

        /// <summary>
        /// Set the volume of the voice source.
        /// </summary>
        public void SetVoiceVolume(float volume)
        {
            if (voiceSource != null)
            {
                voiceSource.volume = Mathf.Clamp01(volume);
            }
        }
    }
}
