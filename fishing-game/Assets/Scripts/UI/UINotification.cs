using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    public static UINotification instance;

    [SerializeField] private Transform NotificationParent;
    [SerializeField] private GameObject UINotificationObject;
    [SerializeField] private GameObject SaveIcon;

    void Awake() {
        instance = this;
    }

    public void ShowNotification(string message, float duration) {
        UiNotificationInstance instance = Instantiate(UINotificationObject, NotificationParent).GetComponent<UiNotificationInstance>();
        instance.Set(message, duration);
    }

    public void ShowSaveIcon() {
        StartCoroutine(ShowSaveIconCoroutine());
    }

    IEnumerator ShowSaveIconCoroutine() {
        SaveIcon.SetActive(true);
        yield return new WaitForSeconds(5f);
        SaveIcon.SetActive(false);
    }
}
