using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();//�޸� ���� ���� ����� Ŭ�� ���� variable

    Dictionary<string, AudioSource> _effectSources = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioSource> _bgmSources = new Dictionary<string, AudioSource>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");//@Sound ������Ʈ ã��
        DontDestroyOnLoad(root);//Root DontDestroy

        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

        //@sound ������Ʈ ���� ������Ʈ�� BGM, Effect AudioSource ����
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>(); 
            go.transform.parent = root.transform; 
        }

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    //---�޸� Ȱ��ȭ�� ���� ����� �ҽ� Clear �Լ�---//
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

    //---BGM, Effect Play �Լ�
    public void Play(string path, Define.Sound type = Define.Sound.Effect, bool loop = false)
    {
        path = $"Sounds/{path}";//Resource ���� ���� ��� ����

        AudioClip audioClip = GetOrAddAudioClip(path);

        //@sound ������Ʈ ���� BGM AudioSource ������Ʈ�� AudioClip �߰�
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
        //@sound ������Ʈ ���� Effect AudioSource ������Ʈ�� AudioClip �߰�
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
