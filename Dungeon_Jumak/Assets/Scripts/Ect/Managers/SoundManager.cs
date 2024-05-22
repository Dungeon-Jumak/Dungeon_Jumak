using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount]; //bgm, effect 2개 오디오소스 생성
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); // 추가된 부분: AudioClip을 저장할 Dictionary

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        DontDestroyOnLoad(root);

        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
       path = $"Sounds/{path}";

        //bgm일 때
        if(type == Define.Sound.Bgm)
        {
            //AudioClip audioClip = GameManager.Resource.Load<AudioClip>(path);
            AudioClip audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log("clip is missing");
            }
            
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        //effect일 때
        else
        {
            //AudioClip audioClip = GameManager.Resource.Load<AudioClip>(path);
            AudioClip audioClip = GetOrAddAudioClip(path);
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
    
    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if(_audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
            if (audioClip == null) Debug.Log("false");
            _audioClips.Add(path, audioClip);
        }
        return audioClip;
    }
}
