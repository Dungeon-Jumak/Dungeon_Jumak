using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireTouch : MonoBehaviour
{
    public Text sizeText;
    public Image fireImage;
    private int touchCount = 0;

    public Fire fire;

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string fireClickSound;
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            touchCount++;

            if (!audioManager.IsPlaying(fireClickSound))
            {
                audioManager.Play(fireClickSound);
                audioManager.Setvolume(fireClickSound, 0.5f);
            }

            if (touchCount % 3 == 0 && data.fireSize <= 100)
            {
                fire.IncreaseFireSize();
            }
        }
    }
}
