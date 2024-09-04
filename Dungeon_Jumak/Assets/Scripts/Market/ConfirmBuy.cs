using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBuy : MonoBehaviour
{
    public GameObject confirmPanel;
    public Button confirmButton;
    public Button cancelButton;
    public TextMeshProUGUI messageText;

    private System.Action onConfirm;
    private System.Action onCancel;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        confirmPanel.SetActive(false); // 패널을 기본적으로 비활성화
    }

    public void Show(string message, System.Action onConfirmCallback, System.Action onCancelCallback)
    {
        messageText.text = message;
        onConfirm = onConfirmCallback;
        onCancel = onCancelCallback;
        confirmPanel.SetActive(true);
    }

    private void OnConfirm()
    {
        onConfirm?.Invoke();
        confirmPanel.SetActive(false);
    }

    private void OnCancel()
    {
        onCancel?.Invoke();
        confirmPanel.SetActive(false);
    }
}
