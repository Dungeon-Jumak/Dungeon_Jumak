using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Array of AudioSource components, sized according to the number of sounds defined in Define.Sound (e.g., Bgm and Effect)
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // Dictionary to store AudioClip objects, facilitating reuse without reloading
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");// Find Gameobject @Sound
        DontDestroyOnLoad(root); 

        // Retrieve all sound type names from the Define.Sound enum
        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>(); // Add an AudioSource to the new GameObject
            go.transform.parent = root.transform; // Set the parent of the new GameObject to the root
        }

        // Set the AudioSource for Bgm to loop
        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Clear()
    {
        // Stop and clear all audio sources
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear(); // Clear all loaded AudioClips
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        // Format the path to retrieve sound files from the Sounds directory
        path = $"Sounds/{path}";

        // If the sound type is Bgm
        if (type == Define.Sound.Bgm) 
        {
            AudioClip audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log("clip is missing");
            }

            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play(); // Play the Bgm continuously
        }
        // If the sound type is Effect
        else
        {
            AudioClip audioClip = GetOrAddAudioClip(path);
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip); // Play the effect once
        }
    }

    // Method to retrieve or load an AudioClip from the dictionary or resources
    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;

        if (!_audioClips.TryGetValue(path, out audioClip))
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path); // Load the AudioClip from resources
            _audioClips.Add(path, audioClip); // Add the AudioClip to the dictionary for future use
        }

        return audioClip;
    }
}
