using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PortalManager : MonoBehaviour
{
    [Header("리턴 성공시 팝업")]
    [SerializeField] private GameObject returnPopup;

    [Header("리턴 실패시 팝업")]
    [SerializeField] private GameObject notReturnPopup;

    [Header("Collider를 비활성화할 포탈 게임 오브젝트")]
    [SerializeField] private GameObject portal;

    private bool isOver30Second = false;

    private void Start()
    {
        // Starting timer for portal
        StartCoroutine(ReturnTimeCountdown());
    }

    private IEnumerator ReturnTimeCountdown()
    {
        yield return new WaitForSeconds(30f);
        isOver30Second = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (isOver30Second)
        {
            //Return Pop up Active
            returnPopup.SetActive(true);
        }
        else
        {
            //Not Return Pop up Active
            notReturnPopup.SetActive(true);
        }

        //Pause
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        returnPopup.SetActive(false);
        notReturnPopup.SetActive(false);

        Time.timeScale = 1f;
    }

    public void StopGame()
    {
        Time.timeScale = 1f;

        //Move Scene
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    public void DisableColliderForSeconds()
    {
        StartCoroutine(DisableColliderCoroutine());
    }

    private IEnumerator DisableColliderCoroutine()
    {
        BoxCollider2D collider = portal.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(3f);
            collider.enabled = true;
        }
    }
}
