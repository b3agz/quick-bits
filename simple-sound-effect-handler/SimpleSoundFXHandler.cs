using System.Collections.Generic;
using UnityEngine;

public class SoundFXHandler : MonoBehaviour {

    [Tooltip("The maximum number of AudioSource objects you want SoundFXManager to spawn.")]
    public int maxSources = 5;
    public SoundFX[] sounds;
    private List<AudioSource> audioSources = new List<AudioSource>();

    private static SoundFXHandler _instance;
    public static SoundFXHandler Instance { get { return _instance; } }

    private void Awake() {

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

    }

    public void PlaySound (string fxName, Vector3 location) {

        // Get our AudioSource.
        AudioSource audioSource = GetAudioSource();

        // Make sure we have an AudioSource before continuing.
        if (audioSource == null)
            return;

        // If we have an AudioSource, we then need to find our SoundFX.
        foreach (SoundFX soundFX in sounds) {

            // If we have a matching SoundFX, move our AudioSource into position, attach it to the SoundFX, and return.
            if (fxName == soundFX.name) {

                audioSource.transform.position = location;
                soundFX.Attach(audioSource);
                return;

            }

        }

        // If we get here, we didn't find a SoundFX matching the name being passed to us.
        Debug.LogWarning("Could not find SoundFX, \"" + fxName + " \" in sounds array.");

    }

    /*
     * Finds the next available (not playing) audiosource and returns it. If none are available, it can spawn a new
     * one if we are under the limit.
     */
    private AudioSource GetAudioSource () {

        // Loop through all the AudioSources we already have.
        foreach (AudioSource source in audioSources) {

            // If the current source isn't playing any audio, we treat it as available and return it.
            if (!source.isPlaying)
                return source;

        }

        // If we get to here, we didn't find an available AudioSources, so check if we're still under the max sources limit.
        if (audioSources.Count < maxSources) {

            // If we are still under the limit, we can create a new AudioSource and add it to our list.
            GameObject newAudioSource = new GameObject();
            newAudioSource.transform.SetParent(transform);
            audioSources.Add(newAudioSource.AddComponent<AudioSource>());
            return audioSources[audioSources.Count - 1];

        }

        // And if we get here, there are no available audio sources left and we can't make any more. Return null and throw up a warning.
        Debug.LogWarning("No available audio sources found and maximum number of audio sources reached.");
        return null;

    }

}

[System.Serializable]
public class SoundFX {

    #region Initialise variables.
    // All variables declared along with default values.
    [Header("FX Details")]
    public string name = "New SoundFX";
    public AudioClip clip = null;

    // Any settings that you want to change for a particular SoundFX go here.
    [Header("FX Settings")]
    public bool loop = false;
    [Range(0f, 1f)]
    public float volume = 1f;
    #endregion

    // Initialise using only an audio clip.
    public SoundFX (AudioClip _clip) {

        clip = _clip;

    }

    // Initialise with a name and audio clip.
    public SoundFX (string _name, AudioClip _clip) {

        name = _name;
        clip = _clip;

    }

    // Attach this soundfx to the AudioSource being passed here, setting any variables in the process.
    public void Attach (AudioSource audioSource) {

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;

        audioSource.Play();

    }

}
