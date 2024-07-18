using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();//메모리 관리 위한 오디오 클립 보관 variable

    Dictionary<string, AudioSource> _effectSources = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioSource> _bgmSources = new Dictionary<string, AudioSource>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");//@Sound 오브젝트 찾기
        DontDestroyOnLoad(root);//Root DontDestroy

        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

        //@sound 오브젝트 하위 오브젝트로 BGM, Effect AudioSource 설정
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>(); 
            go.transform.parent = root.transform; 
        }

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    //---메모리 활성화를 위한 오디오 소스 Clear 함수---//
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear(); // Clear all loaded AudioClips

        foreach (var audioSource in _effectSources.Values)
        {
            Destroy(audioSource.gameObject);
        }
        _effectSources.Clear();
        _bgmSources.Clear();
    }

    //---BGM, Effect Play 함수
    public void Play(string path, Define.Sound type = Define.Sound.Effect, bool loop = false)
    {
        path = $"Sounds/{path}";//Resource 폴더 내의 경로 설정

        AudioClip audioClip = GetOrAddAudioClip(path);

        //@sound 오브젝트 하위 BGM AudioSource 오브젝트에 AudioClip 추가
        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            audioSource.clip = audioClip;
            audioSource.loop = loop;

            string mixerPath = $"Audio/Audio Mixer";
            AudioMixer audioMixer = Resources.Load<AudioMixer>(mixerPath);

            AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("BGM");

            audioSource.outputAudioMixerGroup = mixerGroups[0];

            audioSource.Play(); 

            _bgmSources[path] = audioSource;
        }
        //@sound 오브젝트 하위 Effect AudioSource 오브젝트에 AudioClip 추가
        else if (type == Define.Sound.Effect)
        {
            AudioSource audioSource = new GameObject("SFX").AddComponent<AudioSource>();
            audioSource.transform.parent = GameObject.Find("@Sound").transform;
            audioSource.clip = audioClip;

            string mixerPath = $"Audio/Audio Mixer";
            AudioMixer audioMixer = Resources.Load<AudioMixer>(mixerPath);

            AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("SFX");

            audioSource.outputAudioMixerGroup = mixerGroups[0];

            _effectSources[path] = audioSource;

            if (loop)
            {
                audioSource.loop = loop;
                audioSource.Play();
            }
            else if(!loop)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    IEnumerator DestroyAudioSourceAfterPlayback(AudioSource audioSource, float length, string path)
    {
        yield return new WaitForSeconds(length);
        if (audioSource != null)
        {
            Destroy(audioSource.gameObject);
            _effectSources.Remove(path);
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

    public void Pause(string path, Define.Sound type)
    {
        path = $"Sounds/{path}";

        if (type == Define.Sound.Bgm)
        {
            if (_bgmSources.TryGetValue(path, out AudioSource audioSource))
            {
                audioSource.Pause();
            }
        }
        else
        {
            if (_effectSources.TryGetValue(path, out AudioSource audioSource))
            {
                audioSource.Pause();
            }
        }
    }

    public void Resume(string path, Define.Sound type)
    {
        path = $"Sounds/{path}";

        if (type == Define.Sound.Bgm)
        {
            if (_bgmSources.TryGetValue(path, out AudioSource audioSource))
            {
                audioSource.UnPause();
            }
        }
        else
        {
            if (_effectSources.TryGetValue(path, out AudioSource audioSource))
            {
                audioSource.UnPause();
            }
        }
    }
}
