using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public static Global instance;
    private bool paused = false;

    [SerializeField] private int TextScene = 1;

    void Awake() {
        if(instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SetFPSMouse(bool status) {
        Cursor.visible = !status;
        Cursor.lockState = status ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update() {
        if(Input.GetButtonDown("Cancel")) {
            SetFPSMouse(paused);
            paused = !paused;
        }
    }

    public void LoadScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }

    public void PlayTextScene(string _text, float _duration, int _nextScene) {
        StartCoroutine(IPlayTextScene(_text, _duration, _nextScene));
    }

    IEnumerator IPlayTextScene(string _text, float _duration, int _nextScene) {
        LoadScene(TextScene);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        StoryTextScript.instance.ShowText(_text, _duration, _nextScene);
    }
}
