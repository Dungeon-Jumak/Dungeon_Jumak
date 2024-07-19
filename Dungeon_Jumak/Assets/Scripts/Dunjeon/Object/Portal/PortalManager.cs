using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PortalManager : MonoBehaviour
{
    [Header("���� ������ �˾�")]
    [SerializeField] private GameObject returnPopup;

    [Header("���� ���н� �˾�")]
    [SerializeField] private GameObject notReturnPopup;

    [Header("Collider�� ��Ȱ��ȭ�� ��Ż ���� ������Ʈ")]
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
