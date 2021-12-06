using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTextScript : MonoBehaviour
{
    public static StoryTextScript instance;

    [SerializeField] private Text text; 

    void Awake() {
        instance = this;
    }

    void Start() {
        text.text = "";
    }

    public void ShowText(string _text, float _duration, int _nextScene) {
        StartCoroutine(IShowText(_text, _duration, _nextScene));
    }

    IEnumerator IShowText(string _text, float _duration, int _nextScene) {
        text.text = "";
        int i = 0;
        while(text.text != _text) {
            text.text += _text[i];
            i++;
            yield return new WaitForSeconds(0.08f);
        }

        yield return new WaitForSeconds(_duration);
        /*
        while(text.text != "") {
            text.text = text.text.Substring(0, text.text.Length - 1);
            yield return new WaitForSeconds(0f);
        }*/

        Global.instance.LoadScene(_nextScene);
    }

}
