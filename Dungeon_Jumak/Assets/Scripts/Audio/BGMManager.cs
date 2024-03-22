using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    [SerializeField]
    private AudioClip[] bgms;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    //---싱글톤 적용---//
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack)
    {
        source.clip = bgms[_playMusicTrack];
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOutMusicCoroutine());
    }

    //---페이드 아웃을 위한 코루틴---//
    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 0.5f; i >= 0f; i-= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInMusic()
    {
        StartCoroutine(FadeInMusicCoroutine());
    }

    //---페이드 인을 위한 코루틴---//
    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= 0.3f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    public void SetVolumn(float _volume)
    {
        source.volume = _volume;
    }
    
    public void SetLoop()
    {
        source.loop = true;
    }

    public void CancelLoop()
    {
        source.loop = false;
    }
}
