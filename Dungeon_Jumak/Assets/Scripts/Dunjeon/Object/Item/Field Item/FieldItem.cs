using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [Header("[자석] VFX")]
    public ParticleSystem magnetic;    
    
    [Header("[자석] 오브젝트")]
    public GameObject magneticObj;

    /*[Header("필드 아이템")]
    [SerializeField] List<GameObject>[] fieldItem;*/

    public void Start()
    {
        //magnetic = GetComponent<ParticleSystem>(); // GetComponent를 magnetic에서 직접 호출
    }

    /*private void SpawnFieldItem()
    {
        //Base Position
        Vector3 basePosition = new Vector3(0, 0, 0);

        //Random spawn 
        for (int i = 0; i < num; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(basePosition.x - 15f, basePosition.x + 15f),
                Random.Range(basePosition.y - 15f, basePosition.y + 15f),
                basePosition.z
            );

        }

    }*/

    //Pick Up Item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check Player
        if (!collision.CompareTag("Player"))
            return;

        //Active item
        magnetic.Play();

        StartCoroutine(NonActiveItem());

        //Play SFX
        GameManager.Sound.Play("[S] Pick Up Item", Define.Sound.Effect, false);

        //Nonactive item
        magneticObj.SetActive(false);
    }

    private IEnumerator NonActiveItem()
    {
        yield return new WaitForSeconds(5f);

        Debug.Log("sds");

        //Nonactive VFX
        magnetic.Stop();
    }
}
