using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiNotificationInstance : MonoBehaviour
{
    [SerializeField] private Text text;
    private CanvasGroup group;
    private float duration;

    public void Set(string _message, float _duration) {
        text.text = _message;
        duration = _duration;
    }

    void Start() {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        StartCoroutine(ShowText());
    }

    void Update() {
        
    }

    IEnumerator ShowText() {
        yield return null;
        GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;     // fix it
        group.alpha = 1;

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
