using UnityEngine;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;

[System.Serializable]
public enum SoundCategory
{
    Game,
    Other
}
public enum SoundType
{
    OneShot,    // Normal OneShot sound
    Looping,    // Looping sound (e.g., background music, ambient sound)
    Stoppable   // Sound that plays once but can be manually stopped
}

[System.Serializable]
public class RandomSound
{
    public AudioClip clip;
    public float volume = 1f;
}
[System.Serializable]
public class Sound
{
    public string name;
    public bool randomPitch;
    public bool isRandom;
    [HideIf(nameof(isRandom))]
    public AudioClip clip;
    [HideIf(nameof(isRandom))]
    public float volume = 1f;
    [HideIf(nameof(isRandom))]
    public SoundType soundType;
    [ShowIf(nameof(isRandom))]
    public List<RandomSound> randomClips;

}
   
   

[System.Serializable]
public class SoundGroup
{
    // This will show as a dropdown in the Inspector.
    public SoundCategory category;
    // List of sounds for this category.
    public List<Sound> sounds;
}
public class Audio : MonoBehaviour
{
    public static Audio instance;
    public AudioSource audioSource;
    Camera cam;
    // Global main volume (adjustable in options)
    [Range(0f, 1f)]
    public float mainVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    // Instead of a flat list of sounds, now use groups.
    public List<SoundGroup> soundGroups;

    private Dictionary<string, Sound> soundDictionary;
    private Dictionary<string, AudioSource> activeSounds = new Dictionary<string, AudioSource>(); // Tracks playing sounds
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
           DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Build a dictionary for faster lookup.
     
        soundDictionary = new Dictionary<string, Sound>();

        // Loop through each group, then each sound in that group.
        foreach (SoundGroup group in soundGroups)
        {
            foreach (Sound s in group.sounds)
            {
                if (!soundDictionary.ContainsKey(s.name))
                {
                    soundDictionary.Add(s.name, s);
                }
                else
                {
                    Debug.LogWarning("Duplicate sound name found: " + s.name);
                }
            }
        }
    }

    public void PlaySound(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out Sound s))
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        float pitch = 1f;
        if (s.randomPitch)
        {
            pitch = Random.Range(0.9f, 1.1f);
        }

        AudioClip clip = GetClip(s, out float vol);

        if (s.soundType == SoundType.OneShot)
        {
            audioSource.pitch = pitch; // ✅ apply pitch
            audioSource.PlayOneShot(clip, mainVolume * vol);
            audioSource.pitch = 1f; // ✅ reset so next sound stays normal
        }
        else
        {
            AudioSource newSource = new GameObject("Sound_" + soundName).AddComponent<AudioSource>();
            newSource.clip = clip;
            newSource.volume = mainVolume * vol;
            newSource.pitch = pitch; // ✅ apply pitch here too

            if (s.soundType == SoundType.Looping)
                newSource.loop = true;

            newSource.Play();
            activeSounds[soundName] = newSource;
        }
    }
    void LateUpdate()
    {
        if(cam == null)
            cam = Camera.main;
        Vector3 pos = cam.transform.position + new Vector3(0, 0,10);
        pos.z = 0; // or whatever layer you need
        transform.position = pos;
    }
    private AudioClip GetClip(Sound s, out float vol)
    {
        if (s.isRandom && s.randomClips != null && s.randomClips.Count > 0)
        {
            RandomSound r = s.randomClips[Random.Range(0, s.randomClips.Count)];
            vol = r.volume;
            return r.clip;
        }

        vol = s.volume;
        return s.clip;
    }
    public void SetLoopVolume(string soundName, float volume)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.volume = mainVolume * volume; // volume 0–1
        }
    }
    public void StopSound(string soundName)
    {
        if (activeSounds.ContainsKey(soundName))
        {
            activeSounds[soundName].Stop();
            Destroy(activeSounds[soundName].gameObject);
            activeSounds.Remove(soundName);
        }
    }
    public void PauseGameSounds()
    {
        // Pause all sounds that are of type Loop or Stoppable when the game is paused
        foreach (var sound in activeSounds.Values)
        {
            if (sound != null)
            {
                Sound soundData = GetSoundDataFromSource(sound);  // Retrieve the sound data to check the type
                if (soundData != null && (soundData.soundType == SoundType.Looping || soundData.soundType == SoundType.Stoppable))
                {
                    sound.Pause();
                }
            }
        }
    }
    public void PauseSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Pause();
        }
    }
    public void ResumeSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Play(); // continues from pause point
        }
    }

    public void ResumeGameSounds()
    {
        // Resume all sounds that are of type Loop or Stoppable when the game is resumed
        foreach (var sound in activeSounds.Values)
        {
            if (sound != null)
            {
                Sound soundData = GetSoundDataFromSource(sound);  // Retrieve the sound data to check the type
                if (soundData != null && (soundData.soundType == SoundType.Looping || soundData.soundType == SoundType.Stoppable))
                {
                    sound.Play();
                }
            }
        }
    }
    private Sound GetSoundDataFromSource(AudioSource source)
    {
        foreach (var pair in soundDictionary)
        {
            if (pair.Value.clip == source.clip)  // Match the sound by clip
            {
                return pair.Value;
            }
        }
        return null;
    }
}
